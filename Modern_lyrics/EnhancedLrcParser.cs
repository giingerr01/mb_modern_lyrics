using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MusicBeePlugin
{
    public static class EnhancedLrcParser
    {
        private static readonly Regex HeaderTagRegex = new Regex(@"^\[(?<key>[a-zA-Z]+):(?<value>.*)\]$", RegexOptions.Compiled);
        private static readonly Regex LineTimestampRegex = new Regex(@"^\[(?<timestamp>\d{1,2}:\d{2}(?:\.\d{1,3})?)\](?<text>.*)$", RegexOptions.Compiled);

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
                    continue;
                }
            }

            return result;
        }
    }
}