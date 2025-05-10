namespace BSoundMute.Forms
{
    partial class AboutForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label _authorLabel;
        private System.Windows.Forms.Label _emailLabel;
        private System.Windows.Forms.LinkLabel _emailLinkLabel;

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
            _authorLabel = new System.Windows.Forms.Label();
            _emailLabel = new System.Windows.Forms.Label();
            _emailLinkLabel = new System.Windows.Forms.LinkLabel();
            SuspendLayout();
            // 
            // _authorLabel
            // 
            _authorLabel.AutoSize = true;
            _authorLabel.Location = new System.Drawing.Point(17, 14);
            _authorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _authorLabel.Name = "_authorLabel";
            _authorLabel.Size = new System.Drawing.Size(103, 15);
            _authorLabel.TabIndex = 0;
            _authorLabel.Text = "Author: bwaynesu";
            // 
            // _emailLabel
            // 
            _emailLabel.AutoSize = true;
            _emailLabel.Location = new System.Drawing.Point(17, 35);
            _emailLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _emailLabel.Name = "_emailLabel";
            _emailLabel.Size = new System.Drawing.Size(42, 15);
            _emailLabel.TabIndex = 1;
            _emailLabel.Text = "Email: ";
            // 
            // _emailLinkLabel
            // 
            _emailLinkLabel.AutoSize = true;
            _emailLinkLabel.Location = new System.Drawing.Point(53, 35);
            _emailLinkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            _emailLinkLabel.Name = "_emailLinkLabel";
            _emailLinkLabel.Size = new System.Drawing.Size(150, 15);
            _emailLinkLabel.TabIndex = 2;
            _emailLinkLabel.TabStop = true;
            _emailLinkLabel.Text = "bwaynesu.dev@gmail.com";
            _emailLinkLabel.LinkClicked += OnEmailLinkClicked;
            // 
            // AboutForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(119, 196, 176);
            ClientSize = new System.Drawing.Size(226, 67);
            Controls.Add(_emailLinkLabel);
            Controls.Add(_emailLabel);
            Controls.Add(_authorLabel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Margin = new System.Windows.Forms.Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "About me";
            TopMost = true;
            FormClosing += AboutForm_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
