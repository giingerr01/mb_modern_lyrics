using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using MusicBeePlugin.Models;

namespace MusicBeePlugin.Parsing
{
    public static class EnhancedLyricsParser
    {
        // "v1:", "v2:" ... at the start of a line's text (after any [mm:ss.xx] stamp).
        private static readonly Regex VoiceTag = new Regex(@"^(v\d+)\s*:\s*",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        // If a line has no explicit end tag, its last word can't be stretched longer than this
        // (otherwise an instrumental gap would make the last word fill slowly across the gap).
        private static readonly TimeSpan MaxImpliedLastWordDuration = TimeSpan.FromSeconds(2);

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

                string text = sourceLine.Substring(contentIndex).Trim();

                string voice;
                text = StripVoiceTag(text, out voice);

                // No [mm:ss.xx] line stamp? Fall back to the first word's <mm:ss.xxx> tag as the
                // line start. Metadata lines like [ar:Artist] don't start with '<', so they're skipped.
                if (lineTimestamps.Count == 0)
                {
                    TimeSpan firstWordTimestamp;
                    if (!TryReadLeadingWordTimestamp(text, offsetMilliseconds, out firstWordTimestamp))
                    {
                        continue;
                    }

                    lineTimestamps.Add(firstWordTimestamp);
                }

                if (text.Length == 0)
                {
                    continue;
                }

                for (int timestampIndex = 0; timestampIndex < lineTimestamps.Count; timestampIndex++)
                {
                    TimeSpan? explicitEnd;
                    List<LyricWord> words = ParseWords(text, lineTimestamps[timestampIndex], offsetMilliseconds, out explicitEnd);
                    if (words.Count == 0)
                    {
                        continue;
                    }

                    lines.Add(new TimedLine(lineTimestamps[timestampIndex], words, lines.Count, voice, explicitEnd));
                }
            }

            lines.Sort(delegate (TimedLine left, TimedLine right)
            {
                int timestampComparison = left.Start.CompareTo(right.Start);
                return timestampComparison != 0 ? timestampComparison : left.SourceOrder.CompareTo(right.SourceOrder);
            });

            List<LyricsLine> result = new List<LyricsLine>(lines.Count);
            for (int lineIndex = 0; lineIndex < lines.Count; lineIndex++)
            {
                TimedLine timedLine = lines[lineIndex];
                TimeSpan lastWordStart = timedLine.Words[timedLine.Words.Count - 1].Start;

                TimeSpan lineEnd;
                if (timedLine.ExplicitEnd.HasValue)
                {
                    // The file told us exactly when the last word ends.
                    lineEnd = timedLine.ExplicitEnd.Value;
                }
                else
                {
                    TimeSpan nextStart;
                    if (lineIndex + 1 < lines.Count)
                    {
                        nextStart = lines[lineIndex + 1].Start;
                    }
                    else if (trackDuration.HasValue && trackDuration.Value > timedLine.Start)
                    {
                        nextStart = trackDuration.Value;
                    }
                    else
                    {
                        nextStart = lastWordStart + MaxImpliedLastWordDuration;
                    }

                    lineEnd = nextStart;

                    // Only cap real word-timed lines; a plain line-synced line (one word, no tags)
                    // legitimately lasts until the next line.
                    if (timedLine.Words.Count > 1)
                    {
                        TimeSpan cap = lastWordStart + MaxImpliedLastWordDuration;
                        if (cap < lineEnd)
                        {
                            lineEnd = cap;
                        }
                    }
                }

                if (lineEnd < lastWordStart)
                {
                    lineEnd = lastWordStart;
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

                LyricsLine line = new LyricsLine(timedLine.Words, timedLine.Start, lineEnd);
                line.Voice = timedLine.Voice;
                result.Add(line);
            }

            return result;
        }

        private static string StripVoiceTag(string text, out string voice)
        {
            Match match = VoiceTag.Match(text);
            if (!match.Success)
            {
                voice = null;
                return text;
            }

            voice = match.Groups[1].Value.ToLowerInvariant();
            return text.Substring(match.Length);
        }

        private static bool TryReadLeadingWordTimestamp(string text, long offsetMilliseconds, out TimeSpan result)
        {
            result = TimeSpan.Zero;
            if (text.Length == 0 || text[0] != '<')
            {
                return false;
            }

            int closingBracket = text.IndexOf('>');
            if (closingBracket < 0)
            {
                return false;
            }

            TimeSpan timestamp;
            if (!TimestampParser.TryParseTimestamp(text.Substring(1, closingBracket - 1), out timestamp))
            {
                return false;
            }

            return TryApplyOffset(timestamp, offsetMilliseconds, out result);
        }

        private static List<LyricWord> ParseWords(string text, TimeSpan lineStart, long offsetMilliseconds, out TimeSpan? explicitEnd)
        {
            explicitEnd = null;
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

            string tail = text.Substring(segmentStart);
            if (tail.Trim().Length == 0)
            {
                // Nothing after the last tag: it's an end-of-line marker ("uh<00:04.050>"),
                // i.e. the time the last word stops.
                explicitEnd = currentStart;
            }
            else
            {
                AddWord(words, tail, currentStart);
            }

            return words;
        }

        private static void AddWord(List<LyricWord> words, string text, TimeSpan start)
        {
            string trimmedText = text.Trim();
            if (trimmedText.Length > 0)
            {
                LyricWord word = new LyricWord(trimmedText, start, start);
                word.TrailingSpace = char.IsWhiteSpace(text[text.Length - 1]);
                words.Add(word);
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
            public string Voice { get; private set; }
            public TimeSpan? ExplicitEnd { get; private set; }

            public TimedLine(TimeSpan start, List<LyricWord> words, int sourceOrder, string voice, TimeSpan? explicitEnd)
            {
                Start = start;
                Words = words;
                SourceOrder = sourceOrder;
                Voice = voice;
                ExplicitEnd = explicitEnd;
            }
        }
    }
}