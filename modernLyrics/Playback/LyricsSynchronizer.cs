using System;
using MusicBeePlugin.Models;

namespace MusicBeePlugin.Playback
{
    // Snapshot of "where we are" in the lyrics at one moment. The UI draws from this:
    //   words before WordIndex  -> fully lit
    //   word at WordIndex       -> lit up to WordProgress (0..1)
    //   words after WordIndex   -> dim
    public struct SyncState
    {
        public readonly int LineIndex;       // -1 before the first line
        public readonly int WordIndex;       // -1 if no word has started yet
        public readonly double WordProgress; // 0..1
        public readonly bool InGap;          // true after the line's last word ended, before the next line starts

        public SyncState(int lineIndex, int wordIndex, double wordProgress, bool inGap)
        {
            LineIndex = lineIndex;
            WordIndex = wordIndex;
            WordProgress = wordProgress;
            InGap = inGap;
        }
    }

    public class LyricsSynchronizer
    {
        private LyricsDocument document = LyricsDocument.Empty;
        private int lastLineIndex = -1;

        // Raised only when the active line changes (including seeks and track changes),
        // so the UI can start its scroll animation / re-layout without doing it every tick.
        public event Action<int> ActiveLineChanged;

        public LyricsDocument Document
        {
            get { return document; }
        }

        public void SetDocument(LyricsDocument newDocument)
        {
            document = newDocument ?? LyricsDocument.Empty;
            lastLineIndex = -2; // force the next Update to raise ActiveLineChanged
        }

        public SyncState Update(TimeSpan position)
        {
            int lineIndex = FindActiveLine(position);

            if (lineIndex != lastLineIndex)
            {
                lastLineIndex = lineIndex;
                Action<int> handler = ActiveLineChanged;
                if (handler != null)
                {
                    handler(lineIndex);
                }
            }

            if (lineIndex < 0)
            {
                return new SyncState(-1, -1, 0, false);
            }

            LyricsLine line = document.Lines[lineIndex];

            int wordIndex = -1;
            for (int i = 0; i < line.Words.Count; i++)
            {
                if (line.Words[i].Start <= position)
                {
                    wordIndex = i;
                }
                else
                {
                    break;
                }
            }

            if (wordIndex < 0)
            {
                return new SyncState(lineIndex, -1, 0, false);
            }

            LyricWord word = line.Words[wordIndex];
            double durationMs = (word.End - word.Start).TotalMilliseconds;
            double progress;
            if (durationMs <= 0)
            {
                progress = position >= word.Start ? 1.0 : 0.0;
            }
            else
            {
                progress = (position - word.Start).TotalMilliseconds / durationMs;
                if (progress < 0) progress = 0;
                if (progress > 1) progress = 1;
            }

            bool inGap = position >= line.End;
            return new SyncState(lineIndex, wordIndex, progress, inGap);
        }

        // Playback mostly moves forward one line at a time, so check the cached line and its
        // successor before falling back to a binary search (needed after a seek).
        private int FindActiveLine(TimeSpan position)
        {
            var lines = document.Lines;
            if (lines.Count == 0)
            {
                return -1;
            }

            if (lastLineIndex >= 0 && lastLineIndex < lines.Count)
            {
                if (IsActive(lastLineIndex, position))
                {
                    return lastLineIndex;
                }
                if (lastLineIndex + 1 < lines.Count && IsActive(lastLineIndex + 1, position))
                {
                    return lastLineIndex + 1;
                }
            }

            return document.FindLineIndex(position);
        }

        // "Active" = this is the last line that has started: Start <= position < next line's Start.
        private bool IsActive(int index, TimeSpan position)
        {
            var lines = document.Lines;
            if (lines[index].Start > position)
            {
                return false;
            }
            return index + 1 >= lines.Count || lines[index + 1].Start > position;
        }
    }
}