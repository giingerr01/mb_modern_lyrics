# MB Modern Lyrics

A MusicBee plugin for handling modern lyrics in a more advanced way than the default lyrics support. The project is written in C# and targets the MusicBee plugin API to parse and work with lyrics data from `.lrc` sources.

## Overview

This repository contains the plugin code for MusicBee, including:

- a MusicBee plugin entry point
- a custom enhanced `.lrc` parser
- lyric models for lines and words
- MusicBee API interface definitions required by the plugin

It is designed to support modern lyric formats, including metadata tags and word-timed lyric entries.

## Features

- MusicBee plugin integration
- Enhanced LRC parsing
- Support for lyric metadata such as offsets and tags
- Line-level and word-level timestamp handling
- Compatible with the MusicBee API structure used by plugin development

## Repository structure

- `MusicBeeInterface.cs` — MusicBee API bridge and plugin interface definitions
- `Modern_lyrics/MB_modern_lyrics.cs` — main plugin implementation and lifecycle hooks
- `Modern_lyrics/EnhancedLrcParser.cs` — parser for enhanced LRC content
- `Modern_lyrics/Models/LrcModels.cs` — lyric model classes used by the parser
- `MB_Modern_lyrics.csproj` — .NET project configuration
- `MB_Modern_lyrics.sln` — solution file

## Requirements

- Windows
- MusicBee installed
- .NET Framework 4.8
- Visual Studio or another compatible C# build environment

## Building the project

1. Open `MB_Modern_lyrics.sln` in Visual Studio.
2. Restore NuGet dependencies (if any are added later).
3. Build the solution in Debug or Release mode.

For local development and automatic deployment to MusicBee, the project expects a file named `MusicBee.local.targets` to define the plugin directory. Example:

```xml
<Project>
  <PropertyGroup>
    <MusicBeePluginDir>C:\Program Files\MusicBee\Plugins</MusicBeePluginDir>
  </PropertyGroup>
</Project>
```

This is referenced by the project file and allows the build step to copy the generated plugin DLL into MusicBee's plugin folder.

## Usage

After building, copy the generated plugin assembly into MusicBee's plugin directory and enable it from within MusicBee.

The plugin is intended to work with `.lrc` lyrics and modern lyric data that contains richer timing information than a simple static text file.

## Notes

This repository is still a plugin project in active development. The parser and plugin interface are set up to support richer lyrics workflows, but a full production feature set depends on the MusicBee plugin API and the specific lyrics sources used in the environment.

## License

This project is licensed under the MIT License.
