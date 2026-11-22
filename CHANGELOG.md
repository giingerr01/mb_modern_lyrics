# Changelog

All notable changes to the MB Modern Lyrics plugin will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Changed
- Improved mouse-wheel scrolling and focus behavior in the lyrics display.

### Removed
- Removed the obsolete `CSharpDll.sln`, which referenced a nonexistent project; the solution matching `MB_Modern_lyrics.csproj` remains.

## [1.3.0] - 2026-11-19

### Added
- Added an optional pseudo-karaoke sweep animation for the active lyric line, with a setting to enable or disable it.
- Used word-level ELRC timestamps to synchronize sweep progress when available.

### Changed
- Improved inactive-line dimming and text rendering for clearer lyrics.
- Refined sweep drawing, animation handling, and settings layout, including the font preview.

### Fixed
- Prevented the lyrics scroll position from going below zero.
- Improved font selection and disposal, and ensured lyric colors update when the background changes.
- Corrected sweep rendering and inactive-line brush management.

## [1.2.0] - 2026-11-17

### Added
- Added a settings panel for font family, size, style, active-line color, and alignment.
- Added persistent storage for lyric appearance settings.
- Added line-by-line LRC display with active-line highlighting, scrolling, and alignment options.
- Added sidecar `.lrc` loading and parser support for metadata tags, offsets, repeated timestamps, and vocal-tag removal.

### Changed
- Improved playback synchronization, scroll positioning, and integration with MusicBee skin colors.
- Improved rendering quality and clarified the plugin and settings-panel labels.

### Fixed
- Corrected alignment selection behavior and settings validation and save handling.
- Improved panel creation and cleanup so the current track loads correctly.
