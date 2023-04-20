using System;
using System.Drawing;
using System.Windows.Forms;

using bSoundMute.Controls.Themes;
using bSoundMute.Utils;

namespace bSoundMute.Controls
{
  public class SoundControlButton : Button
  {
    private Size buttonSize_;
    private int buttonX_;
    private int buttonY_;
    private ITheme theme_;

    public int Top_;
    public int Left_;
    public int Width_;
    public int Height_;

    public ITheme GetTheme() { return theme_; }
    public void SetTheme(ITheme _theme) { theme_ = _theme; }

    public SoundControlButton()
    {
      Initialize();
    }

    protected void Initialize()
    {
      base.FlatStyle = FlatStyle.Flat;
      base.Image = Properties.Resources.mute;
      base.BackColor = Color.Transparent;
      base.ForeColor = Color.Transparent;
      base.FlatAppearance.BorderSize = 0;
      base.FlatAppearance.CheckedBackColor = Color.Transparent;
      base.FlatAppearance.MouseDownBackColor = Color.Transparent;
      base.FlatAppearance.MouseOverBackColor = Color.Transparent;
      base.MouseEnter += new System.EventHandler(button_MouseEnter);
      base.MouseLeave += new System.EventHandler(button_MouseLeave);
      base.MouseDown += new System.Windows.Forms.MouseEventHandler(button_MouseDown);
      base.MouseUp += new System.Windows.Forms.MouseEventHandler(button_MouseUp);
      base.BackgroundImage = Properties.Resources.frame_dark;
      base.BackgroundImageLayout = ImageLayout.Stretch;

      CalcButtonSize();
    }

    private void CalcButtonSize()
    {
      if (theme_ != null)
      {
        buttonSize_ = theme_.SystemButtonSize;
        if (BackColor == Color.Empty)
        {
          BackColor = theme_.BackColor;
        }
      }
      else
      {
        buttonSize_ = SystemInformation.CaptionButtonSize;
      }

      /*base.Width = buttonSize_.Width;
      base.Height = buttonSize_.Height;*/

      base.Width = 39;
      base.Height = 25;

      buttonX_ = base.Width / 2 - 1;
      buttonY_ = base.Height / 2 - 1;
    }

    private void button_MouseEnter(object sender, System.EventArgs e)
    {
      base.BackgroundImage = Properties.Resources.glowbg;
    }

    private void button_MouseLeave(object sender, System.EventArgs e)
    {
      base.BackgroundImage = Properties.Resources.frame_dark;
    }

    private void button_MouseDown(object sender, System.EventArgs e)
    {
      base.BackgroundImage = Properties.Resources.darkbg;
    }

    private void button_MouseUp(object sender, System.EventArgs e)
    {
      base.BackgroundImage = Properties.Resources.glowbg;
    }

  }
}
