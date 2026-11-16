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


        private MusicBeeApiInterface mbApiInterface;
        private PluginInfo about = new PluginInfo();

        private LyricsViewControl _lyricsViewControl;
        private JObject Settings { get; set; } = new JObject();
        private Control _dockablePanel;
        private LRCSettings lrcSettings;
        private System.Timers.Timer _positionTimer;
        private ParsedLyrics _currentLyrics = new ParsedLyrics();

        private int _fontSize = 10;
        private string _font = "Lucida Sans";
        private Color _activeColor = Color.Black;
        private Color _bgColor = Color.White;

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

            Logger.FilePath = Path.Combine(LRCSettingsFolder, "LrcSettingsLog.log");


            JObject lyricsSettings = settingsJson["Lyrics-Settings"] as JObject
            ?? new JObject();
            Settings = lyricsSettings;
            LoadSettings();
            about.PluginInfoVersion = 1;
            about.Name = "LRC Lyrics";
            about.Description = "Line-by-line synchronized (LRC) lyrics panel.";
            about.Author = "giingerr01";
            about.TargetApplication = "LRC Lyrics";
            about.Type = PluginType.PanelView;
            about.VersionMajor = 1;
            about.VersionMinor = 1;
            about.Revision = 1;
            about.MinInterfaceVersion = MinInterfaceVersion;
            about.MinApiRevision = MinApiRevision;
            about.ReceiveNotifications = (ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.TagEvents);
            about.ConfigurationPanelHeight = 60;

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
            lrcSettings = new LRCSettings(_font, _fontSize, _bgColor)
            {
                Dock = DockStyle.Fill
            };

            System.Diagnostics.Debug.WriteLine(
                $"LRCSettings null: {lrcSettings == null}");


            lrcSettings.FontSelectionChanged += (font, size) =>
            {
                _font = font;
                _fontSize = size;

                SaveSettings();

                RunOnUiThreadYouMf(() =>
                {
                    _lyricsViewControl?.SetFontSize(_fontSize, _font);
                });
            };

            lrcSettings.ActiveColorChanged += color =>
            {
                _activeColor = color;
                SaveSettings();

                RunOnUiThreadYouMf(() =>
                {
                    _lyricsViewControl?.SetActiveColor(color);
                    SaveSettings();
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

            //these three lines has caused me so much problem
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
            _lyricsViewControl?.Dispose();

            _lyricsViewControl = new LyricsViewControl { Dock = DockStyle.Fill };

            Color bgColor = GetSkinPanelBackgroundColor();

            Color fgColor = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(
                SkinElement.SkinInputPanel, ElementState.ElementStateDefault,
                ElementComponent.ComponentForeground
            ));

            // _lyricsViewControl.SetBackgroundColor(bgColor);
            _lyricsViewControl.BackColor = bgColor;

            _lyricsViewControl.SetFontSize(_fontSize, _font);

            _lyricsViewControl.SetActiveColor(_activeColor);

            _lyricsViewControl.SetLyrics(_currentLyrics);

            panel.Controls.Add(_lyricsViewControl);

            //makeing  sure that the lyrics are loaded even if plugin startup fired bfore panel creating
            if (_currentLyrics == null || _currentLyrics.Lines.Count == 0)
            {
                LoadCurrentTrackLyrics();
            }
            else
            {
                _lyricsViewControl?.SetLyrics(_currentLyrics);
            }

            UpdateTimerState();
            SyncPosition();
            return 0;   // < 0: resizable, fills the space the panel is docked to
        }

        private Color GetSkinPanelBackgroundColor()
        {
            return Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(
                SkinElement.SkinInputPanel,
                ElementState.ElementStateDefault,
                ElementComponent.ComponentBackground));
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
                    Logger.Log("Reading LRC file failed: " + ex.Message);
                }
            }

            _currentLyrics = LrcParser.Parse(rawLyrics);
            if (_lyricsViewControl != null && !_lyricsViewControl.IsDisposed)
            {
                _lyricsViewControl.SetLyrics(_currentLyrics);
                SyncPosition();
                return;
            }
            // _lyricsViewControl?.SetLyrics(_currentLyrics);
        }

        // song.flac -> song.lrc next to it
        private string ReadSidecarLrc()
        {
            try
            {
                string url = mbApiInterface.NowPlaying_GetFileUrl();
                if (string.IsNullOrEmpty(url))
                {
                    return null;
                }

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
            if (_lyricsViewControl == null)
            {
                return;
            }
            int positionMs = mbApiInterface.Player_GetPosition();
            _lyricsViewControl.UpdatePosition(TimeSpan.FromMilliseconds(positionMs));
        }

        private void PositionTimer_Elapsed(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("<PositionTimer_Elapsed()>: thread " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            var uiControl = _dockablePanel ?? (Control)_lyricsViewControl;
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
            _lyricsViewControl?.Dispose();
        }

        private void RunOnUiThreadYouMf(Action action)
        {
            Control uiControl = _dockablePanel ?? (Control)_lyricsViewControl;
            if (uiControl != null && uiControl.InvokeRequired)
            {
                uiControl.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        public List<ToolStripItem> GetHeaderMenuItems()
        {
            List<ToolStripItem> list = new List<ToolStripItem>();
            list.Add(new ToolStripMenuItem("Reload Lyrics", null, (s, e) => LoadCurrentTrackLyrics()));
            return list;
        }
        public void Uninstall()
        {
        }

        private void LoadSettings()
        {
            _fontSize = Settings.Value<int?>("FontSize") ?? 10;
            _font = Settings.Value<string>("Font") ?? "Lucida sans";

            string savedColor = Settings.Value<string>("ActiveColor");
            if (!string.IsNullOrEmpty(savedColor))
            {
                try
                {
                    _activeColor = ColorTranslator.FromHtml(savedColor);
                    Logger.Log("Color Successfully got!!");
                }
                catch (Exception ex)
                {
                    Logger.Log("Error Loading active color: " + ex.Message);
                    _activeColor = Color.Black;
                }
            }

        }
    }
}
