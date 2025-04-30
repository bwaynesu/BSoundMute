using System.Windows.Forms;

namespace BSoundMute.Controls
{
    public static class ActiveMenu
    {
        public static IActiveMenu GetInstance(Form form)
        {
            return ActiveMenuImpl.GetInstance(form);
        }
    }
}