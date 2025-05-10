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
            Hide();
        }

        private void OnEmailLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // copy the email address to the clipboard
            // label text is the email address
            Clipboard.SetText(_emailLinkLabel.Text);

            // show a message box to inform the user
            MessageBox.Show("Email address copied to clipboard.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
