using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OpenAdhanForWindowsX
{
    public partial class Form1 : Form
    {
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MINIMIZE = 0xF020;
        private const int WM_DWMCOMPOSITIONCHANGED = 0x031E;
        private const int WS_MINIMIZEBOX = 0x20000;
        private const int GWL_STYLE = -16;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        bool closeOnExit = false;
        ContextMenu contextMenu1;
        MenuItem exitMenuItem;
        MenuItem openMenuItem;
        MenuItem settingsMenuItem;
        PrayerTimesControl pti = PrayerTimesControl.Instance;
        private SunMoonAnimation sunMoonAnimation;

        private RegistrySettingsHandler registryHandler;

        public Form1()
        {
            InitializeComponent();
            AddNotifyIconContextMenu();

            sunMoonAnimation = new SunMoonAnimation(ovalShape2, pti);
            sunMoonAnimation.PrayerTimesUpdated += SunMoonAnimation_PrayerTimesUpdated;
            sunMoonAnimation.Start();
            //sunMoonAnimation.ToggleDebugMode(true);

            CustomizeMenuStrip();

            // Enable Drag on Click for the form and relevant children.
            this.menuStrip1.MouseDown += Drag_MouseDown;
            this.menuStrip1.MouseMove += menuStrip1_MouseMove;
            this.ovalShape1.MouseDown += Drag_MouseDown;
            this.MouseDown += Drag_MouseDown;
            this.pictureBox1.MouseDown += Drag_MouseDown;
            this.lineShape1.MouseDown += Drag_MouseDown;

            registryHandler = new RegistrySettingsHandler(false);
            if (registryHandler.SafeLoadBoolRegistryValue(RegistrySettingsHandler.bismillahOnStartupKey))
            {
                PrayerTimesControl pti = PrayerTimesControl.Instance;
                pti.playAdhanFile(pti.getDefaultBismillahFilePath());
            }
            if (registryHandler.SafeLoadBoolRegistryValue(RegistrySettingsHandler.initialInstallFlagKey))
            {
                this.Show();
                var settings = new Settings(this);
                settings.Show();
                registryHandler.SaveRegistryValue(RegistrySettingsHandler.initialInstallFlagKey, "0", "int");
            }

            (int, int) savedPosition = registryHandler.LoadWindowPosition();
            Point savedPoint = new Point(savedPosition.Item1, savedPosition.Item2);
            if (savedPoint != Point.Empty)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = savedPoint;
            }
        }

        private void SunMoonAnimation_PrayerTimesUpdated(object sender, EventArgs e)
        {
            // Update UI elements that display prayer times
            updatePrayerTimesDisplay();
        }


        private void AddNotifyIconContextMenu()
        {
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.exitMenuItem = new MenuItem();
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += ExitToolStripMenuItem_Click;
            this.exitMenuItem.Index = 0;
            this.settingsMenuItem = new MenuItem();
            this.settingsMenuItem.Text = "Settings";
            this.settingsMenuItem.Index = 1;
            this.settingsMenuItem.Click += SettingsToolStripMenuItem_Click_1;
            this.openMenuItem = new MenuItem();
            this.openMenuItem.Text = "Open";
            this.openMenuItem.Index = 2;
            this.openMenuItem.Click += MenuItem_openMenuItem;

            this.contextMenu1.MenuItems.Add(this.openMenuItem);
            this.contextMenu1.MenuItems.Add(this.settingsMenuItem);
            this.contextMenu1.MenuItems.Add(this.exitMenuItem);

            this.notifyIcon.ContextMenu = this.contextMenu1;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Don't hide to system tray when minimized - let it stay on taskbar
        }

        private void MenuItem_openMenuItem(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = true;
            this.timer1.Start();
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate(); // Brings the form to the front.
            notifyIcon.Visible = true;
            this.timer1.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(this.closeOnExit)
            {
                this.registryHandler.SaveWindowPosition(this.Location.X, this.Location.Y);
                Application.Exit();
            } else
            {
                e.Cancel = true;
                this.Hide();
                this.timer1.Stop();
            }

        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.closeOnExit = true;
            this.Close();
            Application.Exit();
        }

        private void NotifyIcon_MouseMove(object sender, MouseEventArgs e)
        {
            this.updateTimeToNextPrayerNotification();
        }


        private void SettingsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var settings = new Settings(this);
            settings.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Enable minimize box in window style to allow Win+Down keyboard shortcut
            int style = GetWindowLong(this.Handle, GWL_STYLE);
            SetWindowLong(this.Handle, GWL_STYLE, style | WS_MINIMIZEBOX);
            
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Tuple<PrayerInfo, PrayerInfo> prayerInfo = pti.getNextPrayerNotification();
            PrayerInfo nextPrayer = prayerInfo.Item1;
            PrayerInfo currentPrayer = prayerInfo.Item2;

            // Next Prayer
            label5.Text = nextPrayer.Name;
            label3.Text = nextPrayer.TimeTo.ToString("hh\\:mm\\:ss");

            // Current Prayer
            label2.Text = currentPrayer.Name;
            label8.Text = currentPrayer.TimeSince.ToString("hh\\:mm\\:ss");
        }

        public void SetBold(string nextPrayer)
        {
            if (nextPrayer.Equals("Shurook"))
            {
                ResetLabelBoldness();
                FajrTitleLabel.Font = new Font(FajrTitleLabel.Font.FontFamily, FajrTitleLabel.Font.Size, FontStyle.Bold);
                FajrValueLabel.Font = new Font(FajrValueLabel.Font.FontFamily, FajrValueLabel.Font.Size, FontStyle.Bold);

                ShurookTitleLabel.Refresh();
                ShurookValueLabel.Refresh();
            }
            if (nextPrayer.Equals("Dhuhr"))
            {
                ResetLabelBoldness();
                ShurookTitleLabel.Font = new Font(ShurookTitleLabel.Font.FontFamily, ShurookTitleLabel.Font.Size, FontStyle.Bold);
                ShurookValueLabel.Font = new Font(ShurookValueLabel.Font.FontFamily, ShurookValueLabel.Font.Size, FontStyle.Bold);

                DhuhrTitleLabel.Refresh();
                DhuhrValueLabel.Refresh();
            }
            if (nextPrayer.Equals("Asr"))
            {
                ResetLabelBoldness();
                DhuhrTitleLabel.Font = new Font(DhuhrTitleLabel.Font.FontFamily, DhuhrTitleLabel.Font.Size, FontStyle.Bold);
                DhuhrValueLabel.Font = new Font(DhuhrValueLabel.Font.FontFamily, DhuhrValueLabel.Font.Size, FontStyle.Bold);

                AsrTitleLabel.Refresh();
                AsrValueLabel.Refresh();

            }
            if (nextPrayer.Equals("Maghrib"))
            {
                ResetLabelBoldness();
                AsrTitleLabel.Font = new Font(AsrTitleLabel.Font.FontFamily, AsrTitleLabel.Font.Size, FontStyle.Bold);
                AsrValueLabel.Font = new Font(AsrValueLabel.Font.FontFamily, AsrValueLabel.Font.Size, FontStyle.Bold);

                MaghribTitleLabel.Refresh();
                MahribValueLabel.Refresh();
            }
            if (nextPrayer.Equals("Isha"))
            {
                ResetLabelBoldness();
                MaghribTitleLabel.Font = new Font(MaghribTitleLabel.Font.FontFamily, MaghribTitleLabel.Font.Size, FontStyle.Bold);
                MahribValueLabel.Font = new Font(MahribValueLabel.Font.FontFamily, MahribValueLabel.Font.Size, FontStyle.Bold);

                IshaTitleLabel.Refresh();
                IshaValueLabel.Refresh();
            }
            if (nextPrayer.Equals("Fajr"))
            {
                ResetLabelBoldness();
                IshaTitleLabel.Font = new Font(IshaTitleLabel.Font.FontFamily, IshaTitleLabel.Font.Size, FontStyle.Bold);
                IshaValueLabel.Font = new Font(IshaValueLabel.Font.FontFamily, IshaValueLabel.Font.Size, FontStyle.Bold);

                FajrTitleLabel.Refresh();
                FajrValueLabel.Refresh();
            }
        }
        private void ResetLabelBoldness()
        {
            FajrTitleLabel.Font = new Font(FajrTitleLabel.Font.FontFamily, FajrTitleLabel.Font.Size, FontStyle.Regular);
            FajrValueLabel.Font = new Font(FajrValueLabel.Font.FontFamily, FajrValueLabel.Font.Size, FontStyle.Regular);
            ShurookTitleLabel.Font = new Font(ShurookTitleLabel.Font.FontFamily, ShurookTitleLabel.Font.Size, FontStyle.Regular);
            ShurookValueLabel.Font = new Font(ShurookValueLabel.Font.FontFamily, ShurookValueLabel.Font.Size, FontStyle.Regular);
            DhuhrTitleLabel.Font = new Font(DhuhrTitleLabel.Font.FontFamily, DhuhrTitleLabel.Font.Size, FontStyle.Regular);
            DhuhrValueLabel.Font = new Font(DhuhrValueLabel.Font.FontFamily, DhuhrValueLabel.Font.Size, FontStyle.Regular);
            AsrTitleLabel.Font = new Font(AsrTitleLabel.Font.FontFamily, AsrTitleLabel.Font.Size, FontStyle.Regular);
            AsrValueLabel.Font = new Font(AsrValueLabel.Font.FontFamily, AsrValueLabel.Font.Size, FontStyle.Regular);
            MaghribTitleLabel.Font = new Font(MaghribTitleLabel.Font.FontFamily, MaghribTitleLabel.Font.Size, FontStyle.Regular);
            MahribValueLabel.Font = new Font(MahribValueLabel.Font.FontFamily, MahribValueLabel.Font.Size, FontStyle.Regular);
            IshaTitleLabel.Font = new Font(IshaTitleLabel.Font.FontFamily, IshaTitleLabel.Font.Size, FontStyle.Regular);
            IshaValueLabel.Font = new Font(IshaValueLabel.Font.FontFamily, IshaValueLabel.Font.Size, FontStyle.Regular);     
        }

        private void stopAdhanPlaybackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrayerTimesControl pti = PrayerTimesControl.Instance;
            pti.StopAdhan();
        }

        public void SendPrayerNotification(string prayer, DateTime prayerTime)
        {
            this.notifyIcon.BalloonTipText = $"It is now {prayer} ({prayerTime.ToShortTimeString()})";
            this.notifyIcon.ShowBalloonTip(5000);
        }

        private void AboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var about = new About();
            about.Show();
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void minimizeButton_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void minimizeButton_Paint(object sender, PaintEventArgs e)
        {
            Button btn = (Button)sender;
            
            // Draw the minimize line (horizontal line in the middle-bottom area)
            using (Pen pen = new Pen(Color.Black, 2))
            {
                int lineWidth = 10;
                int x = (btn.Width - lineWidth) / 2;
                int y = btn.Height / 2 + 1;
                e.Graphics.DrawLine(pen, x, y, x + lineWidth, y);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SYSCOMMAND)
            {
                int command = m.WParam.ToInt32() & 0xFFF0;
                if (command == SC_MINIMIZE)
                {
                    this.WindowState = FormWindowState.Minimized;
                    m.Result = IntPtr.Zero;
                    return;
                }
            }
            base.WndProc(ref m);
        }

        private void Drag_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void menuStrip1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Focused)
            {
                Focus();
            }
        }
    }

}
