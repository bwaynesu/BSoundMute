using bSoundMute.Controls;
using bSoundMute.Utils;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace bSoundMute.Forms
{
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label captionWindowLabel;
        private System.Windows.Forms.Label IDWindowLabel;
        private System.Windows.Forms.Label windowSizeLabel;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.IContainer components;
        private NotifyIcon notifyIcon1;

        private IntPtr thisAppHandle_; // this application handle id
        private IntPtr handle_, preHandle_;
        private int applicationNameHash_;
        private StringBuilder buff_ = new StringBuilder(256);
        private Win32.RECT preAppRect_;
        private Win32.RECT appRect_;
        private AppForm appForm_ = new AppForm();
        private SoundControlButton scButton_ = new SoundControlButton();
        private PictureBox pictureBox1;
        private Label versionLabel;
        private Label versionValuelabel;
        private Label copyrightValueLabel;
        private Button okButton;
        private bool trulyExit_ = false;
        private AboutForm aboutForm_ = new AboutForm();
        private Button setupButton;
        private Label label5;
        private Button Refreshbutton;
        private GlobalKeyboardHook gkh_ = new GlobalKeyboardHook();
        private bool isAutoHide_ = true;
        private Stopwatch stopWatch_ = new Stopwatch();

        // 添加用於滑鼠位置檢查節流的變數
        private int mousePollCounter = 0;

        private const int MOUSE_POLL_INTERVAL = 4; // 僅每 4 次計時器刻度檢查一次

        public MainForm()
        {
            // Add mute button
            AddButton(muteButton_Click);

            InitializeComponent();
            applicationNameHash_ = this.Text.GetHashCode();

            InitialGlobalHook();

#if !DEBUG
      label3.Visible = false;
      label4.Visible = false;
      IDWindowLabel.Visible = false;
      windowSizeLabel.Visible = false;
#endif

            versionValuelabel.Text = Application.ProductVersion;
            copyrightValueLabel.Text = "© 2015 " + FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).CompanyName;

            appForm_.Show();
            isAutoHide_ = true;
            stopWatch_.Start();
        }

        private void InitialGlobalHook()
        {
            gkh_.AddKey(Properties.Settings.Default.CombineKey1);
            gkh_.AddKey(Properties.Settings.Default.CombineKey2);
            gkh_.KeyDown += new KeyEventHandler(gkh_KeyDown);
            gkh_.KeyUp += new KeyEventHandler(gkh_KeyUp);
        }

        protected override void Dispose(bool disposing)
        {
            // 確保鍵盤鉤子在應用退出時正確卸載，防止資源洩漏
            if (gkh_ != null)
            {
                gkh_.unhook();
            }

            notifyIcon1.Visible = false;
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.captionWindowLabel = new System.Windows.Forms.Label();
            this.IDWindowLabel = new System.Windows.Forms.Label();
            this.exitButton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.windowSizeLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.setupButton = new System.Windows.Forms.Button();
            this.versionLabel = new System.Windows.Forms.Label();
            this.versionValuelabel = new System.Windows.Forms.Label();
            this.copyrightValueLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.Refreshbutton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            //
            // label1
            //
            this.label1.Font = new System.Drawing.Font("PMingLiU", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(104, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Active Window Detail";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // label2
            //
            this.label2.Location = new System.Drawing.Point(16, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Window Title : ";
            //
            // label3
            //
            this.label3.Location = new System.Drawing.Point(16, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Window Handle :";
            //
            // captionWindowLabel
            //
            this.captionWindowLabel.Location = new System.Drawing.Point(93, 46);
            this.captionWindowLabel.Name = "captionWindowLabel";
            this.captionWindowLabel.Size = new System.Drawing.Size(254, 32);
            this.captionWindowLabel.TabIndex = 3;
            //
            // IDWindowLabel
            //
            this.IDWindowLabel.Location = new System.Drawing.Point(112, 78);
            this.IDWindowLabel.Name = "IDWindowLabel";
            this.IDWindowLabel.Size = new System.Drawing.Size(224, 18);
            this.IDWindowLabel.TabIndex = 4;
            //
            // exitButton
            //
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.exitButton.Location = new System.Drawing.Point(207, 117);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(58, 27);
            this.exitButton.TabIndex = 2;
            this.exitButton.Text = "EXIT";
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            //
            // timer1
            //
            this.timer1.Enabled = true;
            this.timer1.Interval = 250; // 從 25 毫秒改為 250 毫秒以減少 CPU 使用率
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            //
            // windowSizeLabel
            //
            this.windowSizeLabel.Location = new System.Drawing.Point(112, 97);
            this.windowSizeLabel.Name = "windowSizeLabel";
            this.windowSizeLabel.Size = new System.Drawing.Size(224, 19);
            this.windowSizeLabel.TabIndex = 7;
            //
            // label4
            //
            this.label4.Location = new System.Drawing.Point(16, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 19);
            this.label4.TabIndex = 6;
            this.label4.Text = "Window Size :";
            //
            // notifyIcon1
            //
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "程式於背景執行";
            this.notifyIcon1.BalloonTipTitle = "bSoundMute";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            //
            // pictureBox1
            //
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = global::bSoundMute.Properties.Resources.Ike_2;
            this.pictureBox1.Location = new System.Drawing.Point(294, 78);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(55, 74);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            //
            // setupButton
            //
            this.setupButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.setupButton.Location = new System.Drawing.Point(147, 148);
            this.setupButton.Name = "setupButton";
            this.setupButton.Size = new System.Drawing.Size(57, 27);
            this.setupButton.TabIndex = 1;
            this.setupButton.Text = "SETUP";
            this.setupButton.Visible = false;
            this.setupButton.Click += new System.EventHandler(this.setupbutton_Click);
            //
            // versionLabel
            //
            this.versionLabel.Location = new System.Drawing.Point(0, 157);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(55, 19);
            this.versionLabel.TabIndex = 10;
            this.versionLabel.Text = "Version : ";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // versionValuelabel
            //
            this.versionValuelabel.Font = new System.Drawing.Font("PMingLiU", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.versionValuelabel.Location = new System.Drawing.Point(48, 157);
            this.versionValuelabel.Name = "versionValuelabel";
            this.versionValuelabel.Size = new System.Drawing.Size(76, 19);
            this.versionValuelabel.TabIndex = 11;
            this.versionValuelabel.Text = "1.0.0.0";
            this.versionValuelabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // copyrightValueLabel
            //
            this.copyrightValueLabel.Location = new System.Drawing.Point(262, 157);
            this.copyrightValueLabel.Name = "copyrightValueLabel";
            this.copyrightValueLabel.Size = new System.Drawing.Size(104, 19);
            this.copyrightValueLabel.TabIndex = 12;
            this.copyrightValueLabel.Text = "©  2015 bWayneSu";
            this.copyrightValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // okButton
            //
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okButton.Location = new System.Drawing.Point(87, 117);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(57, 27);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(130, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "Hotkeys : B + LCtrl";
            //
            // Refreshbutton
            //
            this.Refreshbutton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Refreshbutton.Location = new System.Drawing.Point(147, 117);
            this.Refreshbutton.Name = "Refreshbutton";
            this.Refreshbutton.Size = new System.Drawing.Size(57, 27);
            this.Refreshbutton.TabIndex = 14;
            this.Refreshbutton.Text = "Refresh";
            this.Refreshbutton.Click += new System.EventHandler(this.Refreshbutton_Click);
            //
            // MainForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 15);
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(369, 174);
            this.Controls.Add(this.Refreshbutton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.copyrightValueLabel);
            this.Controls.Add(this.versionValuelabel);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.setupButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.windowSizeLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.IDWindowLabel);
            this.Controls.Add(this.captionWindowLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "bSoundMute";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion Windows Form Designer generated code

        [STAThread]
        private static void Main()
        {
            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, Application.ProductName, out createdNew))
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

        // no use
        private void GetActiveWindow()
        {
            IntPtr handle;
            buff_.Remove(0, buff_.Length);

            handle = Win32.GetForegroundWindow();

            if (Win32.GetWindowText((int)handle, buff_, 256) > 0)
            {
                // avoid this application
                if (buff_.ToString().GetHashCode() == applicationNameHash_)
                    thisAppHandle_ = handle;

                if ((int)handle != (int)thisAppHandle_)
                {
                    handle_ = handle;

                    // show information
                    this.captionWindowLabel.Text = buff_.ToString();
                    this.IDWindowLabel.Text = handle.ToString();
                    if (Win32.GetWindowRect(handle_, out appRect_))
                    {
                        string tmpStr = appRect_.Top.ToString() + ", " + appRect_.Bottom.ToString() + ", " + appRect_.Left.ToString() + ", " + appRect_.Right.ToString();
                        this.windowSizeLabel.Text = tmpStr;

                        CalAndSetFormWindowPos(); // set window pos
                    }

                    if (preHandle_ != handle_)
                    {
                        appForm_.ActiveBtnAndStartTimer();
                        preHandle_ = handle_;
                    }
                }
            }
        }

        private void CalAndSetFormWindowPos()
        {
            if (appRect_.Top != preAppRect_.Top || appRect_.Bottom != preAppRect_.Bottom || appRect_.Left != preAppRect_.Left || appRect_.Right != preAppRect_.Right)
            {
                if (Win32.DwmIsCompositionEnabled || Application.RenderWithVisualStyles) // not traditional theme
                {
                    if (appRect_.Right - 333 >= appRect_.Left)
                    {
                        Win32.SetWindowPos(appForm_.Handle, Win32.HWND_TOPMOST, appRect_.Right - 278, appRect_.Top + 5, 0, 0, Win32.SWP_NOSIZE | Win32.SWP_SHOWWINDOW);
                    }
                    else
                    {
                        Win32.SetWindowPos(appForm_.Handle, Win32.HWND_TOPMOST, appRect_.Left + 55, appRect_.Top + 5, 0, 0, Win32.SWP_NOSIZE | Win32.SWP_SHOWWINDOW);
                    }
                }
                else
                {
                    if (appRect_.Right - 233 >= appRect_.Left)
                    {
                        Win32.SetWindowPos(appForm_.Handle, Win32.HWND_TOPMOST, appRect_.Right - 178, appRect_.Top, 0, 0, Win32.SWP_NOSIZE | Win32.SWP_SHOWWINDOW);
                    }
                    else
                    {
                        Win32.SetWindowPos(appForm_.Handle, Win32.HWND_TOPMOST, appRect_.Left + 55, appRect_.Top, 0, 0, Win32.SWP_NOSIZE | Win32.SWP_SHOWWINDOW);
                    }
                }

                preAppRect_ = appRect_;
            }
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            // GetActiveWindow();

            // 只在每 MOUSE_POLL_INTERVAL 次計時器刻度時檢查滑鼠位置，以減少 CPU 使用率
            mousePollCounter++;
            if (mousePollCounter >= MOUSE_POLL_INTERVAL)
            {
                GetApplicationMouseIsOver();
                mousePollCounter = 0;
            }

            UpdateMuteIcon();
            appForm_.UpdateBtnDisplay();
            UpdateFrmSize();

            if (isAutoHide_ && stopWatch_.IsRunning && stopWatch_.Elapsed.Seconds >= 3.0f)
            {
                isAutoHide_ = false;
                stopWatch_.Stop();
                this.Hide();
                notifyIcon1.ShowBalloonTip(3000);
            }
        }

        private void exitButton_Click(object sender, System.EventArgs e)
        {
            trulyExit_ = true;
            this.Close();
        }

        private void muteButton_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                notifyIcon1_MouseClick(sender, e);
            }
            else
            {
                muteAction();
            }
        }

        private void muteAction()
        {
            uint pID;
            Win32.GetWindowThreadProcessId(handle_, out pID);
            bool? isMute = SoundController.GetApplicationMute((int)pID);

            if (pID == 0 || !isMute.HasValue)
                return;

            SoundController.SetApplicationMute((int)pID, !((bool)isMute));

            // focus process back
            Win32.SetWindowPos(handle_, new IntPtr(0), 0, 0, 0, 0, Win32.SWP_NOSIZE | Win32.SWP_NOMOVE | Win32.SWP_SHOWWINDOW);
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
            IActiveMenu menu = ActiveMenu.GetInstance(appForm_);

            // define a new button
            menu.ToolTip.SetToolTip(scButton_, "Mute");
            scButton_.MouseUp += handler;

            // add the button to the menu
            menu.Items.Add(scButton_);
        }

        private void UpdateMuteIcon()
        {
            if (handle_ == IntPtr.Zero)
                return;

            uint pID;
            Win32.GetWindowThreadProcessId(handle_, out pID);
            bool? isMute = SoundController.GetApplicationMute((int)pID);

            if (pID == 0 || !isMute.HasValue)
            {
                scButton_.Image = Properties.Resources.mute;
                scButton_.Enabled = false;
                scButton_.Hide();
                return;
            }
            if (!scButton_.Visible && appForm_.enableAllButton_)
            {
                scButton_.Show();
                scButton_.Enabled = true;
            }

            if (isMute.HasValue && isMute == true)
                scButton_.Image = Properties.Resources.mute;
            else
                scButton_.Image = Properties.Resources.unmute;
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
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

        private bool IsMouseEnterActiveArea()
        {
            return false;
        }

        public IntPtr GetApplicationMouseIsOver()
        {
            Win32.POINT location;
            Win32.GetCursorPos(out location);

            IntPtr handle = Win32.WindowFromPoint(location);
            IntPtr windowParent = IntPtr.Zero;
            buff_.Remove(0, buff_.Length);

            while (handle != IntPtr.Zero)
            {
                windowParent = handle;
                handle = Win32.GetParent(handle);
            }
            handle = windowParent;

            if (Win32.GetWindowText((int)handle, buff_, 256) > 0)
            {
                // avoid this application
                if (buff_.ToString().GetHashCode() == applicationNameHash_)
                    thisAppHandle_ = handle;

                if ((int)handle != (int)thisAppHandle_)
                {
                    handle_ = handle;

                    // show information
                    this.captionWindowLabel.Text = buff_.ToString();
                    this.IDWindowLabel.Text = handle.ToString();
                    if (Win32.GetWindowRect(handle_, out appRect_))
                    {
                        string tmpStr = appRect_.Top.ToString() + ", " + appRect_.Bottom.ToString() + ", " + appRect_.Left.ToString() + ", " + appRect_.Right.ToString();
                        this.windowSizeLabel.Text = tmpStr;

                        CalAndSetFormWindowPos(); // set window pos
                    }

                    if (preHandle_ != handle_)
                    {
                        appForm_.ActiveBtnAndStartTimer();
                        preHandle_ = handle_;
                    }
                }
            }

            return handle_;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!trulyExit_)
            {
                e.Cancel = true;
                trulyExit_ = false;
                this.Hide();
                notifyIcon1.ShowBalloonTip(3000);
            }
            notifyIcon1.Visible = false;
        }

        private void Refreshbutton_Click(object sender, EventArgs e)
        {
            trulyExit_ = true;
            Application.Restart();
            Process.GetCurrentProcess().Kill();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            aboutForm_.Show();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            notifyIcon1.ShowBalloonTip(3000);
        }

        private void gkh_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            foreach (bool state in gkh_.HookedKeysState)
            {
                if (state == false)
                {
                    return;
                }
            }

            // mute or unmute
            muteAction();
        }

        private void setupbutton_Click(object sender, EventArgs e)
        {
        }
    }
}