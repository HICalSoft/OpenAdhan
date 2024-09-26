using System;
using System.Drawing;
using System.Windows.Forms;

namespace OpenAdhanForWindowsX
{
    public partial class Form1 : Form
    {
        bool closeOnExit = false;
        ContextMenu contextMenu1;
        MenuItem exitMenuItem;
        MenuItem openMenuItem;
        MenuItem settingsMenuItem;
        PrayerTimesControl pti = PrayerTimesControl.Instance;
        private SunMoonAnimation sunMoonAnimation;

        private bool isDragging = false;
        private Point lastLocation;

        public Form1()
        {
            InitializeComponent();
            AddNotifyIconContextMenu();
            Tuple<PrayerInfo, PrayerInfo> prayerInfo = pti.getNextPrayerNotification();
            PrayerInfo nextPrayer = prayerInfo.Item1;
            PrayerInfo currentPrayer = prayerInfo.Item2;

            SetBold(nextPrayer.Name);

            sunMoonAnimation = new SunMoonAnimation(ovalShape2, pti);
            sunMoonAnimation.PrayerTimesUpdated += SunMoonAnimation_PrayerTimesUpdated;
            sunMoonAnimation.Start();
            //sunMoonAnimation.ToggleDebugMode(true);

            CustomizeMenuStrip();

            // Set the initial cursor for the MenuStrip
            this.menuStrip1.Cursor = Cursors.SizeAll;

            // Add these event handlers
            this.menuStrip1.MouseDown += MenuStrip1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;

            // Handle MouseEnter and MouseLeave for each item in the MenuStrip
            foreach (ToolStripItem item in this.menuStrip1.Items)
            {
                item.MouseEnter += MenuStripItem_MouseEnter;
                item.MouseLeave += MenuStripItem_MouseLeave;
            }

            RegistrySettingsHandler rsh = new RegistrySettingsHandler(false);
            if (rsh.SafeLoadBoolRegistryValue(RegistrySettingsHandler.bismillahOnStartupKey))
            {
                PrayerTimesControl pti = PrayerTimesControl.Instance;
                pti.playAdhan(pti.getDefaultBismillahFilePath());
            }
            if (rsh.SafeLoadBoolRegistryValue(RegistrySettingsHandler.initialInstallFlagKey))
            {
                this.Show();
                var settings = new Settings(this);
                settings.Show();
                rsh.SaveRegistryValue(RegistrySettingsHandler.initialInstallFlagKey, "0", "int");
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
            //if the form is minimized
            //hide it from the task bar
            //and show the system tray icon (represented by the NotifyIcon control)
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                timer1.Stop();
                notifyIcon.Visible = true;
                // notifyIcon.ShowBalloonTip(1000);
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
            SetBold(currentPrayer.Name);
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

        private void MenuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastLocation = e.Location;
                this.Capture = true;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X,
                    (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            this.Capture = false;
        }

        private void MenuStripItem_MouseEnter(object sender, EventArgs e)
        {
            if (sender is ToolStripItem item)
            {
                item.GetCurrentParent().Cursor = Cursors.Default;
            }
        }

        private void MenuStripItem_MouseLeave(object sender, EventArgs e)
        {
            if (sender is ToolStripItem item)
            {
                item.GetCurrentParent().Cursor = Cursors.SizeAll;
            }
        }
    }

}
