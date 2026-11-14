using System;
using System.Collections.Generic;

namespace MusicBeePlugin
{
    public class LyricLine
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Text { get; set; } = string.Empty;
    }

    public class ParsedLyrics
    {
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
        public List<LyricLine> Lines { get; set; } = new List<LyricLine>();
        public TimeSpan Offset { get; set; } = TimeSpan.Zero;
        public bool IsSynchronized { get; set; } = true;
    }
}