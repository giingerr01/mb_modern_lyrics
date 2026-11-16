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
            this.SuspendLayout();
            // 
            // LyricsLabel
            // 
            this.LyricsLabel.AutoSize = true;
            this.LyricsLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LyricsLabel.Location = new System.Drawing.Point(44, 17);
            this.LyricsLabel.Name = "LyricsLabel";
            this.LyricsLabel.Size = new System.Drawing.Size(63, 13);
            this.LyricsLabel.TabIndex = 0;
            this.LyricsLabel.Text = "LRC Lyrics :";
            this.LyricsLabel.Click += new System.EventHandler(this.LyricsLabel_Click);
            // 
            // fontSizeBox
            // 
            this.fontSizeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fontSizeBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.fontSizeBox.Location = new System.Drawing.Point(99, 33);
            this.fontSizeBox.Name = "fontSizeBox";
            this.fontSizeBox.ReadOnly = true;
            this.fontSizeBox.Size = new System.Drawing.Size(178, 20);
            this.fontSizeBox.TabIndex = 1;
            this.fontSizeBox.Click += new System.EventHandler(this.FontSelctor_Click);
            // 
            // LRCSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.fontSizeBox);
            this.Controls.Add(this.LyricsLabel);
            this.Name = "LRCSettings";
            this.Size = new System.Drawing.Size(344, 118);
            this.Load += new System.EventHandler(this.LRCSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LyricsLabel;
        private System.Windows.Forms.TextBox fontSizeBox;
        private System.Windows.Forms.FontDialog fontDialog1;
    }
}
