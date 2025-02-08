using OpenAdhanForWindowsX.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OpenAdhanForWindowsX
{
    public partial class MainAppForm : Form
    {
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        bool closeOnExit = false;
        ContextMenu contextMenu1;
        MenuItem exitMenuItem;
        MenuItem openMenuItem;
        MenuItem settingsMenuItem;
        PrayerTimesControl pti = PrayerTimesControl.Instance;
        private SunMoonAnimation sunMoonAnimation;

        private static readonly Size fullScreenSize = new Size(675, 297);
        private static readonly Size smallScreenSize = new Size(250, 250);
        private bool isFullSize = true;

        private RegistrySettingsHandler registryHandler;

        private readonly List<Label> prayerTimesLabels;

        public MainAppForm()
        {
            InitializeComponent();
            AddNotifyIconContextMenu();

            sunMoonAnimation = new SunMoonAnimation(sunMoonOvalShape, pti);
            sunMoonAnimation.PrayerTimesUpdated += SunMoonAnimation_PrayerTimesUpdated;
            sunMoonAnimation.Start();
            //sunMoonAnimation.ToggleDebugMode(true);

            CustomizeMenuStrip();

            // Enable Drag on Click for the form and relevant children.
            this.menuStrip1.MouseDown += Drag_MouseDown;
            this.menuStrip1.MouseMove += menuStrip1_MouseMove;
            this.backgroundDarkBlueOvalShape.MouseDown += Drag_MouseDown;
            this.MouseDown += Drag_MouseDown;
            this.middleIconPictureBox.MouseDown += Drag_MouseDown;
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
                var settings = new SettingsForm(this);
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

            prayerTimesLabels = new List<Label>() {
                FajrTitleLabel, ShurookTitleLabel, DhuhrTitleLabel, AsrTitleLabel, MaghribTitleLabel, IshaTitleLabel,
                FajrValueLabel, ShurookValueLabel, DhuhrValueLabel, AsrValueLabel, MahribValueLabel, IshaValueLabel,
                ShurookTitleLabel, ShurookValueLabel
            };

            sunMoonOvalShapeContainer.BringToFront();

            this.isFullSize = registryHandler.IsLastSizeFullSize();
            OnFormSizeChanged(this.isFullSize);
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
            var settings = new SettingsForm(this);
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
            nextPrayerNameLabel.Text = nextPrayer.Name;
            nextPrayerInTimeLabel.Text = nextPrayer.TimeTo.ToString("hh\\:mm\\:ss");

            // Current Prayer
            currentPrayerNameLabel.Text = currentPrayer.Name;
            currentPrayerSinceTimeLabel.Text = currentPrayer.TimeSince.ToString("hh\\:mm\\:ss");
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
            var about = new AboutForm();
            about.Show();
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
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

        private void resizeButton_Click(object sender, EventArgs e)
        {
            isFullSize = !isFullSize;

            OnFormSizeChanged(isFullSize);
        }

        public void OnFormSizeChanged(bool isFullSize)
        {
            registryHandler.SaveIsLastSizeFullSize(isFullSize);

            aboutToolStripMenuItem.Visible = isFullSize;
            editToolStripMenuItem.Visible = isFullSize;
            fileToolStripMenuItem.Visible = isFullSize;

            var screenSizeToUse = isFullSize ? fullScreenSize : smallScreenSize;

            // Set the form size
            this.Size = screenSizeToUse;

            // Set the menu strip size and move the exit and resize buttons
            menuStrip1.Size = screenSizeToUse;
            exitButton.Location = new Point(screenSizeToUse.Width - 27, 2);
            resizeButton.Location = new Point(screenSizeToUse.Width - 53, 2);

            // Bottom Prayer Labels
            foreach (var labelToHide in prayerTimesLabels)
            {
                labelToHide.Visible = isFullSize;
            }

            // SunMoonAnimation
            sunMoonAnimation.SetIsMovementEnabled(isFullSize);
            shapeContainer1.Visible = isFullSize;

            if (isFullSize) // positions to back in fullscreen
            {
                var backgroundColor = Color.FromArgb(4, 28, 56);

                // CURRENT PRAYER 
                currentPrayerLabel.Location = new Point(78, 36);
                currentPrayerNameLabel.Location = new Point(72, 44);
                currentPrayerSinceLabel.Location = new Point(78, 93);
                currentPrayerSinceTimeLabel.Location = new Point(75, 103);

                currentPrayerLabel.Font = new Font(currentPrayerLabel.Font.FontFamily, 10, currentPrayerLabel.Font.Style);
                currentPrayerNameLabel.Font = new Font(currentPrayerNameLabel.Font.FontFamily, 30, currentPrayerNameLabel.Font.Style);
                currentPrayerSinceLabel.Font = new Font(currentPrayerSinceLabel.Font.FontFamily, 8.25F, currentPrayerSinceLabel.Font.Style);
                currentPrayerSinceTimeLabel.Font = new Font(currentPrayerSinceTimeLabel.Font.FontFamily, 20, currentPrayerSinceTimeLabel.Font.Style);

                currentPrayerLabel.BackColor = backgroundColor;
                currentPrayerNameLabel.BackColor = backgroundColor;
                currentPrayerSinceLabel.BackColor = backgroundColor;
                currentPrayerSinceTimeLabel.BackColor = backgroundColor;

                // MIDDLE ICON AND SUNMOON
                middleIconPictureBox.Location = new Point(304, 44);
                middleIconPictureBox.BackColor = Color.FromArgb(4, 28, 56);

                // NEXT PRAYER
                nextPrayerLabel.Location = new Point(482, 36);
                nextPrayerNameLabel.Location = new Point(476, 44);
                nextPrayerInLabel.Location = new Point(482, 93);
                nextPrayerInTimeLabel.Location = new Point(479, 103);

                nextPrayerLabel.Font = new Font(nextPrayerLabel.Font.FontFamily, 10, nextPrayerLabel.Font.Style);
                nextPrayerNameLabel.Font = new Font(nextPrayerNameLabel.Font.FontFamily, 30, nextPrayerNameLabel.Font.Style);
                nextPrayerInLabel.Font = new Font(nextPrayerInLabel.Font.FontFamily, 8.25F, nextPrayerInLabel.Font.Style);
                nextPrayerInTimeLabel.Font = new Font(nextPrayerInTimeLabel.Font.FontFamily, 20, nextPrayerInTimeLabel.Font.Style);

                nextPrayerLabel.BackColor = backgroundColor;
                nextPrayerNameLabel.BackColor = backgroundColor;
                nextPrayerInLabel.BackColor = backgroundColor;
                nextPrayerInTimeLabel.BackColor = backgroundColor;
            }
            else // positions to small size
            {
                // CURRENT PRAYER
                currentPrayerLabel.Location = new Point(25, 32);
                currentPrayerNameLabel.Location = new Point(22, 44);

                currentPrayerSinceLabel.Location = new Point(139, 32);
                currentPrayerSinceTimeLabel.Location = new Point(136, 44);

                currentPrayerLabel.Font = new Font(currentPrayerLabel.Font.FontFamily, 9, currentPrayerLabel.Font.Style);
                currentPrayerNameLabel.Font = new Font(currentPrayerNameLabel.Font.FontFamily, 18, currentPrayerNameLabel.Font.Style);
                currentPrayerSinceLabel.Font = new Font(currentPrayerSinceLabel.Font.FontFamily, 9, currentPrayerSinceLabel.Font.Style);
                currentPrayerSinceTimeLabel.Font = new Font(currentPrayerSinceTimeLabel.Font.FontFamily, 18, currentPrayerSinceTimeLabel.Font.Style);

                currentPrayerLabel.BackColor = Color.Transparent;
                currentPrayerNameLabel.BackColor = Color.Transparent;
                currentPrayerSinceLabel.BackColor = Color.Transparent;
                currentPrayerSinceTimeLabel.BackColor = Color.Transparent;

                // MIDDLE ICON AND SUNMOON
                middleIconPictureBox.Location = new Point(93, 110);
                middleIconPictureBox.BackColor = Color.Transparent;
                sunMoonAnimation.GetOvalShape().Location = new Point(110, 65);

                // NEXT PRAYER
                nextPrayerLabel.Location = new Point(25, 190);
                nextPrayerNameLabel.Location = new Point(22, 202);

                nextPrayerInLabel.Location = new Point(139, 190);
                nextPrayerInTimeLabel.Location = new Point(136, 202);

                nextPrayerLabel.Font = new Font(nextPrayerLabel.Font.FontFamily, 9, nextPrayerLabel.Font.Style);
                nextPrayerNameLabel.Font = new Font(nextPrayerNameLabel.Font.FontFamily, 18, nextPrayerNameLabel.Font.Style);
                nextPrayerInLabel.Font = new Font(nextPrayerInLabel.Font.FontFamily, 9, nextPrayerInLabel.Font.Style);
                nextPrayerInTimeLabel.Font = new Font(nextPrayerInTimeLabel.Font.FontFamily, 18, nextPrayerInTimeLabel.Font.Style);

                nextPrayerLabel.BackColor = Color.Transparent;
                nextPrayerNameLabel.BackColor = Color.Transparent;
                nextPrayerInLabel.BackColor = Color.Transparent;
                nextPrayerInTimeLabel.BackColor = Color.Transparent;
            }

            this.Update();
        }
    }

}
