using System;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class SettingsPanel : UserControl
    {
        private readonly Action _saveSettings;

        public int FontSize { get; private set; }

        public SettingsPanel(
            int fontSize,
            Action saveSettings
        )
        {
            InitializeComponent();
            FontSize = fontSize;
            _saveSettings = saveSettings ?? throw new ArgumentNullException(nameof(saveSettings));
            fontSizeBox.Text = FontSize.ToString();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(fontSizeBox.Text, out int fontSize) || fontSize < 1)
            {
                MessageBox.Show("Enter a font size greater than zero.");
                return;
            }

            FontSize = fontSize;
            _saveSettings();
        }
    }
}
