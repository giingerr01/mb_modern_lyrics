# Changelog

A commit-linked history of the repository. Each listed commit includes its tracked file changes, followed by the commit graph.

**History shown:** 127 commits, from 2026-10-29 through 2026-11-17.

**Feature overview:** The project evolved from an initial MusicBee lyrics plugin into an enhanced LRC parser and synchronized lyrics panel, then added line-by-line playback display, configurable appearance and alignment, persistent settings, improved skin/color handling, and build/package configuration changes.

## Commit history

### 2026-11-17

- [`39a7167`](https://github.com/giingerr01/mb_modern_lyrics/commit/39a71678f7327c5aa2fe21072ac88b6303897d85) — feat: add Costura and Fody configuration files for assembly embedding
  - Files: `A FodyWeavers.xml`; `A FodyWeavers.xsd`; `M MB_Modern_lyrics.csproj`; `M packages.config`
- [`d86edc2`](https://github.com/giingerr01/mb_modern_lyrics/commit/d86edc28cece3191db975e963ae7c3b8421ab51f) — chore: update CHANGELOG.md for version 1.2.0 with added features, improvements, and fixes
  - Files: `M CHANGELOG.md`
- [`3851540`](https://github.com/giingerr01/mb_modern_lyrics/commit/3851540bcd63b107c0494d6d44bb889e6e215f79) — feat: update LRCSettings and ModernLyricsPlugins for improved UI and functionality
  - Files: `M CHANGELOG.md`; `M Enhanced_lyrics/LRCSettings.Designer.cs`; `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`e6eb059`](https://github.com/giingerr01/mb_modern_lyrics/commit/e6eb0591b05d04ee79532424780b49201680d136) — Merge branch 'feature/line-by-line' wit base feature implemaiton and main stable branch Version: v1.2.0
  - Files: `A Enhanced_lyrics/LRCSettings.Designer.cs`; `A Enhanced_lyrics/LRCSettings.cs`; `A Enhanced_lyrics/LRCSettings.resx`; `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsParser.cs`; `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`; `A Enhanced_lyrics/Logger.cs`; `M Enhanced_lyrics/ModernLyricsPlugins.cs`; `M MB_Modern_lyrics.csproj`; `M Properties/AssemblyInfo.cs`; `A packages.config`
- [`b48a6e2`](https://github.com/giingerr01/mb_modern_lyrics/commit/b48a6e27f535bfcf6a1b56b7978a7411d2255954) — refactor: add *packages to .gitignore for improved build cleanliness
  - Files: `M .gitignore`
- [`16ecc74`](https://github.com/giingerr01/mb_modern_lyrics/commit/16ecc74481e4bf9a75540bc53c1512ff0506f1bb) — chore: remove Costura and Fody dependencies from project configuration and package files
  - Files: `D FodyWeavers.xml`; `D FodyWeavers.xsd`; `M MB_Modern_lyrics.csproj`; `M packages.config`
- [`d607a81`](https://github.com/giingerr01/mb_modern_lyrics/commit/d607a81441afbd013f6a0a3d513f34ee57dc4cd4) — feat: add lyrics alignment support in LineByLineLyricsViewControl and ModernLyricsPlugins
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`; `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`6c28c0a`](https://github.com/giingerr01/mb_modern_lyrics/commit/6c28c0a7ce709033538974051192076f5380fbd9) — fix: correct typo in alignmentComboBox options and add event handler for selection change
  - Files: `M Enhanced_lyrics/LRCSettings.Designer.cs`
- [`b11be29`](https://github.com/giingerr01/mb_modern_lyrics/commit/b11be2998b722e69a97b14996455756d86340e4a) — feat: add support for lyrics alignment selection in LRCSettings
  - Files: `M Enhanced_lyrics/LRCSettings.cs`
- [`5c40fee`](https://github.com/giingerr01/mb_modern_lyrics/commit/5c40fee011541981d28036f31662353f68e61cea) — feat: add FodyWeavers and update project configuration for Costura integration
  - Files: `A FodyWeavers.xml`; `A FodyWeavers.xsd`; `M MB_Modern_lyrics.csproj`; `M packages.config`
- [`b6f9f08`](https://github.com/giingerr01/mb_modern_lyrics/commit/b6f9f085b04973f7505cf0ff218857ae5d775491) — feat: add alignmentComboBox to LRCSettings for text alignment options
  - Files: `M Enhanced_lyrics/LRCSettings.Designer.cs`
- [`ad1048e`](https://github.com/giingerr01/mb_modern_lyrics/commit/ad1048e5f2aa677624c68d2e810405db909c3fbe) — feat: update LRCSettings to include active color and custom color usage in lyrics display
  - Files: `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`70195af`](https://github.com/giingerr01/mb_modern_lyrics/commit/70195af807e256c0cffc0c93e0700b286b694b79) — feat: enhance LRCSettings to support custom active color and initialization logic
  - Files: `M Enhanced_lyrics/LRCSettings.cs`
- [`0eb72c2`](https://github.com/giingerr01/mb_modern_lyrics/commit/0eb72c25b602d90320ae3e32f368218ac88f895f) — feat: add support for custom color setting in lyrics configuration
  - Files: `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`e4de469`](https://github.com/giingerr01/mb_modern_lyrics/commit/e4de46937e8f09feaabb9c649d01ebafcc1c5228) — fix: set to use cleartypegridfit fot Rendering hint
  - Files: `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`d025ae7`](https://github.com/giingerr01/mb_modern_lyrics/commit/d025ae76133dc67a2151ed5c562aa2c7eaacb019) — Merge branch 'storage-persis' into feature/settings-panel done with font dialog events and storage persistent for he configuration of the panel.
  - Files: `M Enhanced_lyrics/LRCSettings.cs`; `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`; `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`2d473c8`](https://github.com/giingerr01/mb_modern_lyrics/commit/2d473c814e7d0645b7b1d86467fd50347de17182) — refactor: enhance font handling by adding FontStyle support in LRCSettings and related components
  - Files: `M Enhanced_lyrics/LRCSettings.cs`; `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`; `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`2d83676`](https://github.com/giingerr01/mb_modern_lyrics/commit/2d836769314cc10670d18b51e7231a223fc909fc) — refactor: add RemoveVocalTags method to clean lyrics and enhance parsing logic
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsParser.cs`
- [`74235dd`](https://github.com/giingerr01/mb_modern_lyrics/commit/74235dd20b22aeee3963c6a51f5a1024b0fc6a15) — refactor: update plugin name and product description for clarity
  - Files: `M Enhanced_lyrics/ModernLyricsPlugins.cs`; `M Properties/AssemblyInfo.cs`
- [`4d8db11`](https://github.com/giingerr01/mb_modern_lyrics/commit/4d8db11cddf302aeb55c20425563daf63c437efb) — refactor: improve error handling in OnDockablePanelCreated method
  - Files: `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`779e6e6`](https://github.com/giingerr01/mb_modern_lyrics/commit/779e6e66b0bc19e09f76ffcf0f0af2c3a705429e) — refactor: add backColor property and update BackColor assignment in LyricsViewControl
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`
- [`215208f`](https://github.com/giingerr01/mb_modern_lyrics/commit/215208fb171e46ad4a1567e0b0017493de71579f) — refactor: enhance LRCSettings initialization with background color and update UI elements accordingly
  - Files: `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`7ae5a33`](https://github.com/giingerr01/mb_modern_lyrics/commit/7ae5a33812b2557e5791096341b10bafa9b66251) — refactor: adjust layout of fontSizeBox and CustomColorCheckBox in LRCSettings
  - Files: `M Enhanced_lyrics/LRCSettings.Designer.cs`
- [`3855dc5`](https://github.com/giingerr01/mb_modern_lyrics/commit/3855dc5e548d9f18d45df6d740604c43a6507cad) — refactor: update LRCSettings constructor to accept background color and adjust UI elements accordingly
  - Files: `M Enhanced_lyrics/LRCSettings.cs`

### 2026-11-16

- [`99fba77`](https://github.com/giingerr01/mb_modern_lyrics/commit/99fba77b695bfd4a49beeecb9aef2bbcf946147a) — refactor: clean up SetActiveColor method and remove unnecessary comments in ClampScroll
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`
- [`88a5f62`](https://github.com/giingerr01/mb_modern_lyrics/commit/88a5f624c56c583a4b44fbc2c2078454d2e9db71) — refactor: improve logging and color handling in ModernLyricsPlugins
  - Files: `M Enhanced_lyrics/Logger.cs`; `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`5e6b3cd`](https://github.com/giingerr01/mb_modern_lyrics/commit/5e6b3cd30f4630a59e24b99390331bd34d54527b) — refactor: enhance persistent storage for LRC settings and implement SaveSettings method
  - Files: `M Enhanced_lyrics/LRCSettings.cs`; `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`596ea29`](https://github.com/giingerr01/mb_modern_lyrics/commit/596ea293725bcde7553d45875192eeb0a7064874) — refactor: add Logger.cs to project for enhanced logging functionality
  - Files: `A Enhanced_lyrics/Logger.cs`; `M MB_Modern_lyrics.csproj`
- [`906bbf7`](https://github.com/giingerr01/mb_modern_lyrics/commit/906bbf70802ea7de5cc237b2237e6f4edbdf3d2a) — refactor: update .gitignore and add Newtonsoft.Json reference in project files
  - Files: `M .gitignore`; `M MB_Modern_lyrics.csproj`; `A packages.config`
- [`e1ffc03`](https://github.com/giingerr01/mb_modern_lyrics/commit/e1ffc0398bf69db4b72ac82c73b3b164cfd2ed92) — Merge branch 'feature/settings-panel' into feature/line-by-line
  - Files: `A Enhanced_lyrics/LRCSettings.Designer.cs`; `A Enhanced_lyrics/LRCSettings.cs`; `A Enhanced_lyrics/LRCSettings.resx`; `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`; `M Enhanced_lyrics/ModernLyricsPlugins.cs`; `M MB_Modern_lyrics.csproj`
- [`81333fa`](https://github.com/giingerr01/mb_modern_lyrics/commit/81333fa557751e6cf0e63481ed03aab4518c025b) — refactor: add event handling for ActiveColorChanged in LRCSettings to update lyrics color dynamically
  - Files: `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`0ed5ab5`](https://github.com/giingerr01/mb_modern_lyrics/commit/0ed5ab54c3fa763d28e49f02d2236cc924d6eb7b) — refactor: add CustomColorCheckBox for dynamic text color selection and update related event handling
  - Files: `M Enhanced_lyrics/LRCSettings.Designer.cs`; `M Enhanced_lyrics/LRCSettings.cs`
- [`1603d8e`](https://github.com/giingerr01/mb_modern_lyrics/commit/1603d8e61a9446bdcdcee44510f37dca40916771) — refactor: update brush initialization and add SetActiveColor method for dynamic color handling
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`
- [`98134b6`](https://github.com/giingerr01/mb_modern_lyrics/commit/98134b6cbab207637dc7465b1e6a12019124d941) — refactor: adjust LyricsLabel and fontSizeBox positions for improved layout
  - Files: `M Enhanced_lyrics/LRCSettings.Designer.cs`
- [`36f2895`](https://github.com/giingerr01/mb_modern_lyrics/commit/36f28950196184db2d097ac2774780d134b18b22) — refactor: update text rendering hint for improved clarity in lyrics display
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`
- [`ed93ebe`](https://github.com/giingerr01/mb_modern_lyrics/commit/ed93ebe471d28d4d75e0616e80fc32135392fc6b) — refactor: enhance LRCSettings initialization and font handling in configuration
  - Files: `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`ce6919f`](https://github.com/giingerr01/mb_modern_lyrics/commit/ce6919fe572b535dcdefaeeb284083d9eb557c24) — refactor: update font variable to private and adjust font size setter to accept font parameter
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`
- [`97e753f`](https://github.com/giingerr01/mb_modern_lyrics/commit/97e753f25f5c66dc0ec2a53715667374a5261dca) — refactor: add fontDialog1.TrayLocation metadata with default position
  - Files: `M Enhanced_lyrics/LRCSettings.resx`
- [`97315e7`](https://github.com/giingerr01/mb_modern_lyrics/commit/97315e7ef32c384329cefdbaec5884853a58e62f) — refactor: adjust fontSizeBox size and update LRCSettings background properties
  - Files: `M Enhanced_lyrics/LRCSettings.Designer.cs`
- [`60125b0`](https://github.com/giingerr01/mb_modern_lyrics/commit/60125b0051ef7ddddcb624934a93c52145a900cc) — refactor: add empty implementation for fontSizeBox_TextChanged event handler
  - Files: `M Enhanced_lyrics/LRCSettings.cs`
- [`2aeba3d`](https://github.com/giingerr01/mb_modern_lyrics/commit/2aeba3d299ee41fafca8eb6c2088bc63c3df19da) — refactor: update LRCSettings layout and add font selector functionality
  - Files: `M Enhanced_lyrics/LRCSettings.Designer.cs`
- [`6cc88b4`](https://github.com/giingerr01/mb_modern_lyrics/commit/6cc88b4e28bd21dc0a7cdb20fb9f595930fa3fff) — refactor: update LRCSettings constructor to accept font and font size parameters and initialize fontSizeBox
  - Files: `M Enhanced_lyrics/LRCSettings.cs`
- [`0b89bf2`](https://github.com/giingerr01/mb_modern_lyrics/commit/0b89bf2f0a595cf09b9d6e70ffce83f24485e568) — refactor: update font initialization and adjust scroll clamp logic
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`
- [`f8b5673`](https://github.com/giingerr01/mb_modern_lyrics/commit/f8b5673b731e5cc1f543da119ffbc7cb45eaa35a) — refactor: initialize fontSizeBox in LRCSettings constructor and update font variable in LineByLineLyricsViewControl
  - Files: `M Enhanced_lyrics/LRCSettings.cs`; `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`; `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`1bfdfb5`](https://github.com/giingerr01/mb_modern_lyrics/commit/1bfdfb5d64212b0248d58d9815cacc8cb52d6187) — refactor: add LRCSettings designer and resource files for UI component setup
  - Files: `A Enhanced_lyrics/LRCSettings.Designer.cs`; `A Enhanced_lyrics/LRCSettings.cs`; `A Enhanced_lyrics/LRCSettings.resx`
- [`7bb103b`](https://github.com/giingerr01/mb_modern_lyrics/commit/7bb103bccc9e2dbc0d81f447c357ede500e1817b) — refactor: rename SettingsPanel to LRCSettings for consistency and clarity
  - Files: `M MB_Modern_lyrics.csproj`
- [`0e12a40`](https://github.com/giingerr01/mb_modern_lyrics/commit/0e12a40ee8da8d821ca96041bf0da8961bdd397a) — refactor: replace SettingsPanel with LRCSettings for improved font size management
  - Files: `M Enhanced_lyrics/ModernLyricsPlugins.cs`; `D Enhanced_lyrics/SettingsPanel.Designer.cs`; `D Enhanced_lyrics/SettingsPanel.cs`; `D Enhanced_lyrics/SettingsPanel.resx`
- [`3ac0dd5`](https://github.com/giingerr01/mb_modern_lyrics/commit/3ac0dd5bd27ec8081ff1f30250c04deb35755104) — refactor: implement SettingsPanel for dynamic font size adjustment and persist settings
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`; `M Enhanced_lyrics/ModernLyricsPlugins.cs`; `A Enhanced_lyrics/SettingsPanel.Designer.cs`; `A Enhanced_lyrics/SettingsPanel.cs`; `A Enhanced_lyrics/SettingsPanel.resx`; `M MB_Modern_lyrics.csproj`
- [`59a7b43`](https://github.com/giingerr01/mb_modern_lyrics/commit/59a7b43cc164ea1aca722928846022f3b3a99f05) — refactor: adjust scroll clamp to prevent blank panel display and improve text rendering clarity
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`
- [`b847f48`](https://github.com/giingerr01/mb_modern_lyrics/commit/b847f48ec26a828715b8dfb991c2330b3ee32212) — refactor: update fontSize to 10 for improved readability
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`
- [`5d8623c`](https://github.com/giingerr01/mb_modern_lyrics/commit/5d8623c3ae1c726d6dd4afa67153a49c0147c459) — refactor: removed antialias paint method and revised it to use  gridfitfor clear text rendering
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`
- [`28badf6`](https://github.com/giingerr01/mb_modern_lyrics/commit/28badf6fe6f36f3d2e9f51e6bee5bcce6cbc795a) — refactor: streamline font initialization and remove SettingsPanel integration from Configure method
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`; `M Enhanced_lyrics/ModernLyricsPlugins.cs`

### 2026-11-15

- [`f493fa3`](https://github.com/giingerr01/mb_modern_lyrics/commit/f493fa394e1f4dddb5da2dd5ad40ba445c134f8d) — refactor: update fontSize to readonly and remove SettingsPanel references from project
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`; `A Enhanced_lyrics/ModernLyricsPlugins.cs`; `M MB_Modern_lyrics.csproj`
- [`98e7971`](https://github.com/giingerr01/mb_modern_lyrics/commit/98e7971cd49bee0a12e4823b1d2b299e6a5e83e3) — refactor: remove unused SettingsPanel files and related code
  - Files: `D Enhanced_lyrics/LineByLineLyrics/Settings/SettingsPanel.Designer.cs`; `D Enhanced_lyrics/LineByLineLyrics/Settings/SettingsPanel.cs`; `D Enhanced_lyrics/LineByLineLyrics/Settings/SettingsPanel.resx`; `D Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`251e3d6`](https://github.com/giingerr01/mb_modern_lyrics/commit/251e3d66f1f6b0e3de47b5095cda56c28d68dda8) — refactor: simplify SettingsPanel initialization and remove unused event handler
  - Files: `M Enhanced_lyrics/LineByLineLyrics/Settings/SettingsPanel.Designer.cs`; `M Enhanced_lyrics/LineByLineLyrics/Settings/SettingsPanel.cs`; `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`ca855c1`](https://github.com/giingerr01/mb_modern_lyrics/commit/ca855c1275598a695d9277e04ffdee3332db59e6) — feat: add SettingsPanel.resx for resource management in line-by-line lyrics customization
  - Files: `A Enhanced_lyrics/LineByLineLyrics/Settings/SettingsPanel.resx`
- [`5bdfa5e`](https://github.com/giingerr01/mb_modern_lyrics/commit/5bdfa5e4618464c4726d5f89ab489abbb297666b) — feat: integrate SettingsPanel into Configure method for enhanced user settings
  - Files: `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`83e7141`](https://github.com/giingerr01/mb_modern_lyrics/commit/83e714190818e32c59224321a53666d4797894eb) — feat: add SettingsPanel.Designer.cs for font size customization
  - Files: `A Enhanced_lyrics/LineByLineLyrics/Settings/SettingsPanel.Designer.cs`
- [`3adae0e`](https://github.com/giingerr01/mb_modern_lyrics/commit/3adae0e2e31e0245a187a7a2e5fc319ced82287d) — feat: implement SettingsPanel for line-by-line lyrics customization
  - Files: `A Enhanced_lyrics/LineByLineLyrics/Settings/SettingsPanel.cs`
- [`6bbe7fa`](https://github.com/giingerr01/mb_modern_lyrics/commit/6bbe7fa5b3a97eb0d3c5c39a28879d363c1b847c) — fix: change SubType of SettingsPanel from Form to UserControl
  - Files: `M MB_Modern_lyrics.csproj`
- [`e396bcb`](https://github.com/giingerr01/mb_modern_lyrics/commit/e396bcb6ab92cab4fa9347d39a77d0457f8467bc) — fix: correct formatting in IntializeFonts method for inactiveFont initialization
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`
- [`00f00fb`](https://github.com/giingerr01/mb_modern_lyrics/commit/00f00fb8ab5b891c1f2f911f62f03601e3c9ae38) — feat: refactor font handling in LyricsViewControl and remove SettingsPanel files
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`; `D Enhanced_lyrics/LineByLineLyrics/Settings/SettingsPanel.Designer.cs`; `D Enhanced_lyrics/LineByLineLyrics/Settings/SettingsPanel.cs`; `D Enhanced_lyrics/LineByLineLyrics/Settings/SettingsPanel.resx`
- [`c01332b`](https://github.com/giingerr01/mb_modern_lyrics/commit/c01332b62c76e246adf43c4fd3a76ec2aa9fb034) — feat: add SettingsPanel.resx for line-by-line lyrics settings
  - Files: `A Enhanced_lyrics/LineByLineLyrics/Settings/SettingsPanel.resx`
- [`ab5a51b`](https://github.com/giingerr01/mb_modern_lyrics/commit/ab5a51b2d8c32fe80b11e0063a54860c098eab00) — feat: add SettingsPanel.Designer.cs for line-by-line lyrics customization
  - Files: `A Enhanced_lyrics/LineByLineLyrics/Settings/SettingsPanel.Designer.cs`
- [`55aface`](https://github.com/giingerr01/mb_modern_lyrics/commit/55afacebbaf6a87505805693e7c0c74355a45a75) — feat: add SettingsPanel class for line-by-line lyrics settings
  - Files: `A Enhanced_lyrics/LineByLineLyrics/Settings/SettingsPanel.cs`
- [`48476c3`](https://github.com/giingerr01/mb_modern_lyrics/commit/48476c3ee164f95e1b7766ba778e9f0a10de48f7) — feat: add SettingsPanel and its designer to LineByLineLyrics
  - Files: `M MB_Modern_lyrics.csproj`
- [`9a61fee`](https://github.com/giingerr01/mb_modern_lyrics/commit/9a61fee1f2b21d8310e2889a66be1f0df201040f) — fix: correct MusicBeePluginDir variable reference in project file refactor: adjust line gap and wheel step in LyricsViewControl for better layout refactor: modify LRC line splitting logic for improved handling
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsParser.cs`; `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`; `M MB_Modern_lyrics.csproj`
- [`7c4045a`](https://github.com/giingerr01/mb_modern_lyrics/commit/7c4045aa3a04e85c9997f6ad3a7215c2601e544f) — refactor: improve LRC parsing by removing empty lines and trimming whitespace
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsParser.cs`
- [`ede4624`](https://github.com/giingerr01/mb_modern_lyrics/commit/ede462428b15ca199d4741b757039f08e5bb2a60) — fix: type for complie included files in projet
  - Files: `M MB_Modern_lyrics.csproj`
- [`9745ec3`](https://github.com/giingerr01/mb_modern_lyrics/commit/9745ec3c6a0db89ee42f223c817bd93689851fbf) — refactor: removed unuse ApplySkinColors() funcition
  - Files: `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`cedf653`](https://github.com/giingerr01/mb_modern_lyrics/commit/cedf653cf7ba03b2d6087fa6bda1180d8a48f223) — fix: coreected panel and text lyrics color (hardocded)
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`
- [`e76d0da`](https://github.com/giingerr01/mb_modern_lyrics/commit/e76d0da556b5e183579f0aea150b479e0153a60e) — Add CHANGELOG.md to document version history and commit details
  - Files: `A CHANGELOG.md`
- [`5414f76`](https://github.com/giingerr01/mb_modern_lyrics/commit/5414f7622f1b244367ed54fe29f5aaaeeb53360b) — Refactor ApplySkinColors method and reorganize plugin lifecycle methods for improved clarity and functionality
  - Files: `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`4ee5b79`](https://github.com/giingerr01/mb_modern_lyrics/commit/4ee5b79db39916d5332e9d10c1d68c9dea9bd59a) — Enhance LineByLineLyricsViewControl layout and update ApplySkinColors method for dynamic skin support
  - Files: `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`; `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`67fa0b3`](https://github.com/giingerr01/mb_modern_lyrics/commit/67fa0b31bd64e426a1084e904edfd4f455db05e2) — Refactor project structure by updating .csproj file to include specific source files and enhance control handling in ModernLyricsPlugins
  - Files: `M .gitignore`; `M Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`; `M Enhanced_lyrics/ModernLyricsPlugins.cs`; `M MB_Modern_lyrics.csproj`

### 2026-11-14

- [`e80d9c6`](https://github.com/giingerr01/mb_modern_lyrics/commit/e80d9c69d4950b85270aea389707f62ef7589b84) — Add debug logging and improve thread handling in plugin methods
  - Files: `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`ec3f80e`](https://github.com/giingerr01/mb_modern_lyrics/commit/ec3f80ef65c3510a5f7b400bef2f2e46d900ad8b) — Update plugin target application and enhance lyrics loading in OnDockablePanelCreated method
  - Files: `M Enhanced_lyrics/ModernLyricsPlugins.cs`
- [`95cce0f`](https://github.com/giingerr01/mb_modern_lyrics/commit/95cce0f9e6657beaec0ace708aab0620fc52ef20) — Update project file to change source folder from Modern_lyrics to Enhanced_lyrics
  - Files: `M MB_Modern_lyrics.csproj`
- [`18ec5db`](https://github.com/giingerr01/mb_modern_lyrics/commit/18ec5db85332a3434d3e35eed229337789c9cd33) — Refactor and enhance the LRC Lyrics Panel by migrating to a new structure, including updated parser and view control implementations.
  - Files: `A Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsParser.cs`; `A Enhanced_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`; `A Enhanced_lyrics/LineByLineLyrics/Models/LineByLineLyricsModels.cs`; `A Enhanced_lyrics/ModernLyricsPlugins.cs`; `D Modern_lyrics/LineByLineLyrics/LineByLineLyricsParser.cs`; `D Modern_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`; `D Modern_lyrics/LineByLineLyrics/Models/LineByLineLyricsModels.cs`; `D Modern_lyrics/ModernLyricsPlugins.cs`; `M Properties/AssemblyInfo.cs`
- [`b96e922`](https://github.com/giingerr01/mb_modern_lyrics/commit/b96e9221fa92abe37b6f0becc128a74d6b897bbb) — Refactor OnDockablePanelCreated method to use System.Timers.Timer and improve timer handling
  - Files: `M Modern_lyrics/ModernLyricsPlugins.cs`
- [`d318b6a`](https://github.com/giingerr01/mb_modern_lyrics/commit/d318b6a0bad23f984bbfa7f1f6f9c339356d039b) — Refactor position timer initialization and enhance UI thread handling for notifications
  - Files: `M Modern_lyrics/ModernLyricsPlugins.cs`
- [`8d49fec`](https://github.com/giingerr01/mb_modern_lyrics/commit/8d49fec2b5fa5ccf9aa05c50ed21fb612cfda1db) — Refactor LyricsViewControl for improved layout handling and scrolling behavior
  - Files: `M Modern_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`
- [`1f70828`](https://github.com/giingerr01/mb_modern_lyrics/commit/1f70828e99d7a6c595a9feee06c8a5a1cbb64385) — Enhance LrcParser with improved regex patterns and handling for unsynchronized lyrics
  - Files: `M Modern_lyrics/LineByLineLyrics/LineByLineLyricsParser.cs`
- [`89bae7d`](https://github.com/giingerr01/mb_modern_lyrics/commit/89bae7dfebf605e81febea7bc9aa254e3980857b) — Refactor LRC Lyrics Panel plugin details and improve functionality
  - Files: `M Modern_lyrics/ModernLyricsPlugins.cs`
- [`c841eeb`](https://github.com/giingerr01/mb_modern_lyrics/commit/c841eebb0db9b502bbc13772cd8f7a54bd25bb3f) — Remove empty .gitkeep file from WordByWordLyrics directory
  - Files: `D Modern_lyrics/WordByWordLyrics/.gitkeep`
- [`e786d08`](https://github.com/giingerr01/mb_modern_lyrics/commit/e786d081649724fb24a178b5e111c4f31372ae56) — Add MusicBeeInterface.cs with API definitions and structures for plugin development
  - Files: `M MB_Modern_lyrics.csproj`; `D Modern_lyrics/MusicBeeInterface.cs`; `A MusicBeeInterface.cs`
- [`cf3f6ce`](https://github.com/giingerr01/mb_modern_lyrics/commit/cf3f6cedf2fcc9c66b4183b4cd71d6c3654615e4) — Fix assembly name casing in project file for consistency
  - Files: `M MB_Modern_lyrics.csproj`
- [`9910950`](https://github.com/giingerr01/mb_modern_lyrics/commit/9910950b3835a37444ca59be040ff932857b7c07) — Update plugin info to reflect new name and target application
  - Files: `M Modern_lyrics/ModernLyricsPlugins.cs`
- [`c0569e0`](https://github.com/giingerr01/mb_modern_lyrics/commit/c0569e0a856c3118ab82f34472931d84ce313eda) — Update plugin info version to 1 for enhanced lyrics panel
  - Files: `M Modern_lyrics/ModernLyricsPlugins.cs`
- [`19ecc78`](https://github.com/giingerr01/mb_modern_lyrics/commit/19ecc789662d7805cc2f0c72e7930f52c36859dd) — Refactor lyrics parsing and synchronization logic for improved handling of unsynchronized lyrics
  - Files: `M MB_Modern_lyrics.csproj`; `M Modern_lyrics/LineByLineLyrics/LineByLineLyricsParser.cs`; `M Modern_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`; `M Modern_lyrics/LineByLineLyrics/Models/LineByLineLyricsModels.cs`; `M Modern_lyrics/ModernLyricsPlugins.cs`
- [`728b8c7`](https://github.com/giingerr01/mb_modern_lyrics/commit/728b8c7e78e5b2971fa2eb2d0ce2a94317aaa828) — Add MusicBeeInterface class for plugin API integration
  - Files: `M MB_Modern_lyrics.csproj`; `A Modern_lyrics/MusicBeeInterface.cs`; `D MusicBeeInterface.cs`
- [`f0f0e83`](https://github.com/giingerr01/mb_modern_lyrics/commit/f0f0e835d5ed2ccb2de8402d4ee99596a6f29e22) — Add .gitkeep file to WordByWordLyrics directory for version control
  - Files: `A Modern_lyrics/WordByWordLyrics/.gitkeep`
- [`47183be`](https://github.com/giingerr01/mb_modern_lyrics/commit/47183bea1b30daf9f3bcf4afb0a7fd301afda546) — Add LineByLineLyricsModels for structured lyric line and parsed lyrics representation
  - Files: `A Modern_lyrics/LineByLineLyrics/Models/LineByLineLyricsModels.cs`
- [`029ad10`](https://github.com/giingerr01/mb_modern_lyrics/commit/029ad106b77f1ba87d1d46711793bf7d3450c145) — Add LyricsViewControl for displaying line-by-line lyrics with dynamic updates
  - Files: `A Modern_lyrics/LineByLineLyrics/LineByLineLyricsViewControl.cs`
- [`1359b59`](https://github.com/giingerr01/mb_modern_lyrics/commit/1359b59c04bd9d9c19d4cbb9ced3614c0e001ae5) — Add Enhanced LrcParser for line-by-line lyrics parsing and offset handling
  - Files: `A Modern_lyrics/LineByLineLyrics/LineByLineLyricsParser.cs`
- [`2142473`](https://github.com/giingerr01/mb_modern_lyrics/commit/214247390dcefc0ab058e11c604765485d0a66d5) — Add Enhanced Lyrics Panel plugin with word-by-word lyrics parsing and playback synchronization
  - Files: `A Modern_lyrics/ModernLyricsPlugins.cs`
- [`2fd2716`](https://github.com/giingerr01/mb_modern_lyrics/commit/2fd27163b488612b99b9ae0383c999920add24a5) — Update project file to include all C# files in the Modern_lyrics directory
  - Files: `M MB_Modern_lyrics.csproj`
- [`1891ff0`](https://github.com/giingerr01/mb_modern_lyrics/commit/1891ff01b647ed91259b194cd74fcb038e50d35a) — Remove deprecated classes and files related to lyrics parsing and display
  - Files: `D Modern_lyrics/EnhancedLrcParser.cs`; `D Modern_lyrics/LyricsViewControl.cs`; `D Modern_lyrics/MB_modern_lyrics.cs`; `D Modern_lyrics/Models/LrcModels.cs`
- [`cd707df`](https://github.com/giingerr01/mb_modern_lyrics/commit/cd707dfde38ed483f148d5477589aa2bf6298c83) — Update plugin metadata and enhance lyrics loading functionality
  - Files: `M Modern_lyrics/EnhancedLrcParser.cs`; `M Modern_lyrics/MB_modern_lyrics.cs`
- [`bf4adc8`](https://github.com/giingerr01/mb_modern_lyrics/commit/bf4adc856af45fa15a5860cc37e5ba269f213718) — Add position timer and update lyrics view control on playback
  - Files: `M Modern_lyrics/MB_modern_lyrics.cs`
- [`4648cf6`](https://github.com/giingerr01/mb_modern_lyrics/commit/4648cf6bd7df0d3c773e5093a17d781fb27f20ae) — Add LyricsViewControl for displaying and updating lyrics with karaoke support
  - Files: `A Modern_lyrics/LyricsViewControl.cs`
- [`780f9a1`](https://github.com/giingerr01/mb_modern_lyrics/commit/780f9a17f0765b4da0f7338e50fe1e341bba0e37) — Refactor EnhancedLrcParser for improved regex handling and code clarity
  - Files: `M Modern_lyrics/EnhancedLrcParser.cs`
- [`39aa88b`](https://github.com/giingerr01/mb_modern_lyrics/commit/39aa88b05684b047c76d41031d488ce797f4dbce) — Add LyricsViewControl to project compilation
  - Files: `M MB_Modern_lyrics.csproj`
- [`585bdac`](https://github.com/giingerr01/mb_modern_lyrics/commit/585bdacf5975179bc6510c2b5d8663dadec5d2b5) — Fix end time assignment for lyric words and add clamp function for TimeSpan
  - Files: `M Modern_lyrics/EnhancedLrcParser.cs`
- [`6a56974`](https://github.com/giingerr01/mb_modern_lyrics/commit/6a56974a385169d4eb7438c7d56b49bfe7769312) — Implement word parsing and offset handling in EnhancedLrcParser
  - Files: `M Modern_lyrics/EnhancedLrcParser.cs`
- [`2cbbfff`](https://github.com/giingerr01/mb_modern_lyrics/commit/2cbbfffc8643db983fc5b69a1a6a7380079532eb) — Enhance lyric parsing by collecting multiple timestamps and associating words with their respective start times
  - Files: `M Modern_lyrics/EnhancedLrcParser.cs`
- [`e2953fa`](https://github.com/giingerr01/mb_modern_lyrics/commit/e2953fa68940f2d7eba546419418be68d5102409) — Refactor offset parsing and remove unused timestamp handling in EnhancedLrcParser
  - Files: `M Modern_lyrics/EnhancedLrcParser.cs`
- [`ad8af02`](https://github.com/giingerr01/mb_modern_lyrics/commit/ad8af02140f8b0c96b2ccd4e8f6de7d62bea9907) — Fix variable naming for clarity and maintain consistency in EnhancedLrcParser
  - Files: `M Modern_lyrics/EnhancedLrcParser.cs`
- [`d875a29`](https://github.com/giingerr01/mb_modern_lyrics/commit/d875a2983c05b3c33f55fdf12841cddb1d6bdc6b) — Refactor EnhancedLrcParser to improve timestamp handling and lyric line parsing
  - Files: `M Modern_lyrics/EnhancedLrcParser.cs`
- [`42eca51`](https://github.com/giingerr01/mb_modern_lyrics/commit/42eca5185ca3a5cfac940fa4402ea5512b0ef79a) — Merge branch 'main' of github.com:giingerr01/mb_modern_lyrics
  - Files: `M README.md`
- [`520b5b7`](https://github.com/giingerr01/mb_modern_lyrics/commit/520b5b7b7e5096618f013e1868606c2851d5aa6b) — Update plugin name and description for enhanced lyrics functionality
  - Files: `M Modern_lyrics/MB_modern_lyrics.cs`
- [`5516319`](https://github.com/giingerr01/mb_modern_lyrics/commit/55163190c133bb4b576cc5bc395b0d9e02d88108) — Remove trailing newline at the end of EnhancedLrcParser.cs
  - Files: `M Modern_lyrics/EnhancedLrcParser.cs`
- [`43bc95b`](https://github.com/giingerr01/mb_modern_lyrics/commit/43bc95b52effbdf58247392135ce93a432bf0a0f) — Fix assembly name in project file to match enhanced lyrics functionality
  - Files: `M MB_Modern_lyrics.csproj`

### 2026-11-01

- [`41b779f`](https://github.com/giingerr01/mb_modern_lyrics/commit/41b779f84cae03fc2ddbf333e2d1370514a9c53d) — Update README to describe the project and build process for the MusicBee lyrics plugin
  - Files: `M README.md`

### 2026-10-30

- [`9162017`](https://github.com/giingerr01/mb_modern_lyrics/commit/9162017a7451fad35a7b56f2cae61c03e5840aef) — Enhance EnhancedLrcParser to support word timing and full text generation for lyrics
  - Files: `M Modern_lyrics/EnhancedLrcParser.cs`
- [`823ba8c`](https://github.com/giingerr01/mb_modern_lyrics/commit/823ba8c7096d6b5e4abf5834274ad86f074598d5) — Enhance EnhancedLrcParser to support offset parsing and improve timestamp handling
  - Files: `M Modern_lyrics/EnhancedLrcParser.cs`
- [`070e501`](https://github.com/giingerr01/mb_modern_lyrics/commit/070e501abba59a08dfdff6481bfb342f89c7de67) — Fix formatting settings in settings.json for C# extension preference
  - Files: `M .vscode/settings.json`

### 2026-10-29

- [`6f9715e`](https://github.com/giingerr01/mb_modern_lyrics/commit/6f9715e0e6ad5fb16ff3ed3a1db4fb92c8630fdf) — Fix casing of 'System' in LrcModels.cs
  - Files: `M Modern_lyrics/Models/LrcModels.cs`
- [`25fab30`](https://github.com/giingerr01/mb_modern_lyrics/commit/25fab30ef3312a5888fa33090043014920f0173a) — Fix casing and typo in EnhancedLrcParser.cs
  - Files: `M Modern_lyrics/EnhancedLrcParser.cs`
- [`57b6b8d`](https://github.com/giingerr01/mb_modern_lyrics/commit/57b6b8d7647c7c6ae51e1f8267edd73d73ef340d) — Add LrcModels.cs and EnhancedLrcParser.cs to project file for compilation
  - Files: `M MB_Modern_lyrics.csproj`
- [`5d0c1d0`](https://github.com/giingerr01/mb_modern_lyrics/commit/5d0c1d04000ba23d9743a7c87ca45a9d7d9ddef3) — Add comment to clarify metadata tag checking in EnhancedLrcParser
  - Files: `M Modern_lyrics/EnhancedLrcParser.cs`
- [`8ab6afb`](https://github.com/giingerr01/mb_modern_lyrics/commit/8ab6afb7646c5a4ed5bfda2cc134726e09e5a773) — Add EnhancedLrcParser class to parse LRC formatted lyrics
  - Files: `A Modern_lyrics/EnhancedLrcParser.cs`
- [`c4d5564`](https://github.com/giingerr01/mb_modern_lyrics/commit/c4d5564e81de9656321901cae31ce3ebbc585904) — Add LrcModels.cs to define lyric data structures
  - Files: `A Modern_lyrics/Models/LrcModels.cs`
- [`ed78b06`](https://github.com/giingerr01/mb_modern_lyrics/commit/ed78b068416bfe60c70dc6b751c5a8a565ede3e3) — Add lyrics retrieval functionality and clean up commented code
  - Files: `M Modern_lyrics/MB_modern_lyrics.cs`
- [`757e555`](https://github.com/giingerr01/mb_modern_lyrics/commit/757e5557f580a69790e201c32e9124c2463fca4c) — Add MIT License to the project
  - Files: `A LICENSE`
- [`b466021`](https://github.com/giingerr01/mb_modern_lyrics/commit/b4660210058f03da5fe25feb57ace60a186b70b5) — Intial files
  - Files: `A .editorconfig`; `A .gitignore`; `A .vscode/extensions.json`; `A .vscode/settings.json`; `A .vscode/tasks.json`; `A MB_Modern_lyrics.csproj`; `A MB_Modern_lyrics.sln`; `A Modern_lyrics/MB_modern_lyrics.cs`; `A MusicBeeInterface.cs`; `A Properties/AssemblyInfo.cs`; `A app.config`
- [`52059b5`](https://github.com/giingerr01/mb_modern_lyrics/commit/52059b512e718ade3f2d96d780d126ab67595b29) — first commit
  - Files: `A README.md`

## Commit tree

```text
* 39a71678f732 2026-11-17  (HEAD -> main, origin/main, origin/HEAD) feat: add Costura and Fody configuration files for assembly embedding
* d86edc28cece 2026-11-17  chore: update CHANGELOG.md for version 1.2.0 with added features, improvements, and fixes
* 3851540bcd63 2026-11-17  feat: update LRCSettings and ModernLyricsPlugins for improved UI and functionality
*   e6eb0591b05d 2026-11-17  (tag: v1.2.0) Merge branch 'feature/line-by-line' wit base feature implemaiton and main stable branch Version: v1.2.0
|\
* | b48a6e27f535 2026-11-17  refactor: add *packages to .gitignore for improved build cleanliness
| * 16ecc74481e4 2026-11-17  chore: remove Costura and Fody dependencies from project configuration and package files
| * d607a81441af 2026-11-17  feat: add lyrics alignment support in LineByLineLyricsViewControl and ModernLyricsPlugins
| * 6c28c0a7ce70 2026-11-17  fix: correct typo in alignmentComboBox options and add event handler for selection change
| * b11be2998b72 2026-11-17  feat: add support for lyrics alignment selection in LRCSettings
| * 5c40fee01154 2026-11-17  feat: add FodyWeavers and update project configuration for Costura integration
| * b6f9f085b049 2026-11-17  feat: add alignmentComboBox to LRCSettings for text alignment options
| * ad1048e5f2aa 2026-11-17  feat: update LRCSettings to include active color and custom color usage in lyrics display
| * 70195af807e2 2026-11-17  feat: enhance LRCSettings to support custom active color and initialization logic
| * 0eb72c25b602 2026-11-17  feat: add support for custom color setting in lyrics configuration
| * e4de46937e8f 2026-11-17  fix: set to use cleartypegridfit fot Rendering hint
| *   d025ae76133d 2026-11-17  Merge branch 'storage-persis' into feature/settings-panel done with font dialog events and storage persistent for he configuration of the panel.
| |\
| | * 2d473c814e7d 2026-11-17  refactor: enhance font handling by adding FontStyle support in LRCSettings and related components
| * | 2d836769314c 2026-11-17  refactor: add RemoveVocalTags method to clean lyrics and enhance parsing logic
| * | 74235dd20b22 2026-11-17  refactor: update plugin name and product description for clarity
| |/
| * 4d8db11cddf3 2026-11-17  refactor: improve error handling in OnDockablePanelCreated method
| * 779e6e66b0bc 2026-11-17  refactor: add backColor property and update BackColor assignment in LyricsViewControl
| * 215208fb171e 2026-11-17  refactor: enhance LRCSettings initialization with background color and update UI elements accordingly
| * 7ae5a33812b2 2026-11-17  refactor: adjust layout of fontSizeBox and CustomColorCheckBox in LRCSettings
| * 3855dc5e548d 2026-11-17  refactor: update LRCSettings constructor to accept background color and adjust UI elements accordingly
| * 99fba77b695b 2026-11-16  refactor: clean up SetActiveColor method and remove unnecessary comments in ClampScroll
| * 88a5f624c56c 2026-11-16  refactor: improve logging and color handling in ModernLyricsPlugins
| * 5e6b3cd30f46 2026-11-16  refactor: enhance persistent storage for LRC settings and implement SaveSettings method
| * 596ea293725b 2026-11-16  refactor: add Logger.cs to project for enhanced logging functionality
| * 906bbf70802e 2026-11-16  refactor: update .gitignore and add Newtonsoft.Json reference in project files
| * e1ffc0398bf6 2026-11-16  Merge branch 'feature/settings-panel' into feature/line-by-line
|/|
| * 81333fa55775 2026-11-16  refactor: add event handling for ActiveColorChanged in LRCSettings to update lyrics color dynamically
| * 0ed5ab54c3fa 2026-11-16  refactor: add CustomColorCheckBox for dynamic text color selection and update related event handling
| * 1603d8e61a94 2026-11-16  refactor: update brush initialization and add SetActiveColor method for dynamic color handling
| * 98134b6cbab2 2026-11-16  refactor: adjust LyricsLabel and fontSizeBox positions for improved layout
| * 36f289501961 2026-11-16  refactor: update text rendering hint for improved clarity in lyrics display
| * ed93ebe471d2 2026-11-16  refactor: enhance LRCSettings initialization and font handling in configuration
| * ce6919fe572b 2026-11-16  refactor: update font variable to private and adjust font size setter to accept font parameter
| * 97e753f25f5c 2026-11-16  refactor: add fontDialog1.TrayLocation metadata with default position
| * 97315e7ef32c 2026-11-16  refactor: adjust fontSizeBox size and update LRCSettings background properties
| * 60125b0051ef 2026-11-16  refactor: add empty implementation for fontSizeBox_TextChanged event handler
| * 2aeba3d299ee 2026-11-16  refactor: update LRCSettings layout and add font selector functionality
| * 6cc88b4e28bd 2026-11-16  refactor: update LRCSettings constructor to accept font and font size parameters and initialize fontSizeBox
* | 0b89bf2f0a59 2026-11-16  refactor: update font initialization and adjust scroll clamp logic
| * f8b5673b731e 2026-11-16  refactor: initialize fontSizeBox in LRCSettings constructor and update font variable in LineByLineLyricsViewControl
| * 1bfdfb5d6421 2026-11-16  refactor: add LRCSettings designer and resource files for UI component setup
| * 7bb103bccc9e 2026-11-16  refactor: rename SettingsPanel to LRCSettings for consistency and clarity
| * 0e12a40ee8da 2026-11-16  refactor: replace SettingsPanel with LRCSettings for improved font size management
| * 3ac0dd5bd27e 2026-11-16  refactor: implement SettingsPanel for dynamic font size adjustment and persist settings
| * 59a7b43cc164 2026-11-16  refactor: adjust scroll clamp to prevent blank panel display and improve text rendering clarity
|/
* b847f48ec26a 2026-11-16  refactor: update fontSize to 10 for improved readability
* 5d8623c3ae1c 2026-11-16  refactor: removed antialias paint method and revised it to use  gridfitfor clear text rendering
* 28badf6fe6f3 2026-11-16  refactor: streamline font initialization and remove SettingsPanel integration from Configure method
* f493fa394e1f 2026-11-15  refactor: update fontSize to readonly and remove SettingsPanel references from project
* 98e7971cd49b 2026-11-15  refactor: remove unused SettingsPanel files and related code
* 251e3d66f1f6 2026-11-15  refactor: simplify SettingsPanel initialization and remove unused event handler
* ca855c127559 2026-11-15  feat: add SettingsPanel.resx for resource management in line-by-line lyrics customization
* 5bdfa5e46184 2026-11-15  feat: integrate SettingsPanel into Configure method for enhanced user settings
* 83e714190818 2026-11-15  feat: add SettingsPanel.Designer.cs for font size customization
* 3adae0e2e31e 2026-11-15  feat: implement SettingsPanel for line-by-line lyrics customization
* 6bbe7fa5b3a9 2026-11-15  fix: change SubType of SettingsPanel from Form to UserControl
* e396bcb6ab92 2026-11-15  fix: correct formatting in IntializeFonts method for inactiveFont initialization
* 00f00fb8ab5b 2026-11-15  feat: refactor font handling in LyricsViewControl and remove SettingsPanel files
* c01332b62c76 2026-11-15  feat: add SettingsPanel.resx for line-by-line lyrics settings
* ab5a51b2d8c3 2026-11-15  feat: add SettingsPanel.Designer.cs for line-by-line lyrics customization
* 55afacebbaf6 2026-11-15  feat: add SettingsPanel class for line-by-line lyrics settings
* 48476c3ee164 2026-11-15  feat: add SettingsPanel and its designer to LineByLineLyrics
* 9a61fee1f2b2 2026-11-15  fix: correct MusicBeePluginDir variable reference in project file refactor: adjust line gap and wheel step in LyricsViewControl for better layout refactor: modify LRC line splitting logic for improved handling
* 7c4045aa3a04 2026-11-15  refactor: improve LRC parsing by removing empty lines and trimming whitespace
* ede462428b15 2026-11-15  fix: type for complie included files in projet
* 9745ec3c6a0d 2026-11-15  refactor: removed unuse ApplySkinColors() funcition
* cedf653cf7ba 2026-11-15  fix: coreected panel and text lyrics color (hardocded)
* e76d0da556b5 2026-11-15  Add CHANGELOG.md to document version history and commit details
* 5414f7622f1b 2026-11-15  Refactor ApplySkinColors method and reorganize plugin lifecycle methods for improved clarity and functionality
* 4ee5b79db399 2026-11-15  (tag: v1.1.1) Enhance LineByLineLyricsViewControl layout and update ApplySkinColors method for dynamic skin support
* 67fa0b31bd64 2026-11-15  Refactor project structure by updating .csproj file to include specific source files and enhance control handling in ModernLyricsPlugins
* e80d9c69d495 2026-11-14  Add debug logging and improve thread handling in plugin methods
* ec3f80ef65c3 2026-11-14  Update plugin target application and enhance lyrics loading in OnDockablePanelCreated method
* 95cce0f9e665 2026-11-14  Update project file to change source folder from Modern_lyrics to Enhanced_lyrics
* 18ec5db85332 2026-11-14  Refactor and enhance the LRC Lyrics Panel by migrating to a new structure, including updated parser and view control implementations.
* b96e9221fa92 2026-11-14  Refactor OnDockablePanelCreated method to use System.Timers.Timer and improve timer handling
* d318b6a0bad2 2026-11-14  Refactor position timer initialization and enhance UI thread handling for notifications
* 8d49fec2b5fa 2026-11-14  Refactor LyricsViewControl for improved layout handling and scrolling behavior
* 1f70828e99d7 2026-11-14  Enhance LrcParser with improved regex patterns and handling for unsynchronized lyrics
* 89bae7dfebf6 2026-11-14  Refactor LRC Lyrics Panel plugin details and improve functionality
* c841eebb0db9 2026-11-14  Remove empty .gitkeep file from WordByWordLyrics directory
* e786d0816497 2026-11-14  Add MusicBeeInterface.cs with API definitions and structures for plugin development
* cf3f6cedf2fc 2026-11-14  Fix assembly name casing in project file for consistency
* 9910950b3835 2026-11-14  Update plugin info to reflect new name and target application
* c0569e0a856c 2026-11-14  Update plugin info version to 1 for enhanced lyrics panel
* 19ecc789662d 2026-11-14  Refactor lyrics parsing and synchronization logic for improved handling of unsynchronized lyrics
* 728b8c7e78e5 2026-11-14  Add MusicBeeInterface class for plugin API integration
* f0f0e835d5ed 2026-11-14  Add .gitkeep file to WordByWordLyrics directory for version control
* 47183bea1b30 2026-11-14  Add LineByLineLyricsModels for structured lyric line and parsed lyrics representation
* 029ad106b77f 2026-11-14  Add LyricsViewControl for displaying line-by-line lyrics with dynamic updates
* 1359b59c04bd 2026-11-14  Add Enhanced LrcParser for line-by-line lyrics parsing and offset handling
* 214247390dce 2026-11-14  Add Enhanced Lyrics Panel plugin with word-by-word lyrics parsing and playback synchronization
* 2fd27163b488 2026-11-14  Update project file to include all C# files in the Modern_lyrics directory
* 1891ff01b647 2026-11-14  Remove deprecated classes and files related to lyrics parsing and display
* cd707dfde38e 2026-11-14  (tag: v1.0.0) Update plugin metadata and enhance lyrics loading functionality
* bf4adc856af4 2026-11-14  Add position timer and update lyrics view control on playback
* 4648cf6bd7df 2026-11-14  Add LyricsViewControl for displaying and updating lyrics with karaoke support
* 780f9a17f076 2026-11-14  Refactor EnhancedLrcParser for improved regex handling and code clarity
* 39aa88b05684 2026-11-14  Add LyricsViewControl to project compilation
* 585bdacf5975 2026-11-14  Fix end time assignment for lyric words and add clamp function for TimeSpan
* 6a56974a3851 2026-11-14  Implement word parsing and offset handling in EnhancedLrcParser
* 2cbbfffc8643 2026-11-14  Enhance lyric parsing by collecting multiple timestamps and associating words with their respective start times
* e2953fa68940 2026-11-14  Refactor offset parsing and remove unused timestamp handling in EnhancedLrcParser
* ad8af02140f8 2026-11-14  Fix variable naming for clarity and maintain consistency in EnhancedLrcParser
* d875a2983c05 2026-11-14  Refactor EnhancedLrcParser to improve timestamp handling and lyric line parsing
*   42eca5185ca3 2026-11-14  Merge branch 'main' of github.com:giingerr01/mb_modern_lyrics
|\
* | 520b5b7b7e50 2026-11-14  Update plugin name and description for enhanced lyrics functionality
* | 55163190c133 2026-11-14  Remove trailing newline at the end of EnhancedLrcParser.cs
* | 43bc95b52eff 2026-11-14  Fix assembly name in project file to match enhanced lyrics functionality
| * 41b779f84cae 2026-11-01  Update README to describe the project and build process for the MusicBee lyrics plugin
|/
* 9162017a7451 2026-10-30  Enhance EnhancedLrcParser to support word timing and full text generation for lyrics
* 823ba8c7096d 2026-10-30  Enhance EnhancedLrcParser to support offset parsing and improve timestamp handling
* 070e501abba5 2026-10-30  Fix formatting settings in settings.json for C# extension preference
* 6f9715e0e6ad 2026-10-29  Fix casing of 'System' in LrcModels.cs
* 25fab30ef331 2026-10-29  Fix casing and typo in EnhancedLrcParser.cs
* 57b6b8d7647c 2026-10-29  Add LrcModels.cs and EnhancedLrcParser.cs to project file for compilation
* 5d0c1d04000b 2026-10-29  Add comment to clarify metadata tag checking in EnhancedLrcParser
* 8ab6afb7646c 2026-10-29  Add EnhancedLrcParser class to parse LRC formatted lyrics
* c4d5564e81de 2026-10-29  Add LrcModels.cs to define lyric data structures
* ed78b068416b 2026-10-29  Add lyrics retrieval functionality and clean up commented code
* 757e5557f580 2026-10-29  Add MIT License to the project
* b4660210058f 2026-10-29  Intial files
* 52059b512e71 2026-10-29  first commit
```
