using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using MusicBeePlugin.Models;
using MusicBeePlugin.UI;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        private MusicBeeApiInterface mbApiInterface;
        private PluginInfo about = new PluginInfo();
        private LyricsWindow lyricsWindow;

        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            mbApiInterface = new MusicBeeApiInterface();
            mbApiInterface.Initialise(apiInterfacePtr);
            about.PluginInfoVersion = PluginInfoVersion;
            about.Name = "Modern Lyrics";    // name of the plugin
            about.Description = "A modern lyrics parser plugin for MusicBee";  // description of the plugin
            about.Author = "giingerr01";
            about.TargetApplication = "";   //  the name of a Plugin Storage device or panel header for a dockable panel
            about.Type = PluginType.PanelView;
            about.VersionMajor = 1;  // your plugin version
            about.VersionMinor = 0;
            about.Revision = 1;
            about.MinInterfaceVersion = MinInterfaceVersion;
            about.MinApiRevision = MinApiRevision;
            about.ReceiveNotifications = (ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.TagEvents);
            about.ConfigurationPanelHeight = 0;   // height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function
            return about;
        }

        public bool Configure(IntPtr panelHandle)
        {
            // save any persistent settings in a sub-folder of this path
            string dataPath = mbApiInterface.Setting_GetPersistentStoragePath();
            // panelHandle will only be set if you set about.ConfigurationPanelHeight to a non-zero value
            // keep in mind the panel width is scaled according to the font the user has selected
            // if about.ConfigurationPanelHeight is set to 0, you can display your own popup window
            if (panelHandle != IntPtr.Zero)
            {
                Panel configPanel = (Panel)Panel.FromHandle(panelHandle);
                Label prompt = new Label();
                prompt.AutoSize = true;
                prompt.Location = new Point(0, 0);
                prompt.Text = "prompt:";
                TextBox textBox = new TextBox();
                textBox.Bounds = new Rectangle(60, 0, 100, textBox.Height);
                configPanel.Controls.AddRange(new Control[] { prompt, textBox });
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
                    LoadCurrentLyrics();
                    break;
                case NotificationType.TrackChanged:
                case NotificationType.NowPlayingLyricsReady:
                    LoadCurrentLyrics();
                    break;
                case NotificationType.PlayStateChanged:
                    UpdatePlaybackPosition();
                    break;
            }
        }

        public int OnDockablePanelCreated(Control panel)
        {
            //    return the height of the panel and perform any initialisation here
            //    MusicBee will call panel.Dispose() when the user removes this panel from the layout configuration
            //    < 0 indicates to MusicBee this control is resizable and should be sized to fill the panel it is docked to in MusicBee
            //    = 0 indicates to MusicBee this control resizeable
            //    > 0 indicates to MusicBee the fixed height for the control.Note it is recommended you scale the height for high DPI screens(create a graphics object and get the DpiY value)
            float dpiScaling = 0;
            using (Graphics g = panel.CreateGraphics())
            {
                dpiScaling = g.DpiY / 96f;
            }
            lyricsWindow = new LyricsWindow();
            lyricsWindow.Dock = DockStyle.Fill;
            lyricsWindow.SetPlaybackPositionProvider(delegate
            {
                return TimeSpan.FromMilliseconds(Math.Max(0, mbApiInterface.Player_GetPosition()));
            });
            panel.Controls.Add(lyricsWindow);
            LoadCurrentLyrics();
            return -1;
        }

        //  presence of this function indicates to MusicBee that the dockable panel created above will show menu items when the panel header is clicked
        // return the list of ToolStripMenuItems that will be displayed
        public List<ToolStripItem> GetHeaderMenuItems()
        {
            List<ToolStripItem> list = new List<ToolStripItem>();
            list.Add(new ToolStripMenuItem("A menu item"));
            return list;
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
        }

        private void LoadCurrentLyrics()
        {
            if (lyricsWindow == null)
            {
                return;
            }

            string rawLyrics = mbApiInterface.NowPlaying_GetLyrics();
            int durationSeconds = mbApiInterface.NowPlaying_GetDuration();
            TimeSpan? duration = durationSeconds > 0
                ? (TimeSpan?)TimeSpan.FromSeconds(durationSeconds)
                : null;

            lyricsWindow.SetLyrics(rawLyrics, duration);
            UpdatePlaybackPosition();
        }

        private void UpdatePlaybackPosition()
        {
            if (lyricsWindow != null)
            {
                lyricsWindow.UpdatePlaybackPosition(
                    TimeSpan.FromMilliseconds(Math.Max(0, mbApiInterface.Player_GetPosition())));
            }
        }
    }
}