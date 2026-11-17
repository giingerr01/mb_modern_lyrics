namespace MusicBeePlugin.Enhanced_lyrics
{
    partial class LRCSettings
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LyricsLabel = new System.Windows.Forms.Label();
            this.fontSizeBox = new System.Windows.Forms.TextBox();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.CustomColorCheckBox = new System.Windows.Forms.CheckBox();
            this.alignmentComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // LyricsLabel
            // 
            this.LyricsLabel.AutoSize = true;
            this.LyricsLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LyricsLabel.Location = new System.Drawing.Point(-3, 0);
            this.LyricsLabel.Name = "LyricsLabel";
            this.LyricsLabel.Size = new System.Drawing.Size(74, 17);
            this.LyricsLabel.TabIndex = 0;
            this.LyricsLabel.Text = "LRC Lyrics :";
            this.LyricsLabel.Click += new System.EventHandler(this.LyricsLabel_Click);
            // 
            // fontSizeBox
            // 
            this.fontSizeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fontSizeBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.fontSizeBox.Location = new System.Drawing.Point(82, 27);
            this.fontSizeBox.Name = "fontSizeBox";
            this.fontSizeBox.ReadOnly = true;
            this.fontSizeBox.Size = new System.Drawing.Size(178, 20);
            this.fontSizeBox.TabIndex = 1;
            this.fontSizeBox.Click += new System.EventHandler(this.FontSelctor_Click);
            // 
            // CustomColorCheckBox
            // 
            this.CustomColorCheckBox.AutoSize = true;
            this.CustomColorCheckBox.Font = new System.Drawing.Font("JetBrainsMono NFM", 8.249999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CustomColorCheckBox.Location = new System.Drawing.Point(82, 53);
            this.CustomColorCheckBox.Name = "CustomColorCheckBox";
            this.CustomColorCheckBox.Size = new System.Drawing.Size(257, 18);
            this.CustomColorCheckBox.TabIndex = 3;
            this.CustomColorCheckBox.Text = "custom color for highlighted text";
            this.CustomColorCheckBox.UseVisualStyleBackColor = true;
            this.CustomColorCheckBox.CheckedChanged += new System.EventHandler(this.CustomColorCheckBox_CheckedChanged);
            // 
            // alignmentComboBox
            // 
            this.alignmentComboBox.BackColor = System.Drawing.SystemColors.Control;
            this.alignmentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.alignmentComboBox.FormattingEnabled = true;
            this.alignmentComboBox.Items.AddRange(new object[] {
            "Defalut Text Alignment",
            "Left Text Alignment",
            "Right Text Alignment"});
            this.alignmentComboBox.Location = new System.Drawing.Point(82, 3);
            this.alignmentComboBox.Name = "alignmentComboBox";
            this.alignmentComboBox.Size = new System.Drawing.Size(178, 21);
            this.alignmentComboBox.TabIndex = 4;
            // 
            // LRCSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.alignmentComboBox);
            this.Controls.Add(this.CustomColorCheckBox);
            this.Controls.Add(this.fontSizeBox);
            this.Controls.Add(this.LyricsLabel);
            this.Name = "LRCSettings";
            this.Size = new System.Drawing.Size(518, 287);
            this.Load += new System.EventHandler(this.LRCSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LyricsLabel;
        private System.Windows.Forms.TextBox fontSizeBox;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.CheckBox CustomColorCheckBox;
        private System.Windows.Forms.ComboBox alignmentComboBox;
    }
}
