using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicBeePlugin.Enhanced_lyrics.LineByLineLyrics.Settings
{
    public partial class SettingsPanel : UserControl
    {
        private int _fontSize;
        private readonly Action _saveSettings;

        public SettingsPanel(
            int fontSize,
            Action saveSettings
        )
        {
            InitializeComponent();
            _fontSize = fontSize;
            _saveSettings = saveSettings ?? throw new ArgumentNullException(nameof(saveSettings));
            fontSizeBox.Text = fontSize.ToString();
        }

        private void FontSizeBox_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(fontSizeBox.Text, out int newFontSize))
            {
                _fontSize = newFontSize;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            _saveSettings();
        }
    }
}
