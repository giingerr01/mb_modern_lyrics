using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        private MusicBeeApiInterface mbApiInterface;
        private PluginInfo about = new PluginInfo();

        private LyricsViewControl _lyricsViewControl;
        private System.Timers.Timer _positionTimer;
        private ParsedLyrics _currentLyrics = new ParsedLyrics();

        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            mbApiInterface = new MusicBeeApiInterface();
            mbApiInterface.Initialise(apiInterfacePtr);
            about.PluginInfoVersion = 1;
            about.Name = "LRC Lyrics Panel";
            about.Description = "Line-by-line synchronized (LRC) lyrics panel.";
            about.Author = "giingerr01";
            about.TargetApplication = "";
            about.Type = PluginType.PanelView;
            about.VersionMajor = 1;
            about.VersionMinor = 0;
            about.Revision = 1;
            about.MinInterfaceVersion = MinInterfaceVersion;
            about.MinApiRevision = MinApiRevision;
            about.ReceiveNotifications = (ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.TagEvents);
            about.ConfigurationPanelHeight = 0;

            return about;
        }


        public bool Configure(IntPtr panelHandle)
        {
            if (panelHandle != IntPtr.Zero)
            {
                Panel configPanel = (Panel)Control.FromHandle(panelHandle);
                Label prompt = new Label();
                prompt.AutoSize = true;
                prompt.Location = new Point(0, 0);
                prompt.Text = "Coming soon: configuration options for the LRC Lyrics Panel plugin.";
                configPanel.Controls.Add(prompt);
            }
            return false;
        }

        public void SaveSettings()
        {
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

        public void Uninstall()
        {
        }

        // presence of this method tells MusicBee the plugin has a dockable panel
        public int OnDockablePanelCreated(Control panel)
        {
            // created here because this method runs on the UI thread; a WinForms Timer
            // created on another thread never ticks
            if (_positionTimer == null)
            {
                _positionTimer = new System.Timers.Timer();
                _positionTimer.AutoReset = true;
                _positionTimer.Elapsed += PositionTimer_Elapsed;
            }

            _lyricsViewControl = new LyricsViewControl { Dock = DockStyle.Fill };
            _lyricsViewControl.SetLyrics(_currentLyrics);
            panel.Controls.Add(_lyricsViewControl);

            UpdateTimerState();
            SyncPosition();
            return 0;   // < 0: resizable, fills the space the panel is docked to
        }

        private void RunOnUiThreadYouMf(Action action)
        {
            if (_lyricsViewControl != null && _lyricsViewControl.IsHandleCreated && _lyricsViewControl.InvokeRequired)
            {
                _lyricsViewControl.BeginInvoke(action);
            }
            else
            {
                action();
            }
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
                rawLyrics = mbApiInterface.NowPlaying_GetFileTag(MetaDataType.Lyrics);
            }
            if (string.IsNullOrWhiteSpace(rawLyrics))
            {
                rawLyrics = ReadSidecarLrc();
            }

            _currentLyrics = LrcParser.Parse(rawLyrics);
            _lyricsViewControl?.SetLyrics(_currentLyrics);
            SyncPosition();
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
            catch (Exception)
            {
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
            SyncPosition();
        }

        public List<ToolStripItem> GetHeaderMenuItems()
        {
            List<ToolStripItem> list = new List<ToolStripItem>();
            list.Add(new ToolStripMenuItem("Reload Lyrics", null, (s, e) => LoadCurrentTrackLyrics()));
            return list;
        }
    }
}