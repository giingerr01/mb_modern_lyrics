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
        private bool _sweepAnimationEnabled = true;
        private string _font = "Comic Sans";
        private int _fontSize = 12;
        private FontStyle _fontStyle = FontStyle.Bold;
        private Color _activeColor = Color.DodgerBlue;
        private Color _inactiveColor = Color.DimGray;

        private Font _activeFont = new Font("Comic Sans", 12, FontStyle.Bold);
        private Font _inactiveFont = new Font("Comic Sans", 11, FontStyle.Regular);

        private SolidBrush _activeBrush = new SolidBrush(Color.DodgerBlue);
        private SolidBrush _inactiveBrush = new SolidBrush(Color.DimGray);
        private static readonly float[] DimAmounts = { 0.70f, 0.65f, 0.50f, 0.35f, 0.20f, 0.15f, 0.10f };
        private readonly SolidBrush[] _dimBrushes = new SolidBrush[DimAmounts.Length];

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
            RebuildDimBrushes();
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

        public void SetSweepAnimationEnabled(bool enabled)
        {
            if (_sweepAnimationEnabled == enabled)
            {
                return;
            }

            _sweepAnimationEnabled = enabled;
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
            else if (!_sweepAnimationEnabled)
            {
                return;
            }
            // Repaint during playback so the sweep advances smoothly.
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
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
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

                int distance = Math.Abs(i - _currentIndex);
                int brushIndex = Math.Max(0, Math.Min(distance - 1, _dimBrushes.Length - 1));
                bool isActive = _lyrics.IsSynchronized && i == _currentIndex;
                if (isActive)
                {
                    if (_sweepAnimationEnabled)
                    {
                        DrawSweepingLine(g, text, rect, i);
                    }
                    else
                    {
                        g.DrawString(text, _activeFont, _activeBrush, rect, _lineFormat);
                    }
                }
                else if (_lyrics.IsSynchronized && _currentIndex >= 0)
                {
                    g.DrawString(text, _inactiveFont, _dimBrushes[brushIndex], rect, _lineFormat);
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

            g.DrawString(text, _activeFont, _inactiveBrush, rect, _lineFormat);
            if (progress > 0f)
            {
                //saving the state and apply the and restore
                RectangleF sweepRect = new RectangleF(
                    rect.X,
                    rect.Y,
                    rect.Width * progress,
                    rect.Height);
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
                float targetScrollY = _tops[_currentIndex] + _heights[_currentIndex] / 2f -
                    ClientSize.Height / 2f;
                float maxScrollY = Math.Max(0f, y - LineGap - ClientSize.Height);
                _scrollY = Math.Max(0f, Math.Min(targetScrollY, maxScrollY));
            }
        }

        private void RebuildDimBrushes()
        {
            for (int i = 0; i < DimAmounts.Length; i++)
            {
                _dimBrushes[i]?.Dispose();
                _dimBrushes[i] = new SolidBrush(BlendColors(_inactiveColor, BackColor, DimAmounts[i]));
            }
        }

        private static Color BlendColors(Color foreground, Color background, float visibility)
        {
            visibility = Math.Max(0f, Math.Min(1f, visibility));

            int r = (int)(background.R + (foreground.R - background.R) * visibility);
            int g = (int)(background.G + (foreground.G - background.G) * visibility);
            int b = (int)(background.B + (foreground.B - background.B) * visibility);

            return Color.FromArgb(r, g, b);
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            if (_inactiveBrush != null)
            {
                RebuildDimBrushes();
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
                foreach (SolidBrush brush in _dimBrushes)
                {
                    brush?.Dispose();
                }
            }
            base.Dispose(disposing);
        }

    }
}