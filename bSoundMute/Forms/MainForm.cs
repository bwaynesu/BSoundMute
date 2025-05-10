using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BSoundMute.Controls;
using BSoundMute.Utils;

namespace BSoundMute.Forms
{
    public class MainForm : Form
    {
        private System.ComponentModel.IContainer components;

        private Label _titleLabel;
        private Label _windowTitleLabel;
        private Label _windowHandleLabel;
        private Label _windowSizeValueLabel;
        private Label _captionWindowLabel;
        private Label _idWindowLabel;
        private Label _windowSizeLabel;
        private Button _exitButton;
        private System.Windows.Forms.Timer _updateTimer;
        private NotifyIcon _runBgNotifyIcon;
        private IntPtr _thisAppHandle;
        private IntPtr _handle;
        private IntPtr _preHandle;
        private int _applicationNameHash;
        private StringBuilder _buff = new(256);
        private Win32.RECT _preAppRect;
        private Win32.RECT _appRect;
        private AppForm _appForm = new();
        private SoundControlButton _soundCtrlBtn = new();
        private PictureBox _logoPictureBox;
        private Label _versionLabel;
        private Label _versionValuelabel;
        private Label _copyrightValueLabel;
        private Button _okButton;
        private bool _needExit = false;
        private AboutForm _aboutForm = new();
        private Label _hotkeyLabel;
        private Button _refreshBtn;
        private GlobalKeyboardHook _globalKeyboardHook = new();
        private bool _isNotifyIconShowed = false;

        public MainForm()
        {
            // Add mute button
            AddButton(OnMuteButtonClick);

            InitializeComponent();
            _applicationNameHash = this.Text.GetHashCode();

            InitialGlobalHook();

            var version = typeof(MainForm).Assembly.GetName().Version;
            _versionValuelabel.Text = $"{version.Major}.{version.Minor}.{version.Build}";
            _copyrightValueLabel.Text = "© 2015 " + FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).CompanyName;

            _appForm.Show();
        }

