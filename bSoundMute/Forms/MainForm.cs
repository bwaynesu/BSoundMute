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
        private bool isNotifyIconShowed_ = false;

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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            captionWindowLabel = new Label();
            IDWindowLabel = new Label();
            exitButton = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            windowSizeLabel = new Label();
            label4 = new Label();
            notifyIcon1 = new NotifyIcon(components);
            pictureBox1 = new PictureBox();
            setupButton = new Button();
            versionLabel = new Label();
            versionValuelabel = new Label();
            copyrightValueLabel = new Label();
            okButton = new Button();
            label5 = new Label();
            Refreshbutton = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            //
            // label1
            //
            label1.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 136);
            label1.Location = new System.Drawing.Point(125, 7);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(174, 20);
            label1.TabIndex = 0;
            label1.Text = "Active Window Detail";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // label2
            //
            label2.Location = new System.Drawing.Point(19, 49);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(115, 20);
            label2.TabIndex = 1;
            label2.Text = "Window Title : ";
            //
            // label3
            //
            label3.Location = new System.Drawing.Point(19, 83);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(115, 19);
            label3.TabIndex = 2;
            label3.Text = "Window Handle :";
            //
            // captionWindowLabel
            //
            captionWindowLabel.Location = new System.Drawing.Point(112, 49);
            captionWindowLabel.Name = "captionWindowLabel";
            captionWindowLabel.Size = new System.Drawing.Size(304, 34);
            captionWindowLabel.TabIndex = 3;
            //
            // IDWindowLabel
            //
            IDWindowLabel.Location = new System.Drawing.Point(134, 83);
            IDWindowLabel.Name = "IDWindowLabel";
            IDWindowLabel.Size = new System.Drawing.Size(269, 19);
            IDWindowLabel.TabIndex = 4;
            //
            // exitButton
            //
            exitButton.FlatStyle = FlatStyle.System;
            exitButton.Location = new System.Drawing.Point(248, 125);
            exitButton.Name = "exitButton";
            exitButton.Size = new System.Drawing.Size(70, 29);
            exitButton.TabIndex = 2;
            exitButton.Text = "EXIT";
            exitButton.Click += exitButton_Click;
            //
            // timer1
            //
            timer1.Enabled = true;
            timer1.Interval = 250;
            timer1.Tick += timer1_Tick;
            //
            // windowSizeLabel
            //
            windowSizeLabel.Location = new System.Drawing.Point(134, 103);
            windowSizeLabel.Name = "windowSizeLabel";
            windowSizeLabel.Size = new System.Drawing.Size(269, 21);
            windowSizeLabel.TabIndex = 7;
            //
            // label4
            //
            label4.Location = new System.Drawing.Point(19, 103);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(115, 21);
            label4.TabIndex = 6;
            label4.Text = "Window Size :";
            //
            // notifyIcon1
            //
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipText = "程式於背景執行";
            notifyIcon1.BalloonTipTitle = "BSoundMute";
            notifyIcon1.Icon = (System.Drawing.Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Visible = true;
            notifyIcon1.MouseClick += notifyIcon1_MouseClick;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseClick;
            //
            // pictureBox1
            //
            pictureBox1.Cursor = Cursors.Hand;
            pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new System.Drawing.Point(353, 83);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(66, 79);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            //
            // setupButton
            //
            setupButton.FlatStyle = FlatStyle.System;
            setupButton.Location = new System.Drawing.Point(176, 158);
            setupButton.Name = "setupButton";
            setupButton.Size = new System.Drawing.Size(69, 29);
            setupButton.TabIndex = 1;
            setupButton.Text = "SETUP";
            setupButton.Visible = false;
            setupButton.Click += setupbutton_Click;
            //
            // versionLabel
            //
            versionLabel.Location = new System.Drawing.Point(0, 167);
            versionLabel.Name = "versionLabel";
            versionLabel.Size = new System.Drawing.Size(66, 21);
            versionLabel.TabIndex = 10;
            versionLabel.Text = "Version : ";
            versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // versionValuelabel
            //
            versionValuelabel.Font = new System.Drawing.Font("PMingLiU", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
            versionValuelabel.Location = new System.Drawing.Point(58, 167);
            versionValuelabel.Name = "versionValuelabel";
            versionValuelabel.Size = new System.Drawing.Size(91, 21);
            versionValuelabel.TabIndex = 11;
            versionValuelabel.Text = "1.0.0.0";
            versionValuelabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // copyrightValueLabel
            //
            copyrightValueLabel.Location = new System.Drawing.Point(314, 167);
            copyrightValueLabel.Name = "copyrightValueLabel";
            copyrightValueLabel.Size = new System.Drawing.Size(125, 21);
            copyrightValueLabel.TabIndex = 12;
            copyrightValueLabel.Text = "©  2015 bwaynesu";
            copyrightValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // okButton
            //
            okButton.FlatStyle = FlatStyle.System;
            okButton.Location = new System.Drawing.Point(104, 125);
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(69, 29);
            okButton.TabIndex = 0;
            okButton.Text = "OK";
            okButton.Click += okButton_Click;
            //
            // label5
            //
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(156, 171);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(105, 15);
            label5.TabIndex = 13;
            label5.Text = "Hotkeys : B + LCtrl";
            //
            // Refreshbutton
            //
            Refreshbutton.FlatStyle = FlatStyle.System;
            Refreshbutton.Location = new System.Drawing.Point(176, 125);
            Refreshbutton.Name = "Refreshbutton";
            Refreshbutton.Size = new System.Drawing.Size(69, 29);
            Refreshbutton.TabIndex = 14;
            Refreshbutton.Text = "Refresh";
            Refreshbutton.Click += Refreshbutton_Click;
            //
            // MainForm
            //
            AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            ClientSize = new System.Drawing.Size(442, 196);
            Controls.Add(Refreshbutton);
            Controls.Add(label5);
            Controls.Add(okButton);
            Controls.Add(copyrightValueLabel);
            Controls.Add(versionValuelabel);
            Controls.Add(versionLabel);
            Controls.Add(setupButton);
            Controls.Add(pictureBox1);
            Controls.Add(windowSizeLabel);
            Controls.Add(label4);
            Controls.Add(exitButton);
            Controls.Add(IDWindowLabel);
            Controls.Add(captionWindowLabel);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "BSoundMute";
            TopMost = true;
            FormClosing += MainForm_FormClosing;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
            if (trulyExit_)
            {
                notifyIcon1.Visible = false;
            }
            else
            {
                e.Cancel = true;
                this.Hide();
                ShowNotifyIconIfNeeded();
            }
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
            ShowNotifyIconIfNeeded();
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

        private void ShowNotifyIconIfNeeded()
        {
            if (!isNotifyIconShowed_)
            {
                notifyIcon1.ShowBalloonTip(2000);
                isNotifyIconShowed_ = true;
            }
        }
    }
}