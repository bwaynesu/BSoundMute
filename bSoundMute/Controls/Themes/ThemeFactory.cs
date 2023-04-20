using System.Windows.Forms;
using bSoundMute.Utils;

namespace bSoundMute.Controls.Themes
{
  internal class ThemeFactory
  {
    private readonly Form form;

    public ThemeFactory(Form form)
    {
      this.form = form;
    }

    public ITheme GetTheme()
    {
      if (Win32.DwmIsCompositionEnabled)
      {
        // vista
        return new Aero(form);
      }
      else if (Application.RenderWithVisualStyles && Win32.version > 6)
      {
        // vista basic
        return new Styled(form);
      }
      else if (Application.RenderWithVisualStyles)
      {
        // xp
        return new XPStyle(form);
      }
      else
      {
        return new Standard(form);
      }
    }
  }
}
