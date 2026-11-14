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
        private static readonly Regex LineTimestampRegex = new Regex(@"^\[(?<timestamp>\d{1,2}:\d{2}(?:\.\d{1,3})?)\](?<text>.*)$", RegexOptions.Compiled);

        public static readonly Regex SingleStampRegex = new Regex(@"\[(?<ts>\d+:\d{2}(?:\.\d{1,3})?)\]", RegexOptions.Compiled);
        public static readonly Regex WordTimestampRegex = new Regex(@"<(?<min>\d+):(?<sec>\d+(?:\.\d+)?)>(?<text>[^<]*)", RegexOptions.Compiled);

        public static ParsedLyrics Parse(string rawLrc)
        {
            var result = new ParsedLyrics();
            if (string.IsNullOrWhiteSpace(rawLrc))
            {
                return result;
            }
            string[] lines = rawLrc.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                string trimmedLine = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmedLine))
                {
                    continue;
                }

                //checking for metadata tags here 
                var metaMatch = HeaderTagRegex.Match(trimmedLine);
                if (metaMatch.Success)
                {
                    string key = metaMatch.Groups["key"].Value.ToLowerInvariant();
                    string value = metaMatch.Groups["value"].Value;
                    result.Metadata[key] = value;

                    if (key == "offset" && double.TryParse(value, out double offset))
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
                foreach (Match sm in SingleStampRegex.Matches(lineMatch.Groups["stamps"].Value))
                {
                    starts.Add(ParseTimestamp(sm.Groups["ts"].Value));
                }

                string content = lineMatch.Groups["text"].Value;
                var templateWords = ParseWords(content, starts[0]);

                foreach (TimeSpan linestart in starts)
                {
                    TimeSpan shift = linestart - starts[0];
                    var lyricLine = new LyricLine
                    {
                        StartTime = lineStart
                    }
                }



            }

            return result;
        }

        private static TimeSpan ParseTimestamp(string timestamp)
        {
            string[] parts = timestamp.Split(':');
            int minutes = int.Parse(parts[0], CultureInfo.InvariantCulture);
            double seconds = double.Parse(parts[1], CultureInfo.InvariantCulture);
            return TimeSpan.FromMinutes(minutes) + TimeSpan.FromSeconds(seconds);
        }

        private static TimeSpan ParseTimestamp(string minutes, string seconds)
        {
            int parsedMinutes = int.Parse(minutes, CultureInfo.InvariantCulture);
            double parsedSeconds = double.Parse(seconds, CultureInfo.InvariantCulture);
            return TimeSpan.FromMinutes(parsedMinutes) + TimeSpan.FromSeconds(parsedSeconds);
        }
    }
}