using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bSoundMute.Forms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // copy the email address to the clipboard
            // label text is the email address
            Clipboard.SetText(linkLabel1.Text);

            // show a message box to inform the user
            MessageBox.Show("Email address copied to clipboard.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}