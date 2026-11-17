# Changelog

## [1.2.0] - 2026-11-17

### Breaking Changes

- **`MB_Modern_lyrics.csproj` / `packages.config`:** Adds Newtonsoft.Json 13.0.4; restore NuGet packages before building the .NET Framework 4.8 project.

### Architecture & Structure

- **`ModernLyricsPlugins` / `LRCSettings`:** Connects the MusicBee configuration panel to live view updates and persists settings in `LrcSettings/LrcSettings.json` under MusicBee's persistent-storage path.

### Added

- **`Enhanced_lyrics/LRCSettings.cs`:** Adds configuration controls for font family, size and style, active-line color, and lyrics alignment.
- **`LyricsViewControl`:** Adds runtime setters for font, active/inactive colors, and near/center/far line alignment.

### Changed

- **`LineByLineLyricsViewControl.cs`:** Uses ClearTypeGridFit for sharper lyric text and adjusts the upper scroll clamp from -8 to -10; the rendering mode trades some scrolling smoothness for clarity.
- **`PluginInfo` / `Properties/AssemblyInfo.cs`:** Updates the MusicBee plugin name and assembly product metadata for the LRC Lyrics identity.

### Fixed

- **`LineByLineLyricsParser.cs`:** Removes leading `V<number>:` vocal markers from timed and unsynchronized lyric text so they are not rendered as part of the lyrics.

### Handover Notes

- **`Lyrics-Settings` JSON object:** Stores `Font`, `FontSize`, `FontStyle`, `ActiveColor`, `UseCustomColor`, and `LyricsAlignment`; malformed JSON falls back to defaults and is rewritten on a later save.
- **Version metadata:** The repository tag is `v1.2.0`, but MusicBee `PluginInfo` reports 1.1.1 and assembly version attributes remain 1.0.1.0; align these before the next release.
- **`MusicBee.local.targets`:** Set `MusicBeePluginDir` for automatic deployment, or set `CopyPluginToMusicBee=false` to build without copying into MusicBee's plugin folder.
