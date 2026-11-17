using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace MusicBeePlugin
{
    // Standard line-by-line LRC
    //   [ar:Artist] [ti:Title] [offset:+500]
    //   [00:12.00]First line
    //   [00:15.50][01:05.20]Line that repeats
    public static class LrcParser
    {
        // [ar:Artist], [ti:Title], [offset:+500] ... (key is letters only, so timestamps never match)
        private static readonly Regex HeaderTagRegex = new Regex(
            @"^\[(?<key>[a-zA-Z]+):(?<value>.*)\]$", RegexOptions.Compiled);

        // One or more leading timestamps followed by the text.
        // Fraction separator may be '.' or ':' ([mm:ss.xx] or [mm:ss:xx]), fraction is optional.
        private static readonly Regex LineRegex = new Regex(
            @"^(?<stamps>(?:\[\d+:\d{2}(?:[.:]\d{1,3})?\])+)(?<text>.*)$", RegexOptions.Compiled);

        private static readonly Regex SingleStampRegex = new Regex(
            @"\[(?<ts>\d+:\d{2}(?:[.:]\d{1,3})?)\]", RegexOptions.Compiled);

        private static readonly TimeSpan LastLineDuration = TimeSpan.FromSeconds(5);

        private static string RemoveVocalTags(string line)
        {
            return Regex.Replace(line, @"^\s*V\d+:\s*", "", RegexOptions.IgnoreCase);
        }
        public static ParsedLyrics Parse(string rawLrc)
        {
            var result = new ParsedLyrics();
            if (string.IsNullOrWhiteSpace(rawLrc))
            {
                return result;
            }


            string[] lines = rawLrc.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);


            var plainLines = new List<string>();

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
                    plainLines.Add(RemoveVocalTags(trimmed));
                    continue;
                }

                // removing v1 v2 tags befor hand
                try
                {
                    string text = RemoveVocalTags(lineMatch.Groups["text"].Value.Trim());

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
                catch (Exception ex)
                {
                    Logger.Log("Error while Removin prefix tags: " + ex.Message);
                }

            }

            // no timestamps at all: plain, unsynchronized lyrics (file order is kept as-is)
            if (result.Lines.Count == 0)
            {
                result.IsSynchronized = false;
                foreach (string text in plainLines)
                {
                    result.Lines.Add(new LyricLine { Text = text });
                }
                return result;
            }

            // OrderBy is stable, so lines sharing a timestamp keep their file order
            result.Lines = result.Lines.OrderBy(l => l.StartTime).ToList();

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
            // "mm:ss", "mm:ss.xx", "mm:ss.xxx" or "mm:ss:xx"
            string[] parts = timestamp.Split(':');
            int minutes = int.Parse(parts[0], CultureInfo.InvariantCulture);
            string secondsText = parts[1];
            if (parts.Length > 2)
            {
                secondsText += "." + parts[2];
            }
            else
            {
                secondsText = secondsText.Replace(',', '.');
            }

            double seconds = double.Parse(secondsText, CultureInfo.InvariantCulture);
            return TimeSpan.FromMinutes(minutes) + TimeSpan.FromSeconds(seconds);
        }
    }
}
