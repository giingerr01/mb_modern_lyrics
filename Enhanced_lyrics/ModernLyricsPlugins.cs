//ModernLyricsPlugin.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Globalization;
using System.Windows.Forms;
using MusicBeePlugin.Enhanced_lyrics;
using System.Linq.Expressions;

namespace MusicBeePlugin
{
    public partial class Plugin
    {


        private int _fontSize = 10;
        private Control _dockablePanel;
        private LRCSettings lrcSettings;
        private string _font = "Lucida Sans";
        private Color _bgColor = Color.White;
        private bool _useCustomColor = false;
        private Color _activeColor = Color.Black;
        private Color _inactiveColor = Color.Gray;
        private System.Timers.Timer _positionTimer;
        private PluginInfo about = new PluginInfo();
        private MusicBeeApiInterface mbApiInterface;
        private SweepLyricsViewControl _sweepLyricsViewControl;
        private FontStyle _fontStyle = FontStyle.Regular;
        private bool _enableSweepAnimation = true;
        private JObject Settings { get; set; } = new JObject();
        private ParsedLyrics _currentLyrics = new ParsedLyrics();
        private StringAlignment _lyricsAlignment = StringAlignment.Center;

        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            Logger.Log("Plugin Intialized!");
            System.Diagnostics.Debug.WriteLine("<Inialize()>: thread " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            mbApiInterface = new MusicBeeApiInterface();
            mbApiInterface.Initialise(apiInterfacePtr);

            // storage
            string settingsPath = mbApiInterface.Setting_GetPersistentStoragePath();
            string LRCSettingsFolder = Path.Combine(settingsPath, "LrcSettings");
            Directory.CreateDirectory(LRCSettingsFolder);

            string jsonFilePath = Path.Combine(LRCSettingsFolder, "LrcSettings.json");
            JObject settingsJson = new JObject();
            if (File.Exists(jsonFilePath))
            {
                try
                {
                    settingsJson = JObject.Parse(File.ReadAllText(jsonFilePath));
                }
                catch (Exception ex)
                {
                    settingsJson = new JObject();
                    Logger.Log("Error while reading: " + ex.Message);
                }
            }
            //assining the path for the logge as the same folder for the settings folder
            Logger.FilePath = Path.Combine(LRCSettingsFolder, "LrcSettingsLog.log");

            JObject lyricsSettings = settingsJson["Lyrics-Settings"] as JObject
            ?? new JObject();
            Settings = lyricsSettings;
            LoadSettings();
            about.PluginInfoVersion = 1;
            about.Name = "ELRC Lyrics";
            about.Description = "Line-by-line synchronized (ELRC) lyrics panel.";
            about.Author = "giingerr01";
            about.TargetApplication = "ELRC Lyrics";
            about.Type = PluginType.PanelView;
            about.VersionMajor = 1;
            about.VersionMinor = 1;
            about.Revision = 1;
            about.MinInterfaceVersion = MinInterfaceVersion;
            about.MinApiRevision = MinApiRevision;
            about.ReceiveNotifications = (ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.TagEvents);
            about.ConfigurationPanelHeight = 120;
            return about;
        }

