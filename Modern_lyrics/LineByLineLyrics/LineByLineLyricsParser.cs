using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MusicBeePlugin
{
    public static class EnhancedLrcParser
    {
        // [ar:Artist], [ti:Title], [offset:+500] ...
        private static readonly Regex HeaderTagRegex = new Regex(
            @"^\[(?<key>[a-zA-Z]+):(?<value>.*)\]$", RegexOptions.Compiled);

        // One or more leading timestamps followed by the text: [00:12.00][01:30.00]text
        private static readonly Regex LineRegex = new Regex(
            @"^(?<stamps>(?:\[\d+:\d{2}(?:\.\d{1,3})?\])+)(?<text>.*)$", RegexOptions.Compiled);

        private static readonly Regex SingleStampRegex = new Regex(
            @"\[(?<ts>\d+:\d{2}(?:\.\d{1,3})?)\]", RegexOptions.Compiled);

        private static readonly TimeSpan LastLineDuration = TimeSpan.FromSeconds(5);

        public static ParsedLyrics Parse(string rawLrc)
        {
            var result = new ParsedLyrics();
            if (string.IsNullOrWhiteSpace(rawLrc))
            {
                return result;
            }

            string[] lines = rawLrc.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                if (trimmed.Length == 0)
                {
                    continue;
                }

                var metaMatch = HeaderTagRegex.Match(trimmed);
                if (metaMatch.Success)
                {
                    string key = metaMatch.Groups["key"].Value.ToLowerInvariant();
                    string value = metaMatch.Groups["value"].Value;
                    result.Metadata[key] = value;

                    if (key == "offset" &&
                        double.TryParse(value.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double offsetMs))
                    {
                        result.Offset = TimeSpan.FromMilliseconds(offsetMs);
                    }
                    continue;
                }

                var lineMatch = LineRegex.Match(trimmed);
                if (!lineMatch.Success)
                {
                    continue;
                }

                string text = lineMatch.Groups["text"].Value.Trim();

                // one entry per timestamp, so [00:12.00][01:30.00]text becomes two lines
                foreach (Match sm in SingleStampRegex.Matches(lineMatch.Groups["stamps"].Value))
                {
                    result.Lines.Add(new LyricLine
                    {
                        StartTime = ParseTimestamp(sm.Groups["ts"].Value),
                        Text = text
                    });
                }
            }

            result.Lines.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));

            ApplyOffset(result);
            AssignEndTimes(result);
            return result;
        }

        // [offset:+500] = lyrics appear 500 ms sooner, so subtract it.
        private static void ApplyOffset(ParsedLyrics result)
        {
            if (result.Offset == TimeSpan.Zero)
            {
                return;
            }

            foreach (var line in result.Lines)
            {
                TimeSpan shifted = line.StartTime - result.Offset;
                line.StartTime = shifted < TimeSpan.Zero ? TimeSpan.Zero : shifted;
            }
        }

        private static void AssignEndTimes(ParsedLyrics result)
        {
            for (int i = 0; i < result.Lines.Count; i++)
            {
                var line = result.Lines[i];
                line.EndTime = i + 1 < result.Lines.Count
                    ? result.Lines[i + 1].StartTime
                    : line.StartTime + LastLineDuration;
            }
        }

        private static TimeSpan ParseTimestamp(string timestamp)
        {
            string[] parts = timestamp.Split(':');
            int minutes = int.Parse(parts[0], CultureInfo.InvariantCulture);
            double seconds = double.Parse(parts[1], CultureInfo.InvariantCulture);
            return TimeSpan.FromMinutes(minutes) + TimeSpan.FromSeconds(seconds);
        }
    }
}