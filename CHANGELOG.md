# Changelog

All notable changes to the MB Modern Lyrics plugin will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.2.0] - 2026-11-19

### Added
- **Karaoke Sweep Animation:** Added a pseudo-karaoke sweep effect that smoothly highlights the active lyrics line from left to right based on line duration.
- **Enhanced LRC Support:** Added parser support for word-level ELRC timestamps (`<mm:ss.xxx>`), allowing for more accurate word-by-word sweeping on supported files.
- **Settings Toggle:** Added a checkbox in the settings panel to enable or disable the sweep animation, falling back to the standard static highlight.
- **Color Blending:** Implemented a gradient dimming effect for inactive lines based on their distance from the active line.

### Changed
- Refactored lyric drawing logic to use GDI+ `Graphics.SetClip` for precise text masking instead of drawing overlapping strings.
- Replaced `switch` statements for dimming opacity with a `static readonly` array for cleaner code and better performance.
- Updated font size clamping to prevent out-of-bounds exceptions when users select extreme font sizes.

### Fixed
- Fixed a critical bug where the sweep animation would render overlapping, jumbled text on multi-line lyrics due to incorrect string measurement.
- Fixed a memory leak by properly caching and disposing of `SolidBrush` objects used for dimming, rather than allocating them 60 times a second in `OnPaint`.
- Fixed an issue where the inactive text color was not correctly updating when the MusicBee skin background changed (`OnBackColorChanged`).

### Removed
- Removed excessive `Logger.Log` calls from the `OnPaint` loop to improve rendering performance.
- Removed unused `SmoothingMode.AntiAlias` call and unnecessary `using` statements.

## [1.1.1] - 2026-11-01
*(Note: You will need to fill in your older changes here based on your actual Git history. Here is an example of what it might look like)*

### Fixed
- Resolved threading issues when the dockable panel is created (`InvokeRequired`).
- Fixed an issue where reading sidecar LRC files would fail if the file extension case didn't match.
- Corrected settings persistence to properly save and load active and inactive colors.

## [1.0.0] - 2026-10-15

### Added
- Initial release of the MB Modern Lyrics plugin.
- Line-by-line synchronized LRC parsing.
- Support for embedded lyrics and sidecar `.lrc` files.
- Customizable font, size, alignment, and active/inactive text colors.
- Smooth vertical scrolling animation for active lines.
- Integration with MusicBee skin colors.