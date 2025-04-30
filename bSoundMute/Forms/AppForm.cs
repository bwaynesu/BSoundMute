using bSoundMute.Controls;
using bSoundMute.Utils;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace bSoundMute.Forms
{
    public partial class AppForm : Form
    {
        private readonly Stopwatch hideItemsTimer_ = new Stopwatch();

        public bool EnableAllButton { get; private set; } = true;

        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int en);

        public AppForm()
        {
            InitializeComponent();
            CheckComposition();
        }

        public bool CheckComposition()
        {
            var en = 0;
            var mg = new Win32.MARGINS
            {
                m_Buttom = -1,
                m_Left = -1,
                m_Right = -1,
                m_Top = -1
            };

            if (System.Environment.OSVersion.Version.Major >= 6) //Make sure you are not on a legacy OS
            {
                //Check if the desktop composition is enabled
                DwmIsCompositionEnabled(ref en);

                if (en > 0)
                {
                    Win32.DwmExtendFrameIntoClientArea(this.Handle, ref mg);
                    return true;
                }
                else
                {
                    Debug.WriteLine("Desktop Composition is Disabled!");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Please run this on Windows Vista or newer.");
                return false;
            }
        }

        public void ActiveBtnAndStartTimer()
        {
            if (!EnableAllButton)
            {
                EnableItems(true);
                EnableAllButton = true;
            }

            hideItemsTimer_.Restart();
        }

        public void UpdateBtnDisplay()
        {
            if (IsMouseEnter())
            {
                ActiveBtnAndStartTimer();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!hideItemsTimer_.IsRunning)
            {
                return;
            }

            if (hideItemsTimer_.ElapsedMilliseconds < 2500)
            {
                return;
            }

            if (EnableAllButton)
            {
                EnableItems(false);
                EnableAllButton = false;
            }

            hideItemsTimer_.Stop();
            hideItemsTimer_.Reset();
        }

        private bool IsMouseEnter()
        {
            this.Cursor = new Cursor(Cursor.Current.Handle);
            if (Cursor.Position.X >= this.Left && Cursor.Position.X <= this.Right)
            {
                if (Cursor.Position.Y >= this.Top && Cursor.Position.Y <= this.Bottom)
                {
                    return true;
                }
            }
            return false;
        }

        private void EnableItems(bool enable)
        {
            var menu = ActiveMenu.GetInstance(this);

            for (var i = 0; i < menu.Items.Count; ++i)
            {
                if (enable)
                {
                    menu.Items[i].Show();
                }
                else
                {
                    menu.Items[i].Hide();
                }

                menu.Items[i].Enabled = enable;
            }
        }
    }
}