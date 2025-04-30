using System.Drawing;
using System.Windows.Forms;

namespace BSoundMute.Controls.Themes
{
    internal class XPStyle : Styled
    {
        public XPStyle(Form form)
          : base(form)
        {
        }

        public override Color BackColor
        {
            get
            {
                if (base.backColor == Color.Empty)
                {
                    base.backColor = Color.FromKnownColor(KnownColor.ActiveBorder);
                }
                return base.backColor;
            }
        }

        public override Size FrameBorder
        {
            get
            {
                if (base.frameBorder == Size.Empty)
                {
                    base.frameBorder = new Size(base.FrameBorder.Width + 2, base.FrameBorder.Height);
                }
                return base.frameBorder;
            }
        }
    }
}