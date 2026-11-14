using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public class LyricsViewControl : UserControl
    {
        private ParsedLyrics _lyrics = new ParsedLyrics();
        private int _currentIndex = -1;

        private const int HorizontalPadding = 20;

        private readonly Font _activeFont = new Font("Segoe UI", 16F, FontStyle.Bold);
        private readonly Font _inactiveFont = new Font("Segoe UI", 14F, FontStyle.Regular);
        private readonly SolidBrush _activeBrush = new SolidBrush(Color.White);
        private readonly SolidBrush _inactiveBrush = new SolidBrush(Color.Gray);
        private readonly StringFormat _format = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter,
            FormatFlags = StringFormatFlags.NoWrap
        };

        public LyricsViewControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.FromArgb(20, 20, 20);
        }

        public void SetLyrics(ParsedLyrics lyrics)
        {
            _lyrics = lyrics ?? new ParsedLyrics();
            _currentIndex = -1;
            Invalidate();
        }

        public void UpdatePosition(TimeSpan position)
        {
            int newIndex = FindLineIndex(position);
            if (newIndex != _currentIndex)
            {
                _currentIndex = newIndex;
                Invalidate();
            }
        }

        // index of the last line whose StartTime <= position, or -1 before the first line
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
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(BackColor);

            if (_lyrics.Lines.Count == 0)
            {
                g.DrawString("No Lyrics Available", _inactiveFont, _inactiveBrush, ClientRectangle, _format);
                return;
            }

            int lineHeight = (int)(_activeFont.GetHeight(g) * 2f);
            int activeY = (ClientSize.Height - lineHeight) / 2;
            int width = Math.Max(1, ClientSize.Width - HorizontalPadding * 2);

            for (int i = 0; i < _lyrics.Lines.Count; i++)
            {
                int y = activeY + (i - _currentIndex) * lineHeight;

                if (y < -lineHeight || y > ClientSize.Height)
                {
                    continue;
                }

                string text = _lyrics.Lines[i].Text;
                if (text.Length == 0)
                {
                    continue;
                }

                bool isActive = i == _currentIndex;
                var rect = new Rectangle(HorizontalPadding, y, width, lineHeight);

                g.DrawString(text,
                    isActive ? _activeFont : _inactiveFont,
                    isActive ? _activeBrush : _inactiveBrush,
                    rect, _format);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _activeFont.Dispose();
                _inactiveFont.Dispose();
                _activeBrush.Dispose();
                _inactiveBrush.Dispose();
                _format.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}