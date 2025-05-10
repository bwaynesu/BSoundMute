using BSoundMute.Utils;
using System.Windows.Forms;

namespace BSoundMute.Controls.Themes
{
    internal class ThemeFactory
    {
        private readonly Form _form;

        public ThemeFactory(Form form)
        {
            this._form = form;
        }

        public ITheme GetTheme()
        {
            if (Win32.DwmIsCompositionEnabled)
            {
                // vista
                return new Aero(_form);
            }
            else if (Application.RenderWithVisualStyles && Win32.version > 6)
            {
                // vista basic
                return new Styled(_form);
            }
            else if (Application.RenderWithVisualStyles)
            {
                // xp
                return new XPStyle(_form);
            }
            else
            {
                return new Standard(_form);
            }
        }
    }
}