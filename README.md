# mb_modern_lyrics

`MusicBeePlugin.Parsing.EnhancedLyricsParser.Parse` reads LRC lyrics with
`[mm:ss.xx]` line timestamps and enhanced-LRC `<mm:ss.xxx>` word timestamps.
It also applies `[offset:milliseconds]` metadata and infers line and word end
times from the next timestamp. Pass the track duration to `Parse` when known to
set the end time of the final line.