        public void SaveSettings()
        {
            // Settings["FontSize"] = _fontSize;
            // Settings["Font"] = _font;

            string settingsPath = mbApiInterface.Setting_GetPersistentStoragePath();
            string LRCSettingsFolder = Path.Combine(settingsPath, "LrcSettings");
            Directory.CreateDirectory(LRCSettingsFolder);
            string jsonFilePath = Path.Combine(LRCSettingsFolder, "LrcSettings.json");
            JObject settingsJson = new JObject();

            if (File.Exists(jsonFilePath))
            {
                try
                {
                    settingsJson = JObject.Parse(File.ReadAllText(jsonFilePath));
                }
                catch (Exception ex)
                {
                    Logger.Log("Error reading settings: " + ex.Message);

                }
            }
            JObject lyricsSettings = settingsJson["Lyrics-Settings"] as JObject ?? new JObject();

            // property to be saved
            lyricsSettings["FontSize"] = _fontSize;
            lyricsSettings["Font"] = _font;
            lyricsSettings["ActiveColor"] = ColorTranslator.ToHtml(_activeColor);
            lyricsSettings["FontStyle"] = _fontStyle.ToString();
            lyricsSettings["UseCustomColor"] = _useCustomColor;
            lyricsSettings["LyricsAlignment"] = _lyricsAlignment.ToString();
            lyricsSettings["EnableSweepAnimation"] = _enableSweepAnimation;

            //attach the root
            settingsJson["Lyrics-Settings"] = lyricsSettings;

            File.WriteAllText(jsonFilePath, settingsJson.ToString(Formatting.Indented));

            //log success
            Logger.Log("Settings Saved!! IN Settings FIle");
        }
        public bool Configure(IntPtr panelHandle)
        {
            if (panelHandle == IntPtr.Zero)
            {
                return false;
            }

            Control configPanel = Control.FromHandle(panelHandle);
            if (configPanel == null)
            {
                return false;
            }

            configPanel.Controls.Clear();

            _bgColor = GetSkinPanelBackgroundColor();
            lrcSettings = new LRCSettings(
                _font, _fontSize, _bgColor, _fontStyle, _activeColor, _useCustomColor,
                _lyricsAlignment, _enableSweepAnimation)
            {
                Dock = DockStyle.Fill
            };

            System.Diagnostics.Debug.WriteLine(
                $"LRCSettings null: {lrcSettings == null}");


            lrcSettings.FontSelectionChanged += (font, size, fontStyle) =>
            {
                _font = font;
                _fontSize = size;
                _fontStyle = fontStyle;

                SaveSettings();

                RunOnUiThreadYouMf(() =>
                {
                    _sweepLyricsViewControl?.SetFontSize(_fontSize, _font, _fontStyle);
                });
            };

            lrcSettings.ActiveColorChanged += (color, useCustomColor) =>
            {
                _activeColor = color;
                _useCustomColor = useCustomColor;
                SaveSettings();

                RunOnUiThreadYouMf(() =>
                {
                    _sweepLyricsViewControl?.SetActiveColor(
                        _useCustomColor ? _activeColor : Color.Black);
                });
            };

            lrcSettings.LyricsAlignmentChanged += alignment =>
            {
                _lyricsAlignment = alignment;
                SaveSettings();

                RunOnUiThreadYouMf(() =>
                {
                    _sweepLyricsViewControl?.SetLyricsAlignment(_lyricsAlignment);
                });
            };

            lrcSettings.SweepAnimationChanged += enabled =>
            {
                _enableSweepAnimation = enabled;
                SaveSettings();

                RunOnUiThreadYouMf(() =>
                {
                    _sweepLyricsViewControl?.SetSweepAnimationEnabled(_enableSweepAnimation);
                });
            };

            configPanel.Controls.Add(lrcSettings);

            return false;
        }

