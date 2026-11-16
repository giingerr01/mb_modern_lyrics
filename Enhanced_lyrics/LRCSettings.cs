using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicBeePlugin.Enhanced_lyrics
{
    public partial class LRCSettings : UserControl
    {
        private String _font = "Lucida Sans";
        private int _fontSize = 10;
        public event Action <string, int> FontSelectionChanged;

        public LRCSettings(string font, int fontSize)
        {
            InitializeComponent();
            _font = font;
            _fontSize = fontSize;

            fontSizeBox.Text = $"{_font}, {_fontSize}pt";
        }

        private void LRCSettings_Load(object sender, EventArgs e)
        {

        }

        private void LyricsLabel_Click(object sender, EventArgs e)
        {

        }

       private void FontSelctor_Click(object sender, EventArgs e)
        {
            using (FontDialog fontDialog = new FontDialog())
            {
                fontDialog.Font = new Font(_font, _fontSize);

                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    _font = fontDialog.Font.Name;
                    _fontSize = (int)fontDialog.Font.Size;

                    fontSizeBox.Text = $"{_font}, {_fontSize}pt";
                    FontSelectionChanged?.Invoke(_font, _fontSize);
                }
            }

        }
 
    }
}
