using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using System.Diagnostics;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        private MusicBeeApiInterface mbApiInterface;
        private PluginInfo about = new PluginInfo();

        private LyricsViewControl _lyricsViewControl;
        private Timer _positiontimer;


        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            mbApiInterface = new MusicBeeApiInterface();
            mbApiInterface.Initialise(apiInterfacePtr);
            about.PluginInfoVersion = PluginInfoVersion;
            about.Name = "MB Enhanced Lyrics Panel";
            about.Description = "A MusicBee plugin word by word lyrics parser.";
            about.Author = "giingerr01";
            about.TargetApplication = "Enhanced Lyrics";
            about.Type = PluginType.General;
            about.VersionMajor = 1;
            about.VersionMinor = 0;
            about.Revision = 1;
            about.MinInterfaceVersion = MinInterfaceVersion;
            about.MinApiRevision = MinApiRevision;
            about.ReceiveNotifications = (ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.TagEvents);
            about.ConfigurationPanelHeight = 0;   // height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function

            _lyricsViewControl = new LyricsViewControl();
            _positiontimer = new Timer();
            _positiontimer.Interval = 30; // Update every 100 ms
            _positiontimer.Tick += PostionTimer_Tick;
            return about;
        }

        public Control GetDisplayCanvas()
        {
            return _lyricsViewControl;
        }

        public bool Configure(IntPtr panelHandle)
        {
            string dataPath = mbApiInterface.Setting_GetPersistentStoragePath();
            if (panelHandle != IntPtr.Zero)
            {
                Panel configPanel = (Panel)Control.FromHandle(panelHandle);
                Label prompt = new Label();
                prompt.AutoSize = true;
                prompt.Location = new Point(0, 0);
                prompt.Text = "Enhanced Lyrics Plugin config";
                configPanel.Controls.Add(prompt);
            }
            return false;
        }

        // called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
        // its up to you to figure out whether anything has changed and needs updating
        public void SaveSettings()
        {
            // save any persistent settings in a sub-folder of this path
            string dataPath = mbApiInterface.Setting_GetPersistentStoragePath();
        }

        // MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
        public void Close(PluginCloseReason reason)
        {
            _positiontimer.Stop();
            _positiontimer.Dispose();
            _lyricsViewControl?.Dispose();
        }

        // uninstall this plugin - clean up any persisted files
        public void Uninstall()
        {
        }

        // receive event notifications from MusicBee
        // you need to set about.ReceiveNotificationFlags = PlayerEvents to receive all notifications, and not just the startup event
        public void ReceiveNotification(string sourceFileUrl, NotificationType type)
        {
            // perform some action depending on the notification type
            switch (type)
            {
                case NotificationType.PluginStartup:
                case NotificationType.TrackChanged:
                case NotificationType.NowPlayingListChanged:
                    LoadCurrentTrackLyrics();

                    if (mbApiInterface.Player_GetPlayState() == PlayState.Playing)
                    {
                        _positiontimer.Start();
                    }
                    else
                    {
                        _positiontimer.Stop();
                    }
                    break;

                case NotificationType.PlayStateChanged:
                    PlayState state = mbApiInterface.Player_GetPlayState();
                    if (state == PlayState.Playing)
                    {
                        _positiontimer.Start();
                    }
                    else
                    {
                        _positiontimer.Stop();
                    }
                    break;
            }
        }

        private void LoadCurrentTrackLyrics()
        {
            string rawlyrics = mbApiInterface.NowPlaying_GetFileTag(MetaDataType.Lyrics);
            if (!string.IsNullOrEmpty(rawlyrics))
            {
                _lyricsViewControl.SetLyrics(new ParsedLyrics());
                return;
            }
            ParsedLyrics parsed = EnhancedLrcParser.Parse(rawlyrics);
            _lyricsViewControl.SetLyrics(parsed);

        }

        public List<ToolStripItem> GetHeaderMenuItems()
        {
            List<ToolStripItem> list = new List<ToolStripItem>();
            list.Add(new ToolStripMenuItem("Reload Lyrics", null, (s, e) =>
                LoadCurrentTrackLyrics()
            ));
            return list;
        }

        public int OnDockablePanelCreated(Control panel)
        {
            panel.Controls.Add(_lyricsViewControl);
            _lyricsViewControl.Dock = DockStyle.Fill;
            return 0;
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Red);
            TextRenderer.DrawText(e.Graphics, "hello", SystemFonts.CaptionFont, new Point(10, 10), Color.Blue);
        }

        private void PostionTimer_Tick(object sender, EventArgs e)
        {
            if (mbApiInterface.Player_GetPlayState() == PlayState.Playing)
            {
                int positionMs = mbApiInterface.Player_GetPosition();
                TimeSpan currentPosition = TimeSpan.FromMilliseconds(positionMs);
                _lyricsViewControl.UpdatePosition(currentPosition);

            }
        }
    }
}