using System.Drawing;
using System.Windows.Forms;

namespace BSoundMute.Controls.Themes
{
    internal class XPStyle : Styled
    {
        public XPStyle(Form form) : base(form)
        {
        }

        public override Color BackColor
        {
            get
            {
                if (base._backColor == Color.Empty)
                {
                    base._backColor = Color.FromKnownColor(KnownColor.ActiveBorder);
                }
                return base._backColor;
            }
        }

        public override Size FrameBorder
        {
            get
            {
                if (base._frameBorder == Size.Empty)
                {
                    base._frameBorder = new Size(base.FrameBorder.Width + 2, base.FrameBorder.Height);
                }
                return base._frameBorder;
            }
        }
    }
}