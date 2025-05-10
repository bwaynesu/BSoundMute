using System.Drawing;
using System.Windows.Forms;

namespace BSoundMute.Controls.Themes
{
    internal class Aero : ThemeBase
    {
        private Size _maxFrameBorder = Size.Empty;
        private Size _minFrameBorder = Size.Empty;

        public Aero(Form form) : base(form)
        {
        }

        public override Color BackColor => Color.Transparent;

        public override Size ControlBoxSize
        {
            get
            {
                if (base._controlBoxSize == Size.Empty)
                {
                    if (IsToolbar)
                    {
                        if (_form.ControlBox)
                        {
                            base._controlBoxSize = new Size(SystemButtonSize.Width, SystemButtonSize.Height);
                        }
                        else
                        {
                            base._controlBoxSize = new Size(1, 0);
                        }
                    }
                    else
                    {
                        if (!_form.MaximizeBox && !_form.MinimizeBox && _form.ControlBox)
                        {
                            if (_form.HelpButton)
                            {
                                base._controlBoxSize = new Size((2 * SystemButtonSize.Width) + 7, SystemButtonSize.Height);
                            }
                            else
                            {
                                base._controlBoxSize = new Size((1 * SystemButtonSize.Width) + 13, SystemButtonSize.Height);
                            }
                        }
                        else
                        {
                            int index;
                            index = (_form.ControlBox) ? 3 : 0;
                            base._controlBoxSize = new Size(index * SystemButtonSize.Width, SystemButtonSize.Height);
                        }
                    }
                }
                return base._controlBoxSize;
            }
        }

        public override Point ButtonOffset
        {
            get
            {
                if (base._buttonOffset == Point.Empty)
                {
                    if (IsToolbar)
                    {
                        base._buttonOffset = new Point(3, 1);
                    }
                    else
                    {
                        base._buttonOffset = new Point(0, -2);
                    }
                }
                return base._buttonOffset;
            }
        }

        public override Size FrameBorder
        {
            get
            {
                if (_form.WindowState == FormWindowState.Maximized)
                {
                    if (_maxFrameBorder == Size.Empty)
                    {
                        switch (_form.FormBorderStyle)
                        {
                            case FormBorderStyle.FixedToolWindow:
                                _maxFrameBorder = new Size(SystemInformation.FrameBorderSize.Width - 8, -1);
                                break;

                            case FormBorderStyle.SizableToolWindow:
                                _maxFrameBorder = new Size(SystemInformation.FrameBorderSize.Width - 3, 4);
                                break;

                            case FormBorderStyle.Sizable:
                                _maxFrameBorder = new Size(SystemInformation.FrameBorderSize.Width + 2, 7);
                                break;

                            default:
                                _maxFrameBorder = new Size(SystemInformation.FrameBorderSize.Width - 3, 2);
                                break;
                        }
                    }
                    return _maxFrameBorder;
                }
                else
                {
                    if (_minFrameBorder == Size.Empty)
                    {
                        switch (_form.FormBorderStyle)
                        {
                            case FormBorderStyle.FixedToolWindow:
                                _minFrameBorder = new Size(SystemInformation.FrameBorderSize.Width - 8, -1);
                                break;

                            case FormBorderStyle.SizableToolWindow:
                                _minFrameBorder = new Size(SystemInformation.FrameBorderSize.Width - 3, 4);
                                break;

                            case FormBorderStyle.Sizable:
                                _minFrameBorder = new Size(SystemInformation.FrameBorderSize.Width - 3, 1);
                                break;

                            case FormBorderStyle.Fixed3D:
                                _minFrameBorder = new Size(SystemInformation.Border3DSize.Width, -4);
                                break;

                            case FormBorderStyle.FixedSingle:
                                _minFrameBorder = new Size(SystemInformation.Border3DSize.Width - 2, -4);
                                break;

                            default:
                                _minFrameBorder = new Size(SystemInformation.Border3DSize.Width - 1, -4);
                                break;
                        }
                    }
                    return _minFrameBorder;
                }
            }
        }

        public override Size SystemButtonSize
        {
            get
            {
                if (base._systemButtonSize == Size.Empty)
                {
                    if (IsToolbar)
                    {
                        Size size = SystemInformation.SmallCaptionButtonSize;
                        size.Height += 2;
                        size.Width += 2;
                        base._systemButtonSize = size;
                    }
                    else
                    {
                        Size size = SystemInformation.CaptionButtonSize;
                        size.Height += 1;
                        //size.Width -= 1;
                        base._systemButtonSize = size;
                    }
                }
                return base._systemButtonSize;
            }
        }
    }
}