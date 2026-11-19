using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Net.Configuration;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Forms;
//SweeplyricsViewControl.cs

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
        private string _font = "Comic Sans";
        private int _fontSize = 12;
        private FontStyle _fontStyle = FontStyle.Bold;
        private Color _activeColor = Color.DodgerBlue;
        private Color _inactiveColor = Color.DimGray;

        private Font _activeFont = new Font("Comic Sans", 12, FontStyle.Bold);
        private Font _inactiveFont = new Font("Comic Sans", 11, FontStyle.Regular);

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

        public void SetActiveColor(Color color)
        {
            _activeColor = color;
            _activeBrush.Dispose();
            _activeBrush = new SolidBrush(_activeColor);
            Invalidate();
        }

        public void SetInActiveColor(Color color)
        {
            _inactiveColor = color;
            _inactiveBrush.Dispose();
            _inactiveBrush = new SolidBrush(_inactiveColor);
            Invalidate();
        }

        public void SetFontSize(int fontSize, string font, FontStyle fontStyle)
        {
            int clampedSize = Math.Max(8, Math.Min(72, fontSize));
            string fontName = font ?? _font;
            if (_font == fontName && _fontSize == clampedSize && _fontStyle == fontStyle)
            {
                return;
            }

            var activeFont = new Font(fontName, clampedSize, fontStyle);
            var inactiveFont = new Font(fontName, Math.Max(9, clampedSize - 1), fontStyle);
            _activeFont.Dispose();
            _inactiveFont.Dispose();
            _activeFont = activeFont;
            _inactiveFont = inactiveFont;
            _font = fontName;
            _fontSize = clampedSize;
            _fontStyle = fontStyle;
            _layoutDirty = true;
            Invalidate();
        }

        public void SetLyricsAlignment(StringAlignment alignment)
        {
            if (alignment != StringAlignment.Near &&
                alignment != StringAlignment.Center &&
                alignment != StringAlignment.Far)
            {
                throw new ArgumentOutOfRangeException(nameof(alignment));
            }

            _lineFormat.Alignment = alignment;
            Invalidate();
        }

        public void SetLyrics(ParsedLyrics lyrics)
        {
            _lyrics = lyrics ?? new ParsedLyrics();
            _currentIndex = -1;
            _scrollY = 0;
            _layoutDirty = true;
            Invalidate();
        }

        public void UpdatePosition(TimeSpan position)
        {
            _currentPosition = position;

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

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _layoutDirty = true;
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
            }

            return result;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            g.Clear(BackColor);

            if (_layoutDirty)
            {
                EnsureLayout(g);
                _layoutDirty = false;
            }

            if (_lyrics.Lines.Count == 0)
            {
                g.DrawString("No Lyrics Available", _activeFont, _activeBrush, ClientRectangle, _lineFormat);
                return;
            }

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

                bool isActive = _lyrics.IsSynchronized && i == _currentIndex;
                if (isActive)
                {
                    DrawSweepingLine(g, text, rect, i);
                }
                else
                {
                    g.DrawString(text, _inactiveFont, _inactiveBrush, rect, _lineFormat);
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

            double elapsedMs = (_currentPosition - line.StartTime).TotalMilliseconds;

            float progress = (float)(elapsedMs / totalMs);

            //clamping between 0 to 1
            progress = Math.Max(0f, Math.Min(1f, progress));

            g.DrawString(text, _inactiveFont, _inactiveBrush, rect, _lineFormat);
            if (progress > 0f)
            {
                //mesure the size of the text to decide hwo wide is the seeep should be

                SizeF textSize = g.MeasureString(text, _activeFont, 10000, StringFormat.GenericTypographic);

                float widthOfSweep = textSize.Width * progress;

                float startX = rect.X + (rect.Width - textSize.Width) / 2f;

                if (startX < rect.X)
                {
                    startX = rect.X;
                }

                //width of the sweep mask
                RectangleF sweepRect = new RectangleF(startX, rect.Y, widthOfSweep, rect.Height);

                //here saving the state and apply and restroe
                GraphicsState state = g.Save();
                g.SetClip(sweepRect, CombineMode.Intersect);
                g.DrawString(text, _activeFont, _activeBrush, rect, _lineFormat);
                g.Restore(state);
            }
        }

        private void EnsureLayout(Graphics g)
        {
            int width = Math.Max(1, ClientSize.Width - HorizontalPadding * 2);
            int count = _lyrics.Lines.Count;
            _tops = new float[count];
            _heights = new float[count];

            float y = 0;
            for (int i = 0; i < count; i++)
            {
                float h = g.MeasureString(_lyrics.Lines[i].Text, _activeFont, width, _lineFormat).Height;
                _tops[i] = y;
                _heights[i] = h;
                y += h + LineGap;
            }

            if (_currentIndex >= 0 && _currentIndex < count)
            {
                _scrollY = _tops[_currentIndex] + _heights[_currentIndex] / 2f - ClientSize.Height / 2f;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _activeFont?.Dispose();
                _activeBrush?.Dispose();
                _inactiveBrush?.Dispose();
                _inactiveFont?.Dispose();
                _lineFormat?.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}