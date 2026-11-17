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

    // Color backGroundColor = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(
    // SkinElement.SkinInputPanel,
    //         ElementState.ElementStateDefault,
    //         ElementComponent.ComponentBackground));
    public partial class LRCSettings : UserControl
    {
        private String _font = "Lucida Sans";
        private int _fontSize = 10;
        private FontStyle _fontStyle = FontStyle.Regular;
        private Color _activeColor = Color.Black;
        public event Action<string, int, FontStyle> FontSelectionChanged;
        public event Action<Color> ActiveColorChanged;


        public LRCSettings(string font, int fontSize, Color bgColor, FontStyle fontStyle)
        {
            InitializeComponent();
            this.BackColor = bgColor;
            LyricsLabel.BackColor = Color.Transparent;
            fontSizeBox.BackColor = bgColor;
            CustomColorCheckBox.UseVisualStyleBackColor = false;
            CustomColorCheckBox.BackColor = bgColor;
            _font = font;
            _fontSize = fontSize;
            _fontStyle = fontStyle;
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
                    _fontStyle = fontDialog.Font.Style;

                    fontSizeBox.Text = $"{_font}, {_fontSize}pt";
                    FontSelectionChanged?.Invoke(_font, _fontSize, _fontStyle);
                }
            }

        }

        private void FontSizeBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void CustomColorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CustomColorCheckBox.Checked)
            {
                using (ColorDialog colorDialog = new ColorDialog())
                {
                    colorDialog.Color = _activeColor;
                    colorDialog.FullOpen = true;

                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        _activeColor = colorDialog.Color;
                        ActiveColorChanged?.Invoke(_activeColor);
                    }
                    else
                    {
                        CustomColorCheckBox.Checked = false;

                    }
                }
            }
            else
            {
                _activeColor = Color.Black;
                ActiveColorChanged?.Invoke(_activeColor);
            }

        }
    }
}
