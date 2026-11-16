using System.Windows.Forms;

namespace MusicBeePlugin.Enhanced_lyrics
{
    public partial class SettingsPanel : UserControl
    {
        public const int MinimumFontSize = 8;
        public const int MaximumFontSize = 72;

        public SettingsPanel(int fontSize)
        {
            InitializeComponent();
            FontSizeInput.Value = fontSize;
        }

        public int FontSize => decimal.ToInt32(FontSizeInput.Value);
    }
}
