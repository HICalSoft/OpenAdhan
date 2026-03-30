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
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

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
            this.DoubleBuffered = true;
            AddNotifyIconContextMenu();

            sunMoonAnimation = new SunMoonAnimation(ovalShape2, pti);
            sunMoonAnimation.PrayerTimesUpdated += SunMoonAnimation_PrayerTimesUpdated;
            sunMoonAnimation.Start();
            //sunMoonAnimation.ToggleDebugMode(true);

            CustomizeMenuStrip();

            // Enable drag-to-move by clicking anywhere on the form body
            this.MouseDown += Drag_MouseDown;
            this.menuStrip1.MouseDown += Drag_MouseDown;
            this.ovalShape1.MouseDown += Drag_MouseDown;
            this.pictureBox1.MouseDown += Drag_MouseDown;
            this.lineShape1.MouseDown += Drag_MouseDown;

            registryHandler = new RegistrySettingsHandler(false);
            if (registryHandler.SafeLoadBoolRegistryValue(RegistrySettingsHandler.bismillahOnStartupKey))
            {
                try
                {
                    PrayerTimesControl pti = PrayerTimesControl.Instance;
                    pti.playAdhanFile(pti.getDefaultBismillahFilePath());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error playing Bismillah on startup: {ex.Message}");
                }
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
            if (this.WindowState == FormWindowState.Minimized)
                return;

            RepositionControls();
        }

        /// <summary>
        /// Repositions all controls proportionally based on the current form size.
        /// Original design dimensions: 675 x 297 client area.
        /// </summary>
        private void RepositionControls()
        {
            int w = this.ClientSize.Width;
            int h = this.ClientSize.Height;
            const double origW = 675.0;
            const double origH = 297.0;
            double scaleX = w / origW;
            double scaleY = h / origH;

            this.SuspendLayout();

            // Top section: Current prayer info (left side)
            label1.Location = new Point((int)(78 * scaleX), (int)(36 * scaleY));
            label2.Location = new Point((int)(72 * scaleX), (int)(44 * scaleY));
            label7.Location = new Point((int)(78 * scaleX), (int)(93 * scaleY));
            label8.Location = new Point((int)(75 * scaleX), (int)(103 * scaleY));

            // Top section: Kaaba icon (centered)
            pictureBox1.Location = new Point((w - pictureBox1.Width) / 2, (int)(44 * scaleY));

            // Top section: Next prayer info (right side)
            label4.Location = new Point((int)(482 * scaleX), (int)(36 * scaleY));
            label5.Location = new Point((int)(476 * scaleX), (int)(44 * scaleY));
            label6.Location = new Point((int)(482 * scaleX), (int)(93 * scaleY));
            label3.Location = new Point((int)(479 * scaleX), (int)(103 * scaleY));

            // Horizon line
            int lineY = (int)(216 * scaleY);
            lineShape1.X1 = 0;
            lineShape1.X2 = w;
            lineShape1.Y1 = lineY;
            lineShape1.Y2 = lineY;

            // Background oval (top section fill)
            ovalShape1.Location = new Point((int)(-523 * scaleX), (int)(-50 * scaleY));
            ovalShape1.Size = new Size((int)(1727 * scaleX), (int)(206 * scaleY));

            // Prayer title labels (bottom section)
            int titleY = (int)(169 * scaleY);
            int shurookTitleY = (int)(197 * scaleY);
            FajrTitleLabel.Location = new Point((int)(60 * scaleX), titleY);
            ShurookTitleLabel.Location = new Point((int)(122 * scaleX), shurookTitleY);
            DhuhrTitleLabel.Location = new Point((int)(184 * scaleX), titleY);
            AsrTitleLabel.Location = new Point((int)(324 * scaleX), titleY);
            MaghribTitleLabel.Location = new Point((int)(435 * scaleX), titleY);
            IshaTitleLabel.Location = new Point((int)(568 * scaleX), titleY);

            // Prayer value labels (bottom section)
            int valueY = (int)(239 * scaleY);
            int shurookValueY = (int)(222 * scaleY);
            FajrValueLabel.Location = new Point((int)(36 * scaleX), valueY);
            ShurookValueLabel.Location = new Point((int)(119 * scaleX), shurookValueY);
            DhuhrValueLabel.Location = new Point((int)(170 * scaleX), valueY);
            AsrValueLabel.Location = new Point((int)(300 * scaleX), valueY);
            MahribValueLabel.Location = new Point((int)(430 * scaleX), valueY);
            IshaValueLabel.Location = new Point((int)(547 * scaleX), valueY);

            this.ResumeLayout(true);

            // Update sun/moon animation positioning
            if (sunMoonAnimation != null)
            {
                sunMoonAnimation.UpdateDimensions(w, (int)(200 * scaleY));
            }
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
            // Enable dark title bar on Windows 10 1809+ / Windows 11
            try
            {
                int darkMode = 1;
                DwmSetWindowAttribute(this.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref darkMode, sizeof(int));
            }
            catch
            {
                // Silently ignore on older Windows versions that don't support this
            }

            timer1.Start();
            RepositionControls();
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

        private void Drag_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }

}