        protected override void Dispose(bool disposing)
        {
            // Ensure keyboard hook is properly unhooked when application exits to prevent resource leaks
            if (_globalKeyboardHook != null)
            {
                _globalKeyboardHook.unhook();
            }

            _runBgNotifyIcon.Visible = false;
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        [STAThread]
        private static void Main()
        {
            using (Mutex mutex = new(true, Application.ProductName, out var createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm());
                }
                else
                {
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            Win32.SetForegroundWindow(process.MainWindowHandle);
                            break;
                        }
                    }
                }
            }
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            _titleLabel = new Label();
            _windowTitleLabel = new Label();
            _windowHandleLabel = new Label();
            _captionWindowLabel = new Label();
            _idWindowLabel = new Label();
            _exitButton = new Button();
            _updateTimer = new System.Windows.Forms.Timer(components);
            _windowSizeLabel = new Label();
            _windowSizeValueLabel = new Label();
            _runBgNotifyIcon = new NotifyIcon(components);
            _logoPictureBox = new PictureBox();
            _versionLabel = new Label();
            _versionValuelabel = new Label();
            _copyrightValueLabel = new Label();
            _okButton = new Button();
            _hotkeyLabel = new Label();
            _refreshBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)_logoPictureBox).BeginInit();
            SuspendLayout();
            // 
            // _titleLabel
            // 
            _titleLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            _titleLabel.Location = new System.Drawing.Point(125, 7);
            _titleLabel.Name = "_titleLabel";
            _titleLabel.Size = new System.Drawing.Size(174, 20);
            _titleLabel.TabIndex = 0;
            _titleLabel.Text = "Active Window Detail";
            _titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _windowTitleLabel
            // 
            _windowTitleLabel.Location = new System.Drawing.Point(19, 39);
            _windowTitleLabel.Name = "_windowTitleLabel";
            _windowTitleLabel.Size = new System.Drawing.Size(115, 20);
            _windowTitleLabel.TabIndex = 1;
            _windowTitleLabel.Text = "Window Title :";
            // 
            // _windowHandleLabel
            // 
            _windowHandleLabel.Location = new System.Drawing.Point(19, 64);
            _windowHandleLabel.Name = "_windowHandleLabel";
            _windowHandleLabel.Size = new System.Drawing.Size(115, 20);
            _windowHandleLabel.TabIndex = 3;
            _windowHandleLabel.Text = "Window Handle :";
            // 
            // _captionWindowLabel
            // 
            _captionWindowLabel.Location = new System.Drawing.Point(102, 39);
            _captionWindowLabel.Name = "_captionWindowLabel";
            _captionWindowLabel.Size = new System.Drawing.Size(304, 20);
            _captionWindowLabel.TabIndex = 2;
            // 
            // _idWindowLabel
            // 
            _idWindowLabel.Location = new System.Drawing.Point(119, 64);
            _idWindowLabel.Name = "_idWindowLabel";
            _idWindowLabel.Size = new System.Drawing.Size(269, 20);
            _idWindowLabel.TabIndex = 4;
            // 
            // _exitButton
            // 
            _exitButton.FlatStyle = FlatStyle.System;
            _exitButton.Location = new System.Drawing.Point(248, 130);
            _exitButton.Name = "_exitButton";
            _exitButton.Size = new System.Drawing.Size(70, 29);
            _exitButton.TabIndex = 9;
            _exitButton.Text = "EXIT";
            _exitButton.Click += OnExitButtonClick;
            // 
            // _updateTimer
            // 
            _updateTimer.Enabled = true;
            _updateTimer.Interval = 250;
            _updateTimer.Tick += OnUpdateTimerTick;
            // 
            // _windowSizeLabel
            // 
            _windowSizeLabel.Location = new System.Drawing.Point(100, 89);
            _windowSizeLabel.Name = "_windowSizeLabel";
            _windowSizeLabel.Size = new System.Drawing.Size(269, 20);
            _windowSizeLabel.TabIndex = 6;
            // 
            // _windowSizeValueLabel
            // 
            _windowSizeValueLabel.Location = new System.Drawing.Point(19, 89);
            _windowSizeValueLabel.Name = "_windowSizeValueLabel";
            _windowSizeValueLabel.Size = new System.Drawing.Size(115, 21);
            _windowSizeValueLabel.TabIndex = 5;
            _windowSizeValueLabel.Text = "Window Size :";
            // 
            // _runBgNotifyIcon
            // 
            _runBgNotifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            _runBgNotifyIcon.BalloonTipText = "Program running in the background";
            _runBgNotifyIcon.BalloonTipTitle = "BSoundMute";
            _runBgNotifyIcon.Icon = (System.Drawing.Icon)resources.GetObject("_runBgNotifyIcon.Icon");
            _runBgNotifyIcon.Visible = true;
            _runBgNotifyIcon.MouseClick += OnRunBgNotifyIconMouseClick;
            _runBgNotifyIcon.MouseDoubleClick += OnRunBgNotifyIconMouseClick;
            // 
            // _logoPictureBox
            // 
            _logoPictureBox.Cursor = Cursors.Hand;
            _logoPictureBox.Image = (System.Drawing.Image)resources.GetObject("_logoPictureBox.Image");
            _logoPictureBox.Location = new System.Drawing.Point(360, 102);
            _logoPictureBox.Name = "_logoPictureBox";
            _logoPictureBox.Size = new System.Drawing.Size(66, 79);
            _logoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            _logoPictureBox.TabIndex = 8;
            _logoPictureBox.TabStop = false;
            _logoPictureBox.Click += OnlogoPictureBoxClick;
            // 
            // _versionLabel
            // 
            _versionLabel.Location = new System.Drawing.Point(3, 171);
            _versionLabel.Name = "_versionLabel";
            _versionLabel.Size = new System.Drawing.Size(66, 21);
            _versionLabel.TabIndex = 11;
            _versionLabel.Text = "Version : ";
            _versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _versionValuelabel
            // 
            _versionValuelabel.Location = new System.Drawing.Point(50, 171);
            _versionValuelabel.Name = "_versionValuelabel";
            _versionValuelabel.Size = new System.Drawing.Size(91, 21);
            _versionValuelabel.TabIndex = 12;
            _versionValuelabel.Text = "1.0.0.0";
            _versionValuelabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _copyrightValueLabel
            // 
            _copyrightValueLabel.Location = new System.Drawing.Point(314, 171);
            _copyrightValueLabel.Name = "_copyrightValueLabel";
            _copyrightValueLabel.Size = new System.Drawing.Size(125, 21);
            _copyrightValueLabel.TabIndex = 14;
            _copyrightValueLabel.Text = "© 2015 bwaynesu";
            _copyrightValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _okButton
            // 
            _okButton.FlatStyle = FlatStyle.System;
            _okButton.Location = new System.Drawing.Point(104, 130);
            _okButton.Name = "_okButton";
            _okButton.Size = new System.Drawing.Size(69, 29);
            _okButton.TabIndex = 7;
            _okButton.Text = "OK";
            _okButton.Click += OnOkButtonClick;
            // 
            // _hotkeyLabel
            // 
            _hotkeyLabel.Location = new System.Drawing.Point(156, 171);
            _hotkeyLabel.Name = "_hotkeyLabel";
            _hotkeyLabel.Size = new System.Drawing.Size(105, 21);
            _hotkeyLabel.TabIndex = 13;
            _hotkeyLabel.Text = "Hotkey : B + LCtrl";
            _hotkeyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _refreshBtn
            // 
            _refreshBtn.FlatStyle = FlatStyle.System;
            _refreshBtn.Location = new System.Drawing.Point(176, 130);
            _refreshBtn.Name = "_refreshBtn";
            _refreshBtn.Size = new System.Drawing.Size(69, 29);
            _refreshBtn.TabIndex = 8;
            _refreshBtn.Text = "Refresh";
            _refreshBtn.Click += OnRefreshButtonClick;
            // 
            // MainForm
            // 
            AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            BackColor = System.Drawing.Color.FromArgb(119, 196, 176);
            ClientSize = new System.Drawing.Size(442, 196);
            Controls.Add(_refreshBtn);
            Controls.Add(_hotkeyLabel);
            Controls.Add(_okButton);
            Controls.Add(_copyrightValueLabel);
            Controls.Add(_versionValuelabel);
            Controls.Add(_versionLabel);
            Controls.Add(_logoPictureBox);
            Controls.Add(_windowSizeLabel);
            Controls.Add(_windowSizeValueLabel);
            Controls.Add(_exitButton);
            Controls.Add(_idWindowLabel);
            Controls.Add(_captionWindowLabel);
            Controls.Add(_windowHandleLabel);
            Controls.Add(_windowTitleLabel);
            Controls.Add(_titleLabel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "BSoundMute";
            TopMost = true;
            FormClosing += OnMainFormClosing;
            ((System.ComponentModel.ISupportInitialize)_logoPictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion Windows Form Designer generated code

        private void InitialGlobalHook()
        {
            _globalKeyboardHook.AddKey(Properties.Settings.Default.CombineKey1);
            _globalKeyboardHook.AddKey(Properties.Settings.Default.CombineKey2);
            _globalKeyboardHook.KeyDown += new KeyEventHandler(OnGlobalKeyboardHookKeyDown);
            _globalKeyboardHook.KeyUp += new KeyEventHandler(OnGlobalKeyboardHookKeyUp);
        }

        private void CalAndSetFormWindowPos()
        {
            if (_appRect.Top != _preAppRect.Top || _appRect.Bottom != _preAppRect.Bottom || _appRect.Left != _preAppRect.Left || _appRect.Right != _preAppRect.Right)
            {
                if (Win32.DwmIsCompositionEnabled || Application.RenderWithVisualStyles) // Not traditional theme
                {
                    if (_appRect.Right - 333 >= _appRect.Left)
                    {
                        Win32.SetWindowPos(_appForm.Handle, Win32.HWND_TOPMOST, _appRect.Right - 278, _appRect.Top + 5, 0, 0, Win32.SWP_NOSIZE | Win32.SWP_SHOWWINDOW);
                    }
                    else
                    {
                        Win32.SetWindowPos(_appForm.Handle, Win32.HWND_TOPMOST, _appRect.Left + 55, _appRect.Top + 5, 0, 0, Win32.SWP_NOSIZE | Win32.SWP_SHOWWINDOW);
                    }
                }
                else
                {
                    if (_appRect.Right - 233 >= _appRect.Left)
                    {
                        Win32.SetWindowPos(_appForm.Handle, Win32.HWND_TOPMOST, _appRect.Right - 178, _appRect.Top, 0, 0, Win32.SWP_NOSIZE | Win32.SWP_SHOWWINDOW);
                    }
                    else
                    {
                        Win32.SetWindowPos(_appForm.Handle, Win32.HWND_TOPMOST, _appRect.Left + 55, _appRect.Top, 0, 0, Win32.SWP_NOSIZE | Win32.SWP_SHOWWINDOW);
                    }
                }

                _preAppRect = _appRect;
            }
        }

        private void MuteAction()
        {
            Win32.GetWindowThreadProcessId(_handle, out var pID);
            var isMute = SoundController.GetApplicationMute((int)pID);

            if (pID == 0 || !isMute.HasValue)
                return;

            SoundController.SetApplicationMute((int)pID, !((bool)isMute));

            // focus process back
            Win32.SetWindowPos(_handle, new IntPtr(0), 0, 0, 0, 0, Win32.SWP_NOSIZE | Win32.SWP_NOMOVE | Win32.SWP_SHOWWINDOW);
        }

        private void UpdateFrmSize()
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void AddButton(MouseEventHandler handler)
        {
            // get an instance of IActiveMenu used to attach
            // buttons to the form
            IActiveMenu menu = ActiveMenu.GetInstance(_appForm);

            // define a new button
            menu.ToolTip.SetToolTip(_soundCtrlBtn, "Mute");
            _soundCtrlBtn.MouseUp += handler;

            // add the button to the menu
            menu.Items.Add(_soundCtrlBtn);
        }

        private void UpdateMuteIcon()
        {
            if (_handle == IntPtr.Zero)
            {
                return;
            }

            Win32.GetWindowThreadProcessId(_handle, out var pID);
            var isMute = SoundController.GetApplicationMute((int)pID);

            if (pID == 0 || !isMute.HasValue)
            {
                _soundCtrlBtn.Image = Properties.Resources.mute;
                _soundCtrlBtn.Enabled = false;
                _soundCtrlBtn.Hide();

                return;
            }

            if (!_soundCtrlBtn.Visible && _appForm.EnableAllButton)
            {
                _soundCtrlBtn.Show();
                _soundCtrlBtn.Enabled = true;
            }

            _soundCtrlBtn.Image = isMute.Value ? Properties.Resources.mute : Properties.Resources.unmute;
        }

        private IntPtr GetApplicationMouseIsOver()
        {
            Win32.POINT location;
            Win32.GetCursorPos(out location);

            IntPtr handle = Win32.WindowFromPoint(location);
            IntPtr windowParent = IntPtr.Zero;
            _buff.Remove(0, _buff.Length);

            while (handle != IntPtr.Zero)
            {
                windowParent = handle;
                handle = Win32.GetParent(handle);
            }

            handle = windowParent;

            if (Win32.GetWindowText((int)handle, _buff, 256) > 0)
            {
                // Avoid this application
                if (_buff.ToString().GetHashCode() == _applicationNameHash)
                    _thisAppHandle = handle;

                if ((int)handle != (int)_thisAppHandle)
                {
                    this._handle = handle;

                    // Show information
                    this._captionWindowLabel.Text = _buff.ToString();
                    this._idWindowLabel.Text = handle.ToString();

                    if (Win32.GetWindowRect(this._handle, out _appRect))
                    {
                        string tmpStr = _appRect.Top.ToString() + ", " + _appRect.Bottom.ToString() + ", " + _appRect.Left.ToString() + ", " + _appRect.Right.ToString();
                        this._windowSizeLabel.Text = tmpStr;

                        CalAndSetFormWindowPos(); // set window pos
                    }

                    if (_preHandle != this._handle)
                    {
                        _appForm.ActiveBtnAndStartTimer();
                        _preHandle = this._handle;
                    }
                }
            }

            return this._handle;
        }

        private void ShowNotifyIconIfNeeded()
        {
            if (!_isNotifyIconShowed)
            {
                _runBgNotifyIcon.ShowBalloonTip(2000);
                _isNotifyIconShowed = true;
            }
        }

        private void OnMainFormClosing(object sender, FormClosingEventArgs e)
        {
            if (_needExit)
            {
                _runBgNotifyIcon.Visible = false;
            }
            else
            {
                e.Cancel = true;
                this.Hide();
                ShowNotifyIconIfNeeded();
            }
        }

        private void OnUpdateTimerTick(object sender, System.EventArgs e)
        {
            GetApplicationMouseIsOver();
            UpdateMuteIcon();
            _appForm.UpdateBtnDisplay();
            UpdateFrmSize();
        }

        private void OnExitButtonClick(object sender, System.EventArgs e)
        {
            _needExit = true;
            this.Close();
        }

        private void OnMuteButtonClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                OnRunBgNotifyIconMouseClick(sender, e);
            }
            else
            {
                MuteAction();
            }
        }

        private void OnRunBgNotifyIconMouseClick(object sender, MouseEventArgs e)
        {
            if (this.Visible)
            {
                this.Hide();
            }
            else
            {
                Win32.ShowWindow(this.Handle, Win32.SW_RESTORE);
                this.Visible = true;
            }
        }

        private void OnRefreshButtonClick(object sender, EventArgs e)
        {
            _needExit = true;
            Application.Restart();
            Process.GetCurrentProcess().Kill();
        }

        private void OnlogoPictureBoxClick(object sender, EventArgs e)
        {
            _aboutForm.Show();
        }

        private void OnOkButtonClick(object sender, EventArgs e)
        {
            this.Hide();
            ShowNotifyIconIfNeeded();
        }

        private void OnGlobalKeyboardHookKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            foreach (bool state in _globalKeyboardHook.HookedKeysState)
            {
                if (state == false)
                {
                    return;
                }
            }

            // mute or unmute
            MuteAction();
        }

        private void OnGlobalKeyboardHookKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
