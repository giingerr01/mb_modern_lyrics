namespace MusicBeePlugin.Enhanced_lyrics
{
    partial class SettingsPanel
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
            this.FontSizeInput = new System.Windows.Forms.NumericUpDown();
            this.fontSizeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.FontSizeInput)).BeginInit();
            this.SuspendLayout();
            // 
            // FontSizeInput
            // 
            this.FontSizeInput.Location = new System.Drawing.Point(126, 20);
            this.FontSizeInput.Maximum = new decimal(new int[] {
            72,
            0,
            0,
            0});
            this.FontSizeInput.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.FontSizeInput.Name = "FontSizeInput";
            this.FontSizeInput.Size = new System.Drawing.Size(64, 20);
            this.FontSizeInput.TabIndex = 1;
            // 
            // fontSizeLabel
            // 
            this.fontSizeLabel.AutoSize = true;
            this.fontSizeLabel.Location = new System.Drawing.Point(12, 22);
            this.fontSizeLabel.Name = "fontSizeLabel";
            this.fontSizeLabel.Size = new System.Drawing.Size(108, 13);
            this.fontSizeLabel.TabIndex = 0;
            this.fontSizeLabel.Text = "Lyrics font size (8-72):";
            // 
            // SettingsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.fontSizeLabel);
            this.Controls.Add(this.FontSizeInput);
            this.Name = "SettingsPanel";
            this.Size = new System.Drawing.Size(300, 60);
            ((System.ComponentModel.ISupportInitialize)(this.FontSizeInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown FontSizeInput;
        private System.Windows.Forms.Label fontSizeLabel;
    }
}
