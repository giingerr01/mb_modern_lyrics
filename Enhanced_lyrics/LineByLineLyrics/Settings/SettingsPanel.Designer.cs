namespace MusicBeePlugin.Enhanced_lyrics.LineByLineLyrics.Settings
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PanelFontPicker = new System.Windows.Forms.TextBox();
            this.ThreeDotButton = new System.Windows.Forms.Button();
            this.chkCustomFont = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // PanelFontPicker
            // 
            this.PanelFontPicker.BackColor = System.Drawing.SystemColors.Window;
            this.PanelFontPicker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelFontPicker.Location = new System.Drawing.Point(75, 64);
            this.PanelFontPicker.Name = "PanelFontPicker";
            this.PanelFontPicker.ReadOnly = true;
            this.PanelFontPicker.Size = new System.Drawing.Size(226, 20);
            this.PanelFontPicker.TabIndex = 1;
            // 
            // ThreeDotButton
            // 
            this.ThreeDotButton.Location = new System.Drawing.Point(269, 64);
            this.ThreeDotButton.Name = "ThreeDotButton";
            this.ThreeDotButton.Size = new System.Drawing.Size(32, 20);
            this.ThreeDotButton.TabIndex = 2;
            this.ThreeDotButton.Text = "...";
            this.ThreeDotButton.UseVisualStyleBackColor = true;
            this.ThreeDotButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkCustomFont
            // 
            this.chkCustomFont.AutoSize = true;
            this.chkCustomFont.Location = new System.Drawing.Point(75, 41);
            this.chkCustomFont.Name = "chkCustomFont";
            this.chkCustomFont.Size = new System.Drawing.Size(85, 17);
            this.chkCustomFont.TabIndex = 3;
            this.chkCustomFont.Text = "Custom Font";
            this.chkCustomFont.UseVisualStyleBackColor = true;
            this.chkCustomFont.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // SettingsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(404, 168);
            this.Controls.Add(this.chkCustomFont);
            this.Controls.Add(this.ThreeDotButton);
            this.Controls.Add(this.PanelFontPicker);
            this.Name = "SettingsPanel";
            this.Text = "LRC Lyrics";
            this.Load += new System.EventHandler(this.SettingsPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox PanelFontPicker;
        private System.Windows.Forms.Button ThreeDotButton;
        private System.Windows.Forms.CheckBox chkCustomFont;
    }
}