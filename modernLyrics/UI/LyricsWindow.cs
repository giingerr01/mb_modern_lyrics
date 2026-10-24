using System;
using System.Drawing;
using System.Windows.Forms;
using MusicBeePlugin.Models;
using MusicBeePlugin.Playback;

namespace MusicBeePlugin.UI
{
    public sealed class LyricsWindow : Control
    {
        private readonly LyricsSynchronizer synchronizer = new LyricsSynchronizer();
        private readonly Timer playbackTimer;
        private Func<TimeSpan> playbackPositionProvider;
        private TimeSpan playbackPosition;
        private string emptyMessage = "Waiting for lyrics...";

        public LyricsWindow()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint, true);
            BackColor = SystemColors.Control;
            ForeColor = SystemColors.ControlText;
            Font = SystemFonts.MessageBoxFont;

            playbackTimer = new Timer();
            playbackTimer.Interval = 60;
            playbackTimer.Tick += PlaybackTimer_Tick;
            playbackTimer.Start();
        }

        public void SetPlaybackPositionProvider(Func<TimeSpan> positionProvider)
        {
            playbackPositionProvider = positionProvider;
        }

        public void SetLyrics(string rawLyrics, TimeSpan? duration)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string, TimeSpan?>(SetLyrics), rawLyrics, duration);
                return;
            }

            LyricsDocument document = LyricsDocument.FromText(rawLyrics, duration);
            synchronizer.SetDocument(document);
            emptyMessage = string.IsNullOrWhiteSpace(rawLyrics)
                ? "No lyrics available for this track."
                : "No synced lyrics found.";
            Invalidate();
        }

        public void UpdatePlaybackPosition(TimeSpan position)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<TimeSpan>(UpdatePlaybackPosition), position);
                return;
            }

            playbackPosition = position;
            synchronizer.Update(position);
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                playbackTimer.Stop();
                playbackTimer.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(BackColor);

            LyricsDocument document = synchronizer.Document;
            if (document.IsEmpty)
            {
                DrawCenteredText(e.Graphics, emptyMessage, Font, ForeColor, ClientRectangle);
                return;
            }

            SyncState state = synchronizer.Update(playbackPosition);
            int lineIndex = state.LineIndex;
            if (lineIndex < 0)
            {
                DrawCenteredText(e.Graphics, "Lyrics will appear here during playback.", Font, ForeColor, ClientRectangle);
                return;
            }

            int lineHeight = Font.Height + 8;
            int currentCenter = ClientSize.Height / 2;

            if (lineIndex > 0)
            {
                DrawLine(e.Graphics, document.Lines[lineIndex - 1], Font, Color.Gray,
                    currentCenter - lineHeight, false, -1, 0);
            }

            DrawLine(e.Graphics, document.Lines[lineIndex], Font, ForeColor,
                currentCenter, true, state.WordIndex, state.WordProgress);

            if (lineIndex + 1 < document.Lines.Count)
            {
                DrawLine(e.Graphics, document.Lines[lineIndex + 1], Font, Color.Gray,
                    currentCenter + lineHeight, false, -1, 0);
            }
        }

        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            if (playbackPositionProvider != null)
            {
                UpdatePlaybackPosition(playbackPositionProvider());
            }
        }

        private static void DrawCenteredText(Graphics graphics, string text, Font font, Color color, Rectangle bounds)
        {
            TextRenderer.DrawText(graphics, text, font, bounds, color,
                TextFormatFlags.HorizontalCenter |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis |
                TextFormatFlags.NoPadding);
        }

        private void DrawLine(
            Graphics graphics,
            LyricsLine line,
            Font font,
            Color color,
            int centerY,
            bool active,
            int activeWordIndex,
            double wordProgress)
        {
            Font drawFont = font;
            bool disposeFont = false;
            if (active)
            {
                drawFont = new Font(font, FontStyle.Bold);
                disposeFont = true;
            }

            try
            {
                string[] segments = new string[line.Words.Count];
                int totalWidth = 0;
                for (int wordIndex = 0; wordIndex < line.Words.Count; wordIndex++)
                {
                    LyricWord word = line.Words[wordIndex];
                    segments[wordIndex] = word.Text + (word.TrailingSpace ? " " : string.Empty);
                    totalWidth += TextRenderer.MeasureText(graphics, segments[wordIndex], drawFont,
                        Size.Empty, TextFormatFlags.NoPadding).Width;
                }

                int x = Math.Max(0, (ClientSize.Width - totalWidth) / 2);
                int y = centerY - drawFont.Height / 2;
                for (int wordIndex = 0; wordIndex < line.Words.Count; wordIndex++)
                {
                    int width = TextRenderer.MeasureText(graphics, segments[wordIndex], drawFont,
                        Size.Empty, TextFormatFlags.NoPadding).Width;
                    Rectangle wordBounds = new Rectangle(x, y, width, drawFont.Height + 2);

                    if (active && wordIndex == activeWordIndex)
                    {
                        TextRenderer.DrawText(graphics, segments[wordIndex], drawFont, wordBounds,
                            Color.Gray, TextFormatFlags.NoPadding);
                        wordBounds.Width = (int)(wordBounds.Width * Math.Max(0, Math.Min(1, wordProgress)));
                        TextRenderer.DrawText(graphics, segments[wordIndex], drawFont, wordBounds,
                            color, TextFormatFlags.NoPadding);
                    }
                    else
                    {
                        bool completed = active && wordIndex < activeWordIndex;
                        TextRenderer.DrawText(graphics, segments[wordIndex], drawFont, wordBounds,
                            completed ? color : (active ? Color.Gray : color),
                            TextFormatFlags.NoPadding);
                    }

                    x += width;
                }
            }
            finally
            {
                if (disposeFont)
                {
                    drawFont.Dispose();
                }
            }
        }
    }
}