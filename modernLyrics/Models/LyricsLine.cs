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


        public LyricsLine(List<LyricWord> words, TimeSpan start, TimeSpan end)
        {
            Words = words;
            Start = start;
            End = end;
        }
    }
}
