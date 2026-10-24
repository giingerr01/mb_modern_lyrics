using System;
using System.Collections.Generic;

namespace MusicBeePlugin.Models
{
    public class LyricsLine
    {
        public List<LyricWord> Words { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public bool IsBackground { get; set; }

        // Speaker/voice tag from the source ("v1", "v2", ...), or null if the line had none.
        public string Voice { get; set; }

        public LyricsLine(List<LyricWord> words, TimeSpan start, TimeSpan end)
        {
            Words = words;
            Start = start;
            End = end;
        }
    }
}