using System;

namespace MusicBeePlugin.Models
{
    public class LyricWord
    {
        public string Text { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }

        public TimeSpan Duration => End - Start;

        public LyricWord(string text, TimeSpan start, TimeSpan end)
        {
            Text = text;
            Start = start;
            End = end;
        }
    }
}