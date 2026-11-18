using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Numerics;
using System.Windows.Forms;

namespace MusicBeePlugin.Enhanced_lyrics
{
    public class SweepLyricsViewControl : UserControl
    {
        private const int HorizontalPadding = 24;
        private const int LineGap = 1;

        private ParsedLyrics _lyrics = new ParsedLyrics();
        private int _currentIndex = -1;

        private TimeSpan _currentPosition;

        private float[] _tops = new float[0];
        private float[] _heights = new float[0];
        private bool _layoutDirty = true;
        private float _scrollY;

        private Font _activeFont = new Font("Comic Sans", 12, FontStyle.Bold);
        private Font _inactive = new Font("Comic Sans", 11, FontStyle.Regular);

        private SolidBrush _activeBrush = new SolidBrush(Color.DodgerBlue);
        private SolidBrush _inactiveBrush = new SolidBrush(Color.DimGray);

        private StringFormat _lineFormat = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
            Trimming = StringTrimming.Word
        };

        public SweepLyricsViewControl()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();
            BackColor = Color.FromArgb(30, 30, 30);
        }

        public void SetLyrics(ParsedLyrics lyrics)
        {
            _lyrics = lyrics ?? new ParsedLyrics();
            _currentIndex = -1;
            _layoutDirty = true;
            Invalidate();
        }

        public void UpdatePosition(TimeSpan position)
        {
            _currentIndex = position;

            if (!_lyrics.IsSynchronized || _lyrics.Lines.Count == 0)
            {
                return;
            }

            int newIndex = FindLineIndex(position);

            if (newIndex != _currentIndex)
            {
                _currentIndex = newIndex;
                _layoutDirty = true; //recalculate fo rlayotu scroll
            }
            //Note i have to invalidaat every frame to make sweepanimation
            Invalidate();

        }

        private int FindLineIndex(TimeSpan position)
        {
            var lines = _lyrics.Lines;
            int lo = 0, hi = lines.Count - 1, result = -1;

            while (lo <= hi)
            {
                int mid = (lo + hi) / 2;
                if (lines[mid].StartTime <= position)
                {
                    result = mid;
                    lo = mid + 1;
                }
                else
                {
                    hi = mid - 1;
                }
                return result;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            g.Clear(BackColor);

            if (_lyrics.Lines.Count == 0)
            {
                EnsureLayout(g);
                _layoutDirty = false;

                int width = Math.Max(1, ClientSize.Width - HorizontalPadding * 2);

                for (int i = 0; i < _lyrics.Lines.Count; i++)
                {
                    float y = _tops[i] - _scrollY;
                    var rect = new RectangleF(HorizontalPadding, y, width, _heights[i] + LineGap);

                    if (y + _heights[i] < 0 || y > ClientSize.Height)
                    {
                        continue;
                    }

                    string text = _lyrics.Lines[i].Text;
                    if (string.IsNullOrEmpty(text))
                    {
                        continue;
                    }

                    bool isActive = (i == _currentIndex);
                    if (isActive)
                    {
                        DrawSweepingLine(g, text, rect, i);
                    }
                    else
                    {
                        g.DrawString(text, _inactive, _inactiveBrush, rect, _lineFormat);
                    }
                }
            }
        }
        private void DrawSweepingLine(Graphics g, string text, RectangleF rect, int lineIndex)
        {
            var line = _lyrics.Lines[lineIndex];

            //finally here calculaitng progress (0 to 1)
            double totalMs = (line.EndTime - line.StartTime).TotalMilliseconds;
            if (totalMs <= 0)
            {
                totalMs = 1;
            }

            double elapsedMs = (_currentPosition - line.StartTime)


        }




    }
}