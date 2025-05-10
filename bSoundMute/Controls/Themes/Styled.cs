using System.Drawing;
using System.Windows.Forms;

namespace BSoundMute.Controls.Themes
{
    internal class Styled : ThemeBase
    {
        public Styled(Form form) : base(form)
        {
        }

        public override Color BackColor => Color.Transparent;

        public override Size SystemButtonSize
        {
            get
            {
                if (base._systemButtonSize == Size.Empty)
                {
                    if (IsToolbar)
                    {
                        Size size = base.SystemButtonSize;
                        size.Height += 2;
                        size.Width -= 1;
                        base._systemButtonSize = size;
                    }
                    else
                    {
                        Size size = SystemInformation.CaptionButtonSize;
                        size.Height -= 2;
                        size.Width -= 2;
                        base._systemButtonSize = size;
                    }
                }
                return base._systemButtonSize;
            }
        }

        public override Size FrameBorder
        {
            get
            {
                if (base._frameBorder == Size.Empty)
                {
                    switch (_form.FormBorderStyle)
                    {
                        case FormBorderStyle.SizableToolWindow:
                        case FormBorderStyle.Sizable:
                            base._frameBorder = new Size(SystemInformation.FrameBorderSize.Width + 1,
                                                        SystemInformation.FrameBorderSize.Height + 1);
                            break;

                        default:
                            base._frameBorder = new Size(SystemInformation.Border3DSize.Width + 2,
                                                        SystemInformation.Border3DSize.Height + 2);
                            break;
                    }
                }
                return base._frameBorder;
            }
        }
    }
}