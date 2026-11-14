using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MusicBeePlugin
{
    public static class EnhancedLrcParser
    {
        private static readonly Regex HeaderTagRegex = new Regex(@"^\[(?<key>[a-zA-Z]+):(?<value>.*)\]$", RegexOptions.Compiled);
        private static readonly Regex LineTimestampRegex = new Regex(@"^(?<stamps>(?:\[\d{1,2}:\d{2}(?:\.\d{1,3})?\])*)(?<text>.*)$", RegexOptions.Compiled);

        public static readonly Regex SingleStampRegex = new Regex(@"\[(?<ts>\d{1,2}:\d{2}(?:\.\d{1,3})?)\]", RegexOptions.Compiled);
        public static readonly Regex WordTimestampRegex = new Regex(@"<(?<min>\d+):(?<sec>\d+(?:\.\d+)?)>(?<text>[^<]*)", RegexOptions.Compiled);

        public static readonly TimeSpan LastLineDuration = TimeSpan.FromSeconds(5);

        public static ParsedLyrics Parse(string rawLrc)
        {
            var result = new ParsedLyrics();
            if (string.IsNullOrWhiteSpace(rawLrc))
            {
                return result;
            }

            var lines = rawLrc.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmedLine))
                {
                    continue;
                }

                var metaMatch = HeaderTagRegex.Match(trimmedLine);
                if (metaMatch.Success)
                {
                    var key = metaMatch.Groups["key"].Value.ToLowerInvariant();
                    var value = metaMatch.Groups["value"].Value;
                    result.Metadata[key] = value;

                    if (key == "offset" &&
                        double.TryParse(value.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var offset))
                    {
                        result.Offset = TimeSpan.FromMilliseconds(offset);
                    }

                    continue;
                }

                var lineMatch = LineTimestampRegex.Match(trimmedLine);
                if (!lineMatch.Success)
                {
                    continue;
                }

                var starts = new List<TimeSpan>();
                foreach (Match match in SingleStampRegex.Matches(lineMatch.Groups["stamps"].Value))
                {
                    var timestamp = match.Groups["ts"].Value;
                    starts.Add(ParseTimestamp(timestamp));
                }

                if (starts.Count == 0)
                {
                    continue;
                }

                var content = lineMatch.Groups["text"].Value;
                var templateWords = ParseWords(content, starts[0]);

                foreach (var lineStart in starts)
                {
                    var shift = lineStart - starts[0];
                    var lyricLine = new LyricLine
                    {
                        StartTime = lineStart,
                    };

                    foreach (var word in templateWords)
                    {
                        lyricLine.Words.Add(new LyricWord
                        {
                            Text = word.Text,
                            StartTime = word.StartTime + shift,
                        });
                    }

                    var fullText = new StringBuilder();
                    foreach (var word in lyricLine.Words)
                    {
                        fullText.Append(word.Text);
                    }

                    lyricLine.FullText = fullText.ToString();
                    result.Lines.Add(lyricLine);
                }
            }

            result.Lines.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));

            ApplyOffsets(result);
            AssignEndTimes(result);
            return result;
        }

        private static List<LyricWord> ParseWords(string content, TimeSpan lineStart)
        {
            var words = new List<LyricWord>();
            var wordMatches = WordTimestampRegex.Matches(content);

            if (wordMatches.Count == 0)
            {
                words.Add(new LyricWord
                {
                    Text = content,
                    StartTime = lineStart,
                });
                return words;
            }

            var leading = content.Substring(0, wordMatches[0].Index);
            if (leading.Length > 0)
            {
                words.Add(new LyricWord
                {
                    Text = leading,
                    StartTime = lineStart,
                });
            }

            foreach (Match wm in wordMatches)
            {
                var text = wm.Groups["text"].Value;
                var startTime = ParseTimestamp(wm.Groups["min"].Value, wm.Groups["sec"].Value);
                words.Add(new LyricWord
                {
                    Text = text,
                    StartTime = startTime,
                });
            }

            return words;
        }

        private static void ApplyOffsets(ParsedLyrics result)
        {
            if (result.Offset == TimeSpan.Zero)
            {
                return;
            }

            foreach (var line in result.Lines)
            {
                line.StartTime = Clamp(line.StartTime - result.Offset);
                foreach (var word in line.Words)
                {
                    word.StartTime = Clamp(word.StartTime - result.Offset);
                }
            }
        }

        private static void AssignEndTimes(ParsedLyrics result)
        {
            for (var i = 0; i < result.Lines.Count; i++)
            {
                var line = result.Lines[i];
                if (i + 1 < result.Lines.Count)
                {
                    line.EndTime = result.Lines[i + 1].StartTime;
                }
                else
                {
                    var lastStart = line.Words.Count > 0
                        ? line.Words[line.Words.Count - 1].StartTime
                        : line.StartTime;
                    line.EndTime = (lastStart > line.StartTime ? lastStart : line.StartTime) + LastLineDuration;
                }

                for (var w = 0; w < line.Words.Count; w++)
                {
                    line.Words[w].EndTime = w + 1 < line.Words.Count
                        ? line.Words[w + 1].StartTime
                        : line.EndTime;
                }
            }
        }

        private static TimeSpan Clamp(TimeSpan time)
        {
            return time < TimeSpan.Zero ? TimeSpan.Zero : time;
        }

        private static TimeSpan ParseTimestamp(string timestamp)
        {
            var parts = timestamp.Split(':');
            var minutes = int.Parse(parts[0], CultureInfo.InvariantCulture);
            var seconds = double.Parse(parts[1], CultureInfo.InvariantCulture);
            return TimeSpan.FromMinutes(minutes) + TimeSpan.FromSeconds(seconds);
        }

        private static TimeSpan ParseTimestamp(string minutes, string seconds)
        {
            var parsedMinutes = int.Parse(minutes, CultureInfo.InvariantCulture);
            var parsedSeconds = double.Parse(seconds, CultureInfo.InvariantCulture);
            return TimeSpan.FromMinutes(parsedMinutes) + TimeSpan.FromSeconds(parsedSeconds);
        }
    }
}