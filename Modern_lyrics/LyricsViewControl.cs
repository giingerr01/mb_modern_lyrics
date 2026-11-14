using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public class LyricsViewControl : UserControl
    {
        private ParsedLyrics _parsedLyrics;
        private TimeSpan _currentPosition = TimeSpan.Zero;

        // Styling configuration
        private Font _lineFont = new Font("Segoe UI", 16F, FontStyle.Bold);
        private Font _inactiveFont = new Font("Segoe UI", 14F, FontStyle.Regular);
        private Brush _activeBrush = Brushes.White;
        private Brush _wordHighlightBrush = Brushes.Cyan;
        private Brush _inactiveBrush = Brushes.Gray;
        private Brush _backgroundBrush = new SolidBrush(Color.FromArgb(20, 20, 20));

        private int _currentIndex = -1;

        public LyricsViewControl()
        {
            // Enable double buffering for smooth rendering without flicker
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.ResizeRedraw, true);
            this.BackColor = Color.FromArgb(20, 20, 20);
        }

        public void SetLyrics(ParsedLyrics lyrics)
        {
            _parsedLyrics = lyrics;
            _currentIndex = -1;
            this.Invalidate();
        }

        public void UpdatePosition(TimeSpan position)
        {
            _currentPosition = position;

            if (_parsedLyrics == null || _parsedLyrics.Lines.Count == 0)
            {
                return;
            }

            // Find current active line index based on position
            int newIndex = -1;
            for (int i = 0; i < _parsedLyrics.Lines.Count; i++)
            {
                var line = _parsedLyrics.Lines[i];
                if (_currentPosition >= line.StartTime && _currentPosition < line.EndTime)
                {
                    newIndex = i;
                    break;
                }
                // Fallback if between lines
                if (_currentPosition < line.StartTime)
                {
                    newIndex = i > 0 ? i - 1 : 0;
                    break;
                }
            }

            if (newIndex == -1 && _currentPosition >= _parsedLyrics.Lines[_parsedLyrics.Lines.Count - 1].StartTime)
            {
                newIndex = _parsedLyrics.Lines.Count - 1;
            }

            if (newIndex != _currentIndex)
            {
                _currentIndex = newIndex;
                this.Invalidate(); // Redraw when active line changes
            }
            else
            {
                // Also redraw if within the active line to update word-level karaoke progress
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            g.FillRectangle(_backgroundBrush, this.ClientRectangle);

            if (_parsedLyrics == null || _parsedLyrics.Lines.Count == 0)
            {
                using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    g.DrawString("No Lyrics Available", _inactiveFont, _inactiveBrush, this.ClientRectangle, format);
                }
                return;
            }

            // Calculate vertical centering around the active line
            int centerY = this.ClientSize.Height / 2;
            int lineHeight = 45;

            if (_currentIndex >= 0 && _currentIndex < _parsedLyrics.Lines.Count)
            {
                // Center the active line
                int activeY = centerY - (lineHeight / 2);

                // Draw active and surrounding lines
                for (int i = 0; i < _parsedLyrics.Lines.Count; i++)
                {
                    int distance = i - _currentIndex;
                    int lineY = activeY + (distance * lineHeight);

                    // Skip lines far off-screen to optimize rendering
                    if (lineY < -lineHeight || lineY > this.ClientSize.Height + lineHeight)
                    {
                        continue;
                    }

                    var line = _parsedLyrics.Lines[i];
                    bool isActive = (i == _currentIndex);

                    DrawLine(g, line, lineY, isActive);
                }
            }
        }

        private void DrawLine(Graphics g, LyricLine line, int y, bool isActive)
        {
            float startX = 40; // Left padding
            float currentX = startX;

            if (line.Words == null || line.Words.Count == 0)
            {
                var font = isActive ? _lineFont : _inactiveFont;
                var brush = isActive ? _activeBrush : _inactiveBrush;
                g.DrawString(line.FullText, font, brush, startX, y);
                return;
            }

            foreach (var word in line.Words)
            {
                // Determine if this specific word is currently being spoken/sung
                bool isWordActive = isActive && _currentPosition >= word.StartTime && _currentPosition < word.EndTime;
                bool isWordPassed = isActive && _currentPosition >= word.EndTime;

                Brush wordBrush = _inactiveBrush;
                if (isActive)
                {
                    if (isWordActive || isWordPassed)
                    {
                        wordBrush = _wordHighlightBrush;
                    }
                    else
                    {
                        wordBrush = _activeBrush;
                    }
                }

                var font = isActive ? _lineFont : _inactiveFont;
                g.DrawString(word.Text, font, wordBrush, currentX, y);

                // Measure string width to offset the next word
                SizeF size = g.MeasureString(word.Text, font);
                currentX += size.Width;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _lineFont?.Dispose();
                _inactiveFont?.Dispose();
                _backgroundBrush?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}