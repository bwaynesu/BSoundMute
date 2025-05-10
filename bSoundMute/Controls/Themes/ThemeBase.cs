using System;
using System.Drawing;
using System.Windows.Forms;

namespace BSoundMute.Controls.Themes
{
    internal class ThemeBase : ITheme
    {
        protected Color _backColor = Color.Empty;
        protected Point _buttonOffset = new Point(55, 0);
        protected Size _controlBoxSize = Size.Empty;
        protected Form _form;
        protected Size _frameBorder = Size.Empty;
        protected bool? _isDisplayed;
        protected bool? _isToolbar;
        protected Size _systemButtonSize = Size.Empty;

        public ThemeBase(Form form)
        {
            this._form = form;
        }

        protected bool IsToolbar
        {
            get
            {
                if (_isToolbar == null)
                {
                    _isToolbar = _form.FormBorderStyle == FormBorderStyle.FixedToolWindow ||
                                _form.FormBorderStyle == FormBorderStyle.SizableToolWindow;
                }
                return (bool)_isToolbar;
            }
        }

        #region ITheme Members

        public virtual Color BackColor
        {
            get
            {
                if (_backColor == Color.Empty)
                {
                    _backColor = Color.FromKnownColor(KnownColor.Control);
                }
                return _backColor;
            }
        }

        public bool IsDisplayed
        {
            get
            {
                return true;
            }
        }

        public virtual Size ControlBoxSize
        {
            get
            {
                if (_controlBoxSize == Size.Empty)
                {
                    if (IsToolbar)
                    {
                        if (_form.ControlBox)
                        {
                            _controlBoxSize = new Size(SystemButtonSize.Width, SystemButtonSize.Height);
                        }
                        else
                        {
                            _controlBoxSize = new Size(0, 0);
                        }
                    }
                    else
                    {
                        int index;
                        if (!_form.MaximizeBox && !_form.MinimizeBox && _form.ControlBox)
                        {
                            index = (_form.HelpButton) ? 2 : 1;
                        }
                        else
                        {
                            index = (_form.ControlBox) ? 3 : 0;
                        }
                        _controlBoxSize = new Size(index * SystemButtonSize.Width, SystemButtonSize.Height);
                    }
                }
                return _controlBoxSize;
            }
        }

        public virtual Point ButtonOffset
        {
            get { return _buttonOffset; }
        }

        public virtual Size FrameBorder
        {
            get
            {
                if (_frameBorder == Size.Empty)
                {
                    switch (_form.FormBorderStyle)
                    {
                        case FormBorderStyle.SizableToolWindow:
                            _frameBorder = new Size(SystemInformation.FrameBorderSize.Width + 2,
                                                   SystemInformation.FrameBorderSize.Height + 2);
                            break;

                        case FormBorderStyle.Sizable:
                            _frameBorder = new Size(SystemInformation.FrameBorderSize.Width,
                                                   SystemInformation.FrameBorderSize.Height + 2);
                            break;

                        case FormBorderStyle.FixedToolWindow:
                            _frameBorder = new Size(SystemInformation.Border3DSize.Width + 3,
                                                   SystemInformation.Border3DSize.Height + 3);
                            break;

                        default:
                            _frameBorder = new Size(SystemInformation.Border3DSize.Width + 1,
                                                   SystemInformation.Border3DSize.Height + 3);
                            break;
                    }
                }
                return _frameBorder;
            }
        }

        public virtual Size SystemButtonSize
        {
            get
            {
                if (_systemButtonSize == Size.Empty)
                {
                    if (IsToolbar)
                    {
                        Size size = SystemInformation.ToolWindowCaptionButtonSize;
                        size.Height -= 4;
                        size.Width -= 1;
                        _systemButtonSize = size;
                    }
                    else
                    {
                        _systemButtonSize = new Size(SystemInformation.CaptionButtonSize.Width,
                                                    SystemInformation.CaptionHeight - 2 * Math.Max(SystemInformation.BorderSize.Height, SystemInformation.Border3DSize.Height) - 1);
                    }
                }
                return _systemButtonSize;
            }
        }

        #endregion ITheme Members
    }
}