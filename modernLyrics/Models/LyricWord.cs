using System;

namespace MusicBeePlugin.Models
{
    public class LyricWord
    {
        public string Text { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }

        // True when the source had whitespace after this word. Lets the renderer join
        // syllables ("be" + "lieve") without a gap and still put spaces between whole words.
        public bool TrailingSpace { get; set; } = true;

        public TimeSpan Duration => End - Start;

        public LyricWord(string text, TimeSpan start, TimeSpan end)
        {
            Text = text;
            Start = start;
            End = end;
        }
    }
}