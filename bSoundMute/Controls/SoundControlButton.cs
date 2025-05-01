using System.Drawing;
using System.Windows.Forms;

namespace BSoundMute.Controls
{
    public class SoundControlButton : Button
    {
        public SoundControlButton()
        {
            Initialize();
        }

        protected void Initialize()
        {
            FlatStyle = FlatStyle.Flat;
            Image = Properties.Resources.mute;
            BackColor = Color.Transparent;
            ForeColor = Color.Transparent;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.CheckedBackColor = Color.Transparent;
            FlatAppearance.MouseDownBackColor = Color.Transparent;
            FlatAppearance.MouseOverBackColor = Color.Transparent;
            BackgroundImage = Properties.Resources.clear_bg;
            BackgroundImageLayout = ImageLayout.Stretch;
            Width = BackgroundImage.Width;
            Height = BackgroundImage.Height;

            MouseEnter += new System.EventHandler(button_MouseEnter);
            MouseLeave += new System.EventHandler(button_MouseLeave);
            MouseDown += new MouseEventHandler(button_MouseDown);
            MouseUp += new MouseEventHandler(button_MouseUp);
        }

        private void button_MouseEnter(object sender, System.EventArgs e)
        {
            BackgroundImage = Properties.Resources.hover_bg;
        }

        private void button_MouseLeave(object sender, System.EventArgs e)
        {
            BackgroundImage = Properties.Resources.clear_bg;
        }

        private void button_MouseDown(object sender, System.EventArgs e)
        {
            BackgroundImage = Properties.Resources.press_bg;
        }

        private void button_MouseUp(object sender, System.EventArgs e)
        {
            BackgroundImage = Properties.Resources.hover_bg;
        }
    }
}