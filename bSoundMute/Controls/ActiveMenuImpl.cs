using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using BSoundMute.Controls.Themes;
using BSoundMute.Utils;

namespace BSoundMute.Controls
{
    internal class ActiveMenuImpl : Form, IActiveMenu
    {
        internal enum SpillOverMode
        {
            Hide,
            IncreaseSize
        }

        private static readonly Dictionary<Form, IActiveMenu> s_parents;

        private readonly IContainer _components;
        private readonly ActiveItemsImpl _items;
        private readonly Size _originalMinSize;
        private readonly Form _parentForm;
        private readonly SpillOverMode _spillOverMode;
        private readonly ThemeFactory _themeFactory;

        private int _containerMaxWidth;
        private bool _isActivated;
        private ITheme _theme;
        private ToolTip _tooltip;

        public IActiveItems Items => _items;

        public ToolTip ToolTip
        {
            get { return _tooltip ??= new ToolTip(); }
            set { _tooltip = value; }
        }

        public static IActiveMenu GetInstance(Form form)
        {
            if (!s_parents.TryGetValue(form, out IActiveMenu value))
            {
                value = new ActiveMenuImpl(form);
                s_parents.Add(form, value);
            }

            return value;
        }

        static ActiveMenuImpl()
        {
            s_parents = [];
        }

        private ActiveMenuImpl(Form form)
        {
            InitializeComponent();
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            FormBorderStyle = FormBorderStyle.None;
            SizeGripStyle = SizeGripStyle.Hide;

            _items = new ActiveItemsImpl();
            _items.CollectionModified += ItemsCollectionModified;
            _parentForm = form;
            Show(form);
            _parentForm.Disposed += ParentFormDisposed;
            Visible = false;
            _isActivated = form.WindowState != FormWindowState.Minimized;
            _themeFactory = new ThemeFactory(form);
            _theme = _themeFactory.GetTheme();
            _originalMinSize = form.MinimumSize;
            AttachHandlers();
            ToolTip.ShowAlways = true;
            TopMost = false;
            _spillOverMode = SpillOverMode.IncreaseSize;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.BringToFront();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams p = base.CreateParams;
                p.Style = (int)Win32.WS_CHILD;
                p.Style |= (int)Win32.WS_CLIPSIBLINGS;
                p.ExStyle &= (int)Win32.WS_EX_LAYERED;
                p.Parent = Win32.GetDesktopWindow();
                return p;
            }
        }

        private void ItemsCollectionModified(object sender, ListModificationEventArgs e)
        {
            Controls.Clear();
            foreach (SoundControlButton button in Items)
            {
                Controls.Add(button);
            }
            CalcSize();
            OnPosition();
        }

        private void ParentFormDisposed(object sender, EventArgs e)
        {
            var form = (Form)sender;
            if (form == null)
            {
                return;
            }
            if (s_parents.ContainsKey(form))
            {
                s_parents.Remove(form);
            }
        }

        protected void AttachHandlers()
        {
            _parentForm.Deactivate += ParentFormDeactivate;
            _parentForm.Activated += ParentFormActivated;
            _parentForm.SizeChanged += ParentRefresh;
            _parentForm.VisibleChanged += ParentRefresh;
            _parentForm.Move += ParentRefresh;
            _parentForm.SystemColorsChanged += TitleButtonSystemColorsChanged;

            // used to mask the menu control behind the buttons.
            if (Win32.DwmIsCompositionEnabled)
            {
                BackColor = Color.Fuchsia;
                TransparencyKey = Color.Fuchsia;
            }
            else
            {
                BackColor = Color.FromKnownColor(KnownColor.ActiveCaption);
                TransparencyKey = BackColor;
            }
        }

        private void ParentFormDeactivate(object sender, EventArgs e)
        {
            ToolTip.ShowAlways = false;
        }

        private void ParentFormActivated(object sender, EventArgs e)
        {
            ToolTip.ShowAlways = true;
        }

        private void TitleButtonSystemColorsChanged(object sender, EventArgs e)
        {
            Application.Restart();
            Process.GetCurrentProcess().Kill();
        }

        private void CalcSize()
        {
            int left = 0;
            for (int i = (Items.Count - 1); i >= 0; i--)
            {
                var button = Items[i];

                button.Left = left;
                left += Items[i].Width + _theme.ButtonOffset.X;
                button.Top = _theme.ButtonOffset.Y;
            }
            _containerMaxWidth = left;

            if (_spillOverMode == SpillOverMode.IncreaseSize)
            {
                int w =
                    _containerMaxWidth +
                    _theme.ControlBoxSize.Width +
                    _theme.FrameBorder.Width +
                    _theme.FrameBorder.Width;

                _parentForm.MinimumSize = _originalMinSize;

                if (_parentForm.MinimumSize.Width <= w)
                {
                    _parentForm.MinimumSize = new Size(w, _parentForm.MinimumSize.Height);
                }
            }
        }

        protected void ParentRefresh(object sender, EventArgs e)
        {
            if (_parentForm.WindowState == FormWindowState.Minimized)
            {
                _isActivated = false;
                Visible = false;
            }
            else
            {
                _isActivated = true;
                OnPosition();
            }

            Win32.SetWindowPos(_parentForm.Handle, Win32.HWND_TOPMOST, 0, 0, 0, 0, Win32.SWP_NOMOVE | Win32.SWP_NOSIZE | Win32.SWP_SHOWWINDOW);
        }

        private void OnPosition()
        {
            if (!IsDisposed)
            {
                if (_theme == null || !_theme.IsDisplayed)
                {
                    Visible = false;
                    return;
                }

                int top = _theme.FrameBorder.Height;
                int left = _theme.FrameBorder.Width + _theme.ControlBoxSize.Width;

                Top = top + _parentForm.Top;
                Left = _parentForm.Left + _parentForm.Width - _containerMaxWidth - left;

                Visible = _theme.IsDisplayed && _isActivated;

                if (Visible)
                {
                    if (Items.Count > 0)
                    {
                        if (Win32.DwmIsCompositionEnabled || (Application.RenderWithVisualStyles && Win32.version > 6)) // traditional and standard theme don't have opacity
                        {
                            Opacity = _parentForm.Opacity;
                            if (_parentForm.Visible)
                            {
                                Opacity = _parentForm.Opacity;
                            }
                            else
                            {
                                Visible = false;
                            }
                        }
                    }

                    if (_spillOverMode == SpillOverMode.Hide)
                    {
                        foreach (SoundControlButton b in Items)
                        {
                            if (b.Left + Left - _theme.FrameBorder.Width + 2 < _parentForm.Left)
                            {
                                b.Visible = false;
                            }
                            else
                            {
                                b.Visible = true;
                            }
                        }
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (_components != null))
            {
                _components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            ClientSize = new Size(10, 10);
            ControlBox = false;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ActiveMenu";
            ShowIcon = false;
            ShowInTaskbar = false;
            ResumeLayout(false);
        }
    }
}
