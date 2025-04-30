using bSoundMute.Controls;
using bSoundMute.Utils;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace bSoundMute.Forms
{
    public partial class AppForm : Form
    {
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int en);

        private int counter_ = 2;

        public bool enableAllButton_ = true;

        public AppForm()
        {
            InitializeComponent();
            bool enableComposition = CheckComposition();
        }

        public bool CheckComposition()
        {
            int en = 0;
            Win32.MARGINS mg = new Win32.MARGINS();
            mg.m_Buttom = -1;
            mg.m_Left = -1;
            mg.m_Right = -1;
            mg.m_Top = -1;

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (counter_ > 0)
            {
                --counter_;

                if (counter_ <= 0)
                {
                    if (enableAllButton_)
                    {
                        IActiveMenu menu = ActiveMenu.GetInstance(this);
                        for (int i = 0; i < menu.Items.Count; ++i)
                        {
                            menu.Items[i].Hide();
                            menu.Items[i].Enabled = false;
                        }
                    }
                    enableAllButton_ = false;
                }
            }
        }

        public void ActiveBtnAndStartTimer()
        {
            if (!enableAllButton_)
            {
                IActiveMenu menu = ActiveMenu.GetInstance(this);
                for (int i = 0; i < menu.Items.Count; ++i)
                {
                    menu.Items[i].Show();
                    menu.Items[i].Enabled = true;
                }
                enableAllButton_ = true;
            }
            counter_ = 2;
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

        public void UpdateBtnDisplay()
        {
            if (IsMouseEnter())
            {
                ActiveBtnAndStartTimer();
            }
        }
    }
}