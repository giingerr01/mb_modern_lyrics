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
        private string _font = "Lucida Sans";
        private int _fontSize = 10;
        private FontStyle _fontStyle = FontStyle.Regular;
        private Color _activeColor = Color.Black;
        private Color _defaultCheckBoxForeColor;
        private bool _initializing;
        public event Action<string, int, FontStyle> FontSelectionChanged;
        public event Action<Color, bool> ActiveColorChanged;
        public event Action<StringAlignment> LyricsAlignmentChanged;
        public event Action<bool> AnimationEnabledChanged;


        public LRCSettings(
            string font,
            int fontSize,
            Color bgColor,
            FontStyle fontStyle,
            Color activeColor,
            bool useCustomColor,
            StringAlignment lyricsAlignment,
            bool animationEnabled
            )
        {
            InitializeComponent();
            _defaultCheckBoxForeColor = CustomColorCheckBox.ForeColor;
            this.BackColor = bgColor;
            LyricsLabel.BackColor = Color.Transparent;
            fontSizeBox.BackColor = bgColor;
            CustomColorCheckBox.UseVisualStyleBackColor = false;
            CustomColorCheckBox.BackColor = bgColor;
            _initializing = true;
            _font = font;
            _fontSize = fontSize;
            _fontStyle = fontStyle;
            _activeColor = activeColor;

            PreviewFontStyle();

            alignmentComboBox.SelectedIndex = GetAlignmentIndex(lyricsAlignment);
            CustomColorCheckBox.Checked = useCustomColor;
            CustomColorCheckBox.ForeColor = useCustomColor
                ? activeColor
                : _defaultCheckBoxForeColor;
            CheckAnimBox.Checked = animationEnabled;
            _initializing = false;
        }

        private void PreviewFontStyle()
        {
            fontSizeBox.Text = $"{_font}, {_fontStyle}, {_fontSize}pt";

        }

        private static int GetAlignmentIndex(StringAlignment alignment)
        {
            switch (alignment)
            {
                case StringAlignment.Near:
                    return 1;
                case StringAlignment.Far:
                    return 2;
                default:
                    return 0;
            }
        }

        private void AlignmentComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initializing)
            {
                return;
            }

            StringAlignment alignment;
            switch (alignmentComboBox.SelectedIndex)
            {
                case 1:
                    alignment = StringAlignment.Near;
                    break;
                case 2:
                    alignment = StringAlignment.Far;
                    break;
                default:
                    alignment = StringAlignment.Center;
                    break;
            }

            LyricsAlignmentChanged?.Invoke(alignment);
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
            using (Font currentFont = new Font(_font, _fontSize, _fontStyle))
            {
                fontDialog.Font = currentFont;

                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    using (Font selectedFont = fontDialog.Font)
                    {
                        _font = selectedFont.Name;
                        _fontSize = (int)selectedFont.Size;
                        _fontStyle = selectedFont.Style;
                    }

                    PreviewFontStyle();
                    FontSelectionChanged?.Invoke(_font, _fontSize, _fontStyle);
                }
            }

        }

        private void FontSizeBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void CustomColorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_initializing)
            {
                return;
            }

            if (CustomColorCheckBox.Checked)
            {
                using (ColorDialog colorDialog = new ColorDialog())
                {
                    colorDialog.Color = _activeColor;
                    colorDialog.FullOpen = true;

                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        _activeColor = colorDialog.Color;
                        CustomColorCheckBox.ForeColor = _activeColor;
                        ActiveColorChanged?.Invoke(_activeColor, true);
                    }
                    else
                    {
                        _initializing = true;
                        CustomColorCheckBox.Checked = false;
                        CustomColorCheckBox.ForeColor = _defaultCheckBoxForeColor;
                        _initializing = false;
                    }
                }
            }
            else
            {
                CustomColorCheckBox.ForeColor = _defaultCheckBoxForeColor;
                ActiveColorChanged?.Invoke(_activeColor, false);
            }

        }

        private void CheckAnimBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_initializing)
            {
                return;
            }

            AnimationEnabledChanged?.Invoke(CheckAnimBox.Checked);
        }
    }
}
