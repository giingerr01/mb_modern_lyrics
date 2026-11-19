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
            this.CheckAnimBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // LyricsLabel
            // 
            this.LyricsLabel.AutoSize = true;
            this.LyricsLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LyricsLabel.Location = new System.Drawing.Point(-2, 0);
            this.LyricsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LyricsLabel.Name = "LyricsLabel";
            this.LyricsLabel.Size = new System.Drawing.Size(81, 17);
            this.LyricsLabel.TabIndex = 0;
            this.LyricsLabel.Text = "ELRC Lyrics :";
            this.LyricsLabel.Click += new System.EventHandler(this.LyricsLabel_Click);
            // 
            // fontSizeBox
            // 
            this.fontSizeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fontSizeBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.fontSizeBox.Location = new System.Drawing.Point(82, 28);
            this.fontSizeBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.fontSizeBox.Name = "fontSizeBox";
            this.fontSizeBox.ReadOnly = true;
            this.fontSizeBox.Size = new System.Drawing.Size(178, 20);
            this.fontSizeBox.TabIndex = 1;
            this.fontSizeBox.Click += new System.EventHandler(this.FontSelctor_Click);
            // 
            // CustomColorCheckBox
            // 
            this.CustomColorCheckBox.AutoSize = true;
            this.CustomColorCheckBox.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CustomColorCheckBox.Location = new System.Drawing.Point(82, 51);
            this.CustomColorCheckBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CustomColorCheckBox.Name = "CustomColorCheckBox";
            this.CustomColorCheckBox.Size = new System.Drawing.Size(171, 20);
            this.CustomColorCheckBox.TabIndex = 3;
            this.CustomColorCheckBox.Text = "color for highlighted Lyrics";
            this.CustomColorCheckBox.UseVisualStyleBackColor = true;
            this.CustomColorCheckBox.CheckedChanged += new System.EventHandler(this.CustomColorCheckBox_CheckedChanged);
            // 
            // alignmentComboBox
            // 
            this.alignmentComboBox.BackColor = System.Drawing.SystemColors.Control;
            this.alignmentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.alignmentComboBox.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alignmentComboBox.FormattingEnabled = true;
            this.alignmentComboBox.Items.AddRange(new object[] {
            "Default Text Alignment",
            "Left Text Alignment",
            "Right Text Alignment"});
            this.alignmentComboBox.Location = new System.Drawing.Point(82, 3);
            this.alignmentComboBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.alignmentComboBox.Name = "alignmentComboBox";
            this.alignmentComboBox.Size = new System.Drawing.Size(178, 24);
            this.alignmentComboBox.TabIndex = 4;
            this.alignmentComboBox.SelectedIndexChanged += new System.EventHandler(this.AlignmentComboBox_SelectedIndexChanged);
            // 
            // CheckAnimBox
            // 
            this.CheckAnimBox.AutoSize = true;
            this.CheckAnimBox.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckAnimBox.Location = new System.Drawing.Point(82, 67);
            this.CheckAnimBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CheckAnimBox.Name = "CheckAnimBox";
            this.CheckAnimBox.Size = new System.Drawing.Size(152, 20);
            this.CheckAnimBox.TabIndex = 5;
            this.CheckAnimBox.Text = "Enable sweep animation";
            this.CheckAnimBox.UseVisualStyleBackColor = true;
            this.CheckAnimBox.CheckedChanged += new System.EventHandler(this.CheckAnimBox_CheckedChanged);
            // 
            // LRCSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.CheckAnimBox);
            this.Controls.Add(this.alignmentComboBox);
            this.Controls.Add(this.CustomColorCheckBox);
            this.Controls.Add(this.fontSizeBox);
            this.Controls.Add(this.LyricsLabel);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "LRCSettings";
            this.Size = new System.Drawing.Size(553, 100);
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
        private System.Windows.Forms.CheckBox CheckAnimBox;
    }
}
