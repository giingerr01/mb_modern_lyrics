# Changelog

## [1.2.0] - 2026-11-17

### Added

- Added a configurable MusicBee settings panel for LRC lyrics, including font family, size, style, active-line color, and alignment controls.
- Added persistent plugin settings stored in MusicBee's persistent data folder under `LrcSettings/LrcSettings.json`, so the user’s preferred lyric appearance is retained between sessions.
- Added support for line-by-line LRC rendering with active-line highlighting, scroll anchoring, and alignment options for default, left, and right text placement.
- Added fallback lyric loading from sidecar `.lrc` files and support for common metadata tags, offsets, and repeated timestamp entries in the parser.

### Improved

- Improved the lyrics panel to use the MusicBee skin background and foreground colors more cleanly, keeping the UI readable and visually consistent with the host application.
- Improved the playback view logic to keep the current lyric line centered in the panel while maintaining smooth scrolling and preventing the panel from showing blank space at the top.
- Improved rendering quality by switching the graphics hint to ClearType grid fitting and updating the default font handling for sharper lyric text.
- Improved the plugin metadata and configuration panel labels to present the feature as the “LRC Lyrics” panel and make the settings easier to understand.

### Fixed

- Fixed LRC parsing to strip leading vocal tags such as `V1:` and `V2:` before displaying lyrics.
- Fixed alignment option handling so the selected combo-box value matches the actual left/right display behavior.
- Fixed settings validation and save behavior so malformed JSON falls back to defaults instead of leaving the plugin in a broken state.
- Fixed cleanup and panel creation logic so the lyrics view is recreated safely and the current track loads correctly when the MusicBee panel is created.

### Technical Changes

- Removed the Costura/Fody integration from the project configuration to simplify the build and avoid extra packaging dependencies.
- Updated the project and plugin metadata to clarify the panel identity and configuration height for MusicBee.
- Kept local deployment support via `MusicBee.local.targets`, allowing a build to copy the plugin DLL into the MusicBee plugin directory when configured.

> Note: the repository is tagged as `v1.2.0`, but the assembly metadata in `Properties/AssemblyInfo.cs` still reports version `1.0.1.0`. That mismatch should be reconciled before the next formal public release.
