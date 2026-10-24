using System;
using System.Collections.Generic;
using System.Globalization;
using MusicBeePlugin.Models;

namespace MusicBeePlugin.Parsing
{
    public static class EnhancedLyricsParser
    {
        public static List<LyricsLine> Parse(string lyrics, TimeSpan? trackDuration = null)
        {
            if (lyrics == null)
            {
                throw new ArgumentNullException("lyrics");
            }

            if (trackDuration.HasValue && trackDuration.Value < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("trackDuration");
            }

            long offsetMilliseconds = ReadOffset(lyrics);
            List<TimedLine> lines = new List<TimedLine>();
            string[] sourceLines = lyrics.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

            for (int lineIndex = 0; lineIndex < sourceLines.Length; lineIndex++)
            {
                string sourceLine = sourceLines[lineIndex].TrimStart('\uFEFF');
                int contentIndex = 0;
                List<TimeSpan> lineTimestamps = new List<TimeSpan>();

                while (contentIndex < sourceLine.Length && sourceLine[contentIndex] == '[')
                {
                    int closingBracket = sourceLine.IndexOf(']', contentIndex + 1);
                    if (closingBracket < 0)
                    {
                        break;
                    }

                    TimeSpan timestamp;
                    string timestampText = sourceLine.Substring(contentIndex + 1, closingBracket - contentIndex - 1);
                    if (!TimestampParser.TryParseTimestamp(timestampText, out timestamp))
                    {
                        break;
                    }

                    TimeSpan adjustedTimestamp;
                    if (TryApplyOffset(timestamp, offsetMilliseconds, out adjustedTimestamp))
                    {
                        lineTimestamps.Add(adjustedTimestamp);
                    }

                    contentIndex = closingBracket + 1;
                }

                if (lineTimestamps.Count == 0)
                {
                    continue;
                }

                string text = sourceLine.Substring(contentIndex).Trim();
                if (text.Length == 0)
                {
                    continue;
                }

                for (int timestampIndex = 0; timestampIndex < lineTimestamps.Count; timestampIndex++)
                {
                    List<LyricWord> words = ParseWords(text, lineTimestamps[timestampIndex], offsetMilliseconds);
                    lines.Add(new TimedLine(lineTimestamps[timestampIndex], words, lines.Count));
                }
            }

            lines.Sort(delegate(TimedLine left, TimedLine right)
            {
                int timestampComparison = left.Start.CompareTo(right.Start);
                return timestampComparison != 0 ? timestampComparison : left.SourceOrder.CompareTo(right.SourceOrder);
            });

            List<LyricsLine> result = new List<LyricsLine>(lines.Count);
            for (int lineIndex = 0; lineIndex < lines.Count; lineIndex++)
            {
                TimedLine timedLine = lines[lineIndex];
                TimeSpan lineEnd = timedLine.Start;
                if (lineIndex + 1 < lines.Count)
                {
                    lineEnd = lines[lineIndex + 1].Start;
                }
                else if (trackDuration.HasValue && trackDuration.Value > timedLine.Start)
                {
                    lineEnd = trackDuration.Value;
                }

                if (timedLine.Words.Count > 0 &&
                    timedLine.Words[timedLine.Words.Count - 1].Start > lineEnd)
                {
                    lineEnd = timedLine.Words[timedLine.Words.Count - 1].Start;
                }

                for (int wordIndex = 0; wordIndex < timedLine.Words.Count; wordIndex++)
                {
                    LyricWord word = timedLine.Words[wordIndex];
                    TimeSpan wordEnd = lineEnd;
                    if (wordIndex + 1 < timedLine.Words.Count)
                    {
                        wordEnd = timedLine.Words[wordIndex + 1].Start;
                    }

                    if (wordEnd < word.Start)
                    {
                        wordEnd = word.Start;
                    }

                    word.End = wordEnd;
                }

                result.Add(new LyricsLine(timedLine.Words, timedLine.Start, lineEnd));
            }

            return result;
        }

        private static List<LyricWord> ParseWords(string text, TimeSpan lineStart, long offsetMilliseconds)
        {
            List<LyricWord> words = new List<LyricWord>();
            int markerIndex = text.IndexOf('<');
            if (markerIndex < 0)
            {
                words.Add(new LyricWord(text, lineStart, lineStart));
                return words;
            }

            TimeSpan currentStart = lineStart;
            int segmentStart = 0;
            bool foundTimestamp = false;

            while (markerIndex >= 0)
            {
                int closingBracket = text.IndexOf('>', markerIndex + 1);
                if (closingBracket < 0)
                {
                    break;
                }

                TimeSpan timestamp;
                TimeSpan adjustedTimestamp;
                string timestampText = text.Substring(markerIndex + 1, closingBracket - markerIndex - 1);
                if (!TimestampParser.TryParseTimestamp(timestampText, out timestamp) ||
                    !TryApplyOffset(timestamp, offsetMilliseconds, out adjustedTimestamp))
                {
                    markerIndex = text.IndexOf('<', markerIndex + 1);
                    continue;
                }

                AddWord(words, text.Substring(segmentStart, markerIndex - segmentStart), currentStart);
                currentStart = adjustedTimestamp;
                segmentStart = closingBracket + 1;
                foundTimestamp = true;
                markerIndex = text.IndexOf('<', segmentStart);
            }

            if (!foundTimestamp)
            {
                words.Add(new LyricWord(text, lineStart, lineStart));
                return words;
            }

            AddWord(words, text.Substring(segmentStart), currentStart);
            return words;
        }

        private static void AddWord(List<LyricWord> words, string text, TimeSpan start)
        {
            string trimmedText = text.Trim();
            if (trimmedText.Length > 0)
            {
                words.Add(new LyricWord(trimmedText, start, start));
            }
        }

        private static long ReadOffset(string lyrics)
        {
            long offsetMilliseconds = 0;
            string[] sourceLines = lyrics.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

            for (int lineIndex = 0; lineIndex < sourceLines.Length; lineIndex++)
            {
                string sourceLine = sourceLines[lineIndex].Trim();
                if (!sourceLine.StartsWith("[offset:", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                int closingBracket = sourceLine.IndexOf(']');
                if (closingBracket < 0)
                {
                    continue;
                }

                string value = sourceLine.Substring(8, closingBracket - 8).Trim();
                long parsedOffset;
                if (long.TryParse(value, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out parsedOffset))
                {
                    offsetMilliseconds = parsedOffset;
                }
            }

            return offsetMilliseconds;
        }

        private static bool TryApplyOffset(TimeSpan timestamp, long offsetMilliseconds, out TimeSpan adjustedTimestamp)
        {
            try
            {
                long offsetTicks = checked(offsetMilliseconds * TimeSpan.TicksPerMillisecond);
                adjustedTimestamp = TimeSpan.FromTicks(checked(timestamp.Ticks + offsetTicks));
                return true;
            }
            catch (OverflowException)
            {
                adjustedTimestamp = TimeSpan.Zero;
                return false;
            }
        }

        private sealed class TimedLine
        {
            public TimeSpan Start { get; private set; }
            public List<LyricWord> Words { get; private set; }
            public int SourceOrder { get; private set; }

            public TimedLine(TimeSpan start, List<LyricWord> words, int sourceOrder)
            {
                Start = start;
                Words = words;
                SourceOrder = sourceOrder;
            }
        }
    }
}