        // presence of this method tells MusicBee the plugin has a dockable panel
        public int OnDockablePanelCreated(Control panel)
        {
            if (panel == null)
            {
                return 0;
            }

            // Non-presence of these three lines has caused me so much Pain;
            try
            {
                if (panel.InvokeRequired)
                {
                    return (int)panel.Invoke(new Func<int>(() => OnDockablePanelCreated(panel)));
                }
            }
            catch (Exception ex)
            {
                Logger.Log("OnDockablePanel creation Failed: " + ex.Message);
            }

            System.Diagnostics.Debug.WriteLine("<OnDocablePanelCreated()>: thread " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            _dockablePanel = panel;
            if (_positionTimer == null)
            {
                _positionTimer = new System.Timers.Timer();
                _positionTimer.AutoReset = true;
                _positionTimer.Elapsed += PositionTimer_Elapsed;
            }
            _sweepLyricsViewControl?.Dispose();
            _sweepLyricsViewControl = new SweepLyricsViewControl { Dock = DockStyle.Fill };

            Color bgColor = GetSkinPanelBackgroundColor();

            Color fgColor = GetSkinForeColor();

            _sweepLyricsViewControl.BackColor = bgColor;
            _sweepLyricsViewControl.SetFontSize(_fontSize, _font, _fontStyle);
            _sweepLyricsViewControl.SetLyricsAlignment(_lyricsAlignment);
            _sweepLyricsViewControl.SetActiveColor(_useCustomColor ? _activeColor : Color.Black);
            _sweepLyricsViewControl.SetInActiveColor(fgColor);
            _sweepLyricsViewControl.SetSweepAnimationEnabled(_enableSweepAnimation);
            _sweepLyricsViewControl.SetLyrics(_currentLyrics);
            panel.Controls.Add(_sweepLyricsViewControl);

            //makeing  sure that the lyrics are loaded even if plugin startup fired bfore panel creating
            if (_currentLyrics == null || _currentLyrics.Lines.Count == 0)
            {
                LoadCurrentTrackLyrics();
            }
            else
            {
                _sweepLyricsViewControl?.SetLyrics(_currentLyrics);
            }

            UpdateTimerState();
            SyncPosition();
            return 0;   // < 0: resizable, fills the space the panel is docked to
        }

        //get skin fore  color for the texts 
        private Color GetSkinPanelBackgroundColor()
        {
            return Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(
                SkinElement.SkinInputPanel,
                ElementState.ElementStateDefault,
                ElementComponent.ComponentBackground));
        }
        private Color GetSkinForeColor()
        {
            return Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(
                SkinElement.SkinElementPanel,
                ElementState.ElementStateDefault,
                ElementComponent.ComponentForeground
            ));
        }

        public void ReceiveNotification(string sourceFileUrl, NotificationType type)
        {
            switch (type)
            {
                case NotificationType.PluginStartup:
                case NotificationType.TrackChanged:
                case NotificationType.PlayingTracksChanged:
                case NotificationType.NowPlayingLyricsReady:
                    RunOnUiThreadYouMf(() =>
                        {
                            LoadCurrentTrackLyrics();
                            UpdateTimerState();
                        });
                    break;

                case NotificationType.PlayStateChanged:
                    RunOnUiThreadYouMf(() =>
                    {
                        UpdateTimerState();
                        SyncPosition();
                    });
                    break;
            }
        }

        private void LoadCurrentTrackLyrics()
        {
            string rawLyrics = mbApiInterface.NowPlaying_GetLyrics();
            if (string.IsNullOrWhiteSpace(rawLyrics))
            {
                try
                {
                    rawLyrics = mbApiInterface.NowPlaying_GetFileTag(MetaDataType.Lyrics);
                }
                catch (Exception ex)
                {
                    Logger.Log("Error Getting Lyrics from Trackfile: " + ex.Message);
                }
            }
            if (string.IsNullOrWhiteSpace(rawLyrics))
            {
                try
                {
                    rawLyrics = ReadSidecarLrc();
                }
                catch (Exception ex)
                {
                    Logger.Log("Reading Embeded Lyrics failed: " + ex.Message);
                }
            }

            _currentLyrics = LrcParser.Parse(rawLyrics);
            if (_sweepLyricsViewControl != null && !_sweepLyricsViewControl.IsDisposed)
            {
                _sweepLyricsViewControl.SetLyrics(_currentLyrics);
                SyncPosition();
                return;
            }
        }

        // song.flac -> song.lrc  get the file path of the song from api and change the songs title's extension to to .lrc that is next to it next to it
        private string ReadSidecarLrc()
        {
            try
            {
                string url = mbApiInterface.NowPlaying_GetFileUrl();
                if (string.IsNullOrEmpty(url))
                {
                    return null;
                }

                //chaingign extension
                string lrcPath = Path.ChangeExtension(url, ".lrc");
                return File.Exists(lrcPath) ? File.ReadAllText(lrcPath) : null;
            }
            catch (Exception ex)
            {
                Logger.Log("Reading LRC file Failed: " + ex.Message);
                return null;
            }
        }

