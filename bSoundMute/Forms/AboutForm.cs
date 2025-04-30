using System.Windows.Forms;

namespace BSoundMute.Forms
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