using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public class LyricsViewControl : UserControl
    {
        private const int HorizontalPadding = 24;
        private const int LineGap = 1;
        private const float WheelStep = 30f;
        private int _currentIndex = -1;

        // layout
        private float[] _tops = new float[0];
        private float[] _heights = new float[0];
        private float _totalHeight;
        private bool _layoutDirty = true;

        // scrolling
        private float _scrollY;
        private float _targetY;
        private readonly Timer _animTimer;

        private Font _activeFont;
        private Font _inactiveFont;
        private int _fontSize = 10;
        private string _font = "Lucida Sans";
        private ParsedLyrics _lyrics = new ParsedLyrics();

        private Color _activeColor = Color.Black;
        private Color _inactiveColor = Color.WhiteSmoke;

        private SolidBrush _activeBrush;
        private SolidBrush _inactiveBrush;

        // wrapped, horizontally centered lines
        private readonly StringFormat _lineFormat = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Near,
            Trimming = StringTrimming.Word
        };

        // single centered message
        private readonly StringFormat _messageFormat = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        private void IntializeFonts()
        {
            _activeFont = new Font(_font, _fontSize, FontStyle.Bold);
            _inactiveFont = new Font(_font, Math.Max(9, _fontSize - 1), FontStyle.Bold);
        }

        public void SetActiveColor(Color color)
        {
            _activeColor = color;
            _activeBrush.Dispose();
            _activeBrush = new SolidBrush(_activeColor);

            Invalidate();

        }

        public LyricsViewControl()
        {
            IntializeFonts();
            _activeBrush = new SolidBrush(_activeColor);
            _inactiveBrush = new SolidBrush(_inactiveColor);

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();
            BackColor = Color.FromArgb(255, 255, 255);

            _animTimer = new Timer { Interval = 15 };
            _animTimer.Tick += AnimTimer_Tick;
        }

        public void SetLyrics(ParsedLyrics lyrics)
        {
            _lyrics = lyrics ?? new ParsedLyrics();
            _currentIndex = -1;
            _scrollY = 0;
            _targetY = 0;
            _layoutDirty = true;
            _animTimer.Stop();
            Invalidate();
        }

        public void SetFontSize(int fontSize, String font)
        {
            if (fontSize < 8 || fontSize > 72)
            {
                throw new ArgumentOutOfRangeException(nameof(fontSize));
            }

            if (_font == font && _fontSize == fontSize)
            {
                return;
            }

            _font = font;
            _fontSize = fontSize;

            _activeFont.Dispose();
            _inactiveFont.Dispose();

            IntializeFonts();

            _layoutDirty = true;
            _animTimer.Stop();
            Invalidate();
        }

        public void UpdatePosition(TimeSpan position)
        {
            if (!_lyrics.IsSynchronized || _lyrics.Lines.Count == 0)
            {
                return;
            }

            int newIndex = FindLineIndex(position);
            if (newIndex == _currentIndex)
            {
                return;
            }

            _currentIndex = newIndex;

            if (_layoutDirty)
            {
                Invalidate();   // OnPaint lays out and snaps to the right place
                return;
            }

            _targetY = ComputeTarget();

            // big jump (seeking): don't scroll through the whole song
            if (Math.Abs(_targetY - _scrollY) > ClientSize.Height * 2f)
            {
                _scrollY = _targetY;
            }

            _animTimer.Start();
            Invalidate();
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

        private void EnsureLayout(Graphics g)
        {
            int width = Math.Max(1, ClientSize.Width - HorizontalPadding * 2);
            int count = _lyrics.Lines.Count;

            _tops = new float[count];
            _heights = new float[count];

            float y = 0;
            for (int i = 0; i < count; i++)
            {
                string text = _lyrics.Lines[i].Text;

                // measured with the bold font so the active line never needs more room than reserved
                float h = text.Length == 0
                    ? _activeFont.GetHeight(g) * 0.5f
                    : g.MeasureString(text, _activeFont, width, _lineFormat).Height;

                _tops[i] = y;
                _heights[i] = h;
                y += h + LineGap;
            }

            _totalHeight = Math.Max(0, y - LineGap);
            _layoutDirty = false;
        }

        private float MaxScroll()
        {
            return Math.Max(0, _totalHeight - ClientSize.Height);
        }

        private float ClampScroll(float value)
        {
            // -10 is to the gap size between the firs line of lyircs from the celieng of the LRC panle
            return Math.Max(-10, Math.Min(MaxScroll(), value));
        }

        // synced: keeping the active line ahh...vericallly in center and unsynced lyrics keep the current clapmped target
        private float ComputeTarget()
        {
            if (!_lyrics.IsSynchronized || _tops.Length == 0)
            {
                return ClampScroll(_targetY);
            }

            int i = Math.Max(_currentIndex, 0);
            /// clamp the top line so the top part doesnt show show blacnk Panle 
            return ClampScroll(_tops[i] + _heights[i] / 2f - ClientSize.Height / 2f);
        }

        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            float diff = _targetY - _scrollY;
            if (Math.Abs(diff) < 0.5f)
            {
                _scrollY = _targetY;
                _animTimer.Stop();
            }
            else
            {
                _scrollY += diff * 0.18f;
            }
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (!_lyrics.IsSynchronized)
            {
                Focus();   // so the mouse wheel reaches us
            }
        }

        // unsynced lyrics can be scrolled manually
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (_lyrics.IsSynchronized || _layoutDirty)
            {
                return;
            }

            _targetY = ClampScroll(_targetY - e.Delta / 120f * WheelStep);
            _animTimer.Start();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _layoutDirty = true;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit; // avoid Antialias bcz it can render text not very clean sharp
            // use gridfit so it can render text with sharp clarity it does comes with a tradeoff in perfomace of smoothness of the lyrics scrolling
            g.Clear(BackColor);

            if (_lyrics.Lines.Count == 0)
            {
                g.DrawString("No Lyrics Available", _inactiveFont, _inactiveBrush, ClientRectangle, _messageFormat);
                return;
            }

            if (_layoutDirty)
            {
                EnsureLayout(g);
                _targetY = ComputeTarget();
                _scrollY = _targetY;   // snap after new lyrics / resize
            }

            int width = Math.Max(1, ClientSize.Width - HorizontalPadding * 2);
            bool synced = _lyrics.IsSynchronized;

            for (int i = 0; i < _lyrics.Lines.Count; i++)
            {
                float y = _tops[i] - _scrollY;

                if (y + _heights[i] < 0)
                {
                    continue;
                }
                if (y > ClientSize.Height)
                {
                    break;
                }

                string text = _lyrics.Lines[i].Text;
                if (text.Length == 0)
                {
                    continue;
                }

                bool isActive = synced && i == _currentIndex;
                var rect = new RectangleF(HorizontalPadding, y, width, _heights[i] + LineGap);

                g.DrawString(text,
                    isActive ? _activeFont : _inactiveFont,
                    isActive ? _activeBrush : _inactiveBrush,
                    rect, _lineFormat);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _animTimer.Stop();
                _animTimer.Dispose();
                _activeFont.Dispose();
                _inactiveFont.Dispose();
                _activeBrush.Dispose();
                _inactiveBrush.Dispose();
                _lineFormat.Dispose();
                _messageFormat.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
