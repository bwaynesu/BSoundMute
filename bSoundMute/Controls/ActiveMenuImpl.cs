using BSoundMute.Controls.Themes;
using BSoundMute.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BSoundMute.Controls
{
    internal class ActiveMenuImpl : Form, IActiveMenu
    {
        internal enum SpillOverMode
        {
            Hide,
            IncreaseSize
        }

        public IActiveItems Items
        {
            get { return items; }
        }

        public ToolTip ToolTip
        {
            get { return tooltip ?? (tooltip = new ToolTip()); }
            set { tooltip = value; }
        }

        private static readonly Dictionary<Form, IActiveMenu> parents;
        private readonly IContainer components;
        private readonly ActiveItemsImpl items;
        private readonly Size originalMinSize;
        private readonly Form parentForm;
        private readonly SpillOverMode spillOverMode;
        private readonly ThemeFactory themeFactory;
        private int containerMaxWidth;
        private bool isActivated;
        private ITheme theme;
        private ToolTip tooltip;

        public static IActiveMenu GetInstance(Form form)
        {
            if (!parents.ContainsKey(form))
            {
                parents.Add(form, new ActiveMenuImpl(form));
            }
            return parents[form];
        }

        static ActiveMenuImpl()
        {
            parents = new Dictionary<Form, IActiveMenu>();
        }

        private ActiveMenuImpl(Form form)
        {
            InitializeComponent();
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            FormBorderStyle = FormBorderStyle.None;
            SizeGripStyle = SizeGripStyle.Hide;

            items = new ActiveItemsImpl();
            items.CollectionModified += ItemsCollectionModified;
            parentForm = form;
            Show(form);
            parentForm.Disposed += ParentFormDisposed;
            Visible = false;
            isActivated = form.WindowState != FormWindowState.Minimized;
            themeFactory = new ThemeFactory(form);
            theme = themeFactory.GetTheme();
            originalMinSize = form.MinimumSize;
            AttachHandlers();
            ToolTip.ShowAlways = true;
            TopMost = false;
            spillOverMode = SpillOverMode.IncreaseSize;
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
            if (parents.ContainsKey(form))
            {
                parents.Remove(form);
            }
        }

        protected void AttachHandlers()
        {
            parentForm.Deactivate += ParentFormDeactivate;
            parentForm.Activated += ParentFormActivated;
            parentForm.SizeChanged += ParentRefresh;
            parentForm.VisibleChanged += ParentRefresh;
            parentForm.Move += ParentRefresh;
            parentForm.SystemColorsChanged += TitleButtonSystemColorsChanged;
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
            /*theme = themeFactory.GetTheme();
            CalcSize();
            OnPosition();*/
            Application.Restart();
            Process.GetCurrentProcess().Kill();
        }

        private void CalcSize()
        {
            int left = 0;
            for (int i = (Items.Count - 1); i >= 0; i--)
            {
                var button = (SoundControlButton)Items[i];
                button.SetTheme(theme);
                button.Left = left;
                left += Items[i].Width + theme.ButtonOffset.X;
                button.Top = theme.ButtonOffset.Y;
            }
            containerMaxWidth = left;

            if (spillOverMode == SpillOverMode.IncreaseSize)
            {
                int w = containerMaxWidth + theme.ControlBoxSize.Width + theme.FrameBorder.Width +
                        theme.FrameBorder.Width;

                parentForm.MinimumSize = originalMinSize;

                if (parentForm.MinimumSize.Width <= w)
                {
                    parentForm.MinimumSize = new Size(w, parentForm.MinimumSize.Height);
                }
            }
        }

        protected void ParentRefresh(object sender, EventArgs e)
        {
            if (parentForm.WindowState == FormWindowState.Minimized)
            {
                isActivated = false;
                Visible = false;
            }
            else
            {
                isActivated = true;
                OnPosition();
            }
            Win32.SetWindowPos(parentForm.Handle, Win32.HWND_TOPMOST, 0, 0, 0, 0, Win32.SWP_NOMOVE | Win32.SWP_NOSIZE | Win32.SWP_SHOWWINDOW);
        }

        private void OnPosition()
        {
            if (!IsDisposed)
            {
                if (theme == null || !theme.IsDisplayed)
                {
                    Visible = false;
                    return;
                }

                int top = theme.FrameBorder.Height;
                int left = theme.FrameBorder.Width + theme.ControlBoxSize.Width;

                Top = top + parentForm.Top;
                Left = parentForm.Left + parentForm.Width - containerMaxWidth - left;

                Visible = theme.IsDisplayed && isActivated;

                if (Visible)
                {
                    if (Items.Count > 0)
                    {
                        if (Win32.DwmIsCompositionEnabled || (Application.RenderWithVisualStyles && Win32.version > 6)) // traditional and standard theme don't have opacity
                        {
                            Opacity = parentForm.Opacity;
                            if (parentForm.Visible)
                            {
                                Opacity = parentForm.Opacity;
                            }
                            else
                            {
                                Visible = false;
                            }
                        }
                    }
                    if (spillOverMode == SpillOverMode.Hide)
                    {
                        foreach (SoundControlButton b in Items)
                        {
                            if (b.Left + Left - theme.FrameBorder.Width + 2 < parentForm.Left)
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
            if (disposing && (components != null))
            {
                components.Dispose();
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