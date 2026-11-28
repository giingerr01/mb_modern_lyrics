using System;
using System.Collections.Generic;

namespace MusicBeePlugin
{
    public class LyricWord
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan Text { get; set; }

    }

    public class WordByWordParsedLyrics
    {
        public Dictionary<string, string> Metadata { get; set; } = new
        Dictionary<string, string>();
        public List<LyricWord> Words { get; set; } = new List<LyricWord>();
        public TimeSpan Offset { get; set; } = TimeSpan.Zero;
        public bool IsSynchronized { get; set; } = true;
    }
}