        private void UpdateTimerState()
        {
            if (_positionTimer == null)
            {
                return;
            }

            if (mbApiInterface.Player_GetPlayState() == PlayState.Playing)
            {
                _positionTimer.Start();
            }
            else
            {
                _positionTimer.Stop();
            }
        }

        private void SyncPosition()
        {
            if (_sweepLyricsViewControl == null || _sweepLyricsViewControl.IsDisposed)
            {
                return;
            }
            int positionMs = mbApiInterface.Player_GetPosition();
            _sweepLyricsViewControl.UpdatePosition(TimeSpan.FromMilliseconds(positionMs));
        }

        private void PositionTimer_Elapsed(object sender, EventArgs e)
        {
            // System.Diagnostics.Debug.WriteLine("<PositionTimer_Elapsed()>: thread " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            var uiControl = _dockablePanel ?? (Control)_sweepLyricsViewControl;
            if (uiControl == null || uiControl.IsDisposed || !uiControl.IsHandleCreated)
            {
                return;
            }
            if (uiControl.InvokeRequired)
            {
                uiControl.BeginInvoke((Action)SyncPosition);
            }
            else
            {
                SyncPosition();
            }
        }
        public void Close(PluginCloseReason reason)
        {
            if (_positionTimer != null)
            {
                _positionTimer.Stop();
                _positionTimer.Dispose();
            }
            _sweepLyricsViewControl?.Dispose();
            _sweepLyricsViewControl = null;
        }

        private void RunOnUiThreadYouMf(Action action)
        {
            Control uiControl = _dockablePanel ?? (Control)_sweepLyricsViewControl;
            if (uiControl != null && uiControl.InvokeRequired)
            {
                uiControl.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        // refresh handler for menu item in header
        private void RefreshLyricsPanel()
        {
            RunOnUiThreadYouMf(() =>
            {
                _sweepLyricsViewControl?.Invalidate();
                _sweepLyricsViewControl?.Refresh();

                LoadCurrentTrackLyrics();
                SyncPosition();
            });
        }

        // added header menu items for menu using GetMenuItems for this method the Method "GetMenuHeaderItmes()" doesnt work or it seems its been update to the one iam using
        // for my method dont change this method. 
        //                            |
        public List<ToolStripItem> GetMenuItems()
        {
            List<ToolStripItem> list = new List<ToolStripItem>();
            list.Add(new ToolStripMenuItem("Reload Lyrics", null, (s, e) => LoadCurrentTrackLyrics()));
            list.Add(new ToolStripSeparator());
            list.Add(new ToolStripMenuItem("Refresh panel", null, (s, e) => RefreshLyricsPanel()));

            return list;
        }
        public void Uninstall()
        {
        }

        private void LoadSettings()
        {
            //loaded settings
            _fontSize = Settings.Value<int?>("FontSize") ?? 10;
            _font = Settings.Value<string>("Font") ?? "Lucida sans";
            _useCustomColor = Settings.Value<bool?>("UseCustomColor") ?? false;
            _enableSweepAnimation = Settings.Value<bool?>("EnableSweepAnimation") ?? true;

            string savedAlignment = Settings.Value<string>("LyricsAlignment");
            if (Enum.TryParse(savedAlignment, true, out StringAlignment alignment) &&
                (alignment == StringAlignment.Near ||
                 alignment == StringAlignment.Center ||
                 alignment == StringAlignment.Far))
            {
                _lyricsAlignment = alignment;
            }

            string savedStyle = Settings.Value<string>("FontStyle") ?? "Regular";
            if (Enum.TryParse(savedStyle, out FontStyle style))
            {
                _fontStyle = style;
            }

            string savedColor = Settings.Value<string>("ActiveColor");
            if (!string.IsNullOrEmpty(savedColor))
            {
                try
                {
                    _activeColor = ColorTranslator.FromHtml(savedColor);
                }
                catch (Exception)
                {
                    _activeColor = Color.Black;
                }
            }

        }
    }
}
