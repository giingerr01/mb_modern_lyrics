using System;
using System.Collections.Generic;
using MusicBeePlugin.Parsing;

namespace MusicBeePlugin.Models
{
    public class LyricsDocument
    {
        public static readonly LyricsDocument Empty = new LyricsDocument(new List<LyricsLine>());

        public List<LyricsLine> Lines { get; private set; }

        public bool IsEmpty => Lines.Count == 0;

        public LyricsDocument(List<LyricsLine> lines)
        {
            Lines = lines ?? new List<LyricsLine>();
        }

        public static LyricsDocument FromText(string rawLyrics, TimeSpan? trackDuration)
        {
            if (string.IsNullOrWhiteSpace(rawLyrics))
            {
                return Empty;
            }

            List<LyricsLine> lines = EnhancedLyricsParser.Parse(rawLyrics, trackDuration);
            return lines.Count == 0 ? Empty : new LyricsDocument(lines);
        }

        // Index of the last line whose Start is <= position, or -1 if position is before the first line.
        // Lines are sorted by Start, so this is a binary search.
        public int FindLineIndex(TimeSpan position)
        {
            int low = 0;
            int high = Lines.Count - 1;
            int result = -1;

            while (low <= high)
            {
                int mid = low + (high - low) / 2;
                if (Lines[mid].Start <= position)
                {
                    result = mid;
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }
            }

            return result;
        }
    }
}