using System;
using System.Timers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public Form1()
        {
            InitializeComponent();
            AddNotifyIconContextMenu();
            Tuple<string, TimeSpan> nextPrayerTuple = pti.getNextPrayerNotification();
            SetBold(nextPrayerTuple.Item1);
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
            Tuple<string,TimeSpan> nextPrayerTuple = pti.getNextPrayerNotification();
            label2.Text = $"Time until {nextPrayerTuple.Item1}:";
            label3.Text = nextPrayerTuple.Item2.ToString("hh\\:mm\\:ss");
        }

        public void SetBold(string nextPrayer)
        {
            if (nextPrayer.Equals("Shurook"))
            {
                ResetLabelBoldness();
                FajrTitleLabel.Font = new Font(FajrTitleLabel.Font.FontFamily, FajrTitleLabel.Font.Size, FontStyle.Bold);
                FajrValueLabel.Font = new Font(FajrValueLabel.Font.FontFamily, FajrValueLabel.Font.Size, FontStyle.Bold);
            }
            if (nextPrayer.Equals("Dhuhr"))
            {
                ResetLabelBoldness();
                ShurookTitleLabel.Font = new Font(ShurookTitleLabel.Font.FontFamily, ShurookTitleLabel.Font.Size, FontStyle.Bold);
                ShurookValueLabel.Font = new Font(ShurookValueLabel.Font.FontFamily, ShurookValueLabel.Font.Size, FontStyle.Bold);
            }
            if (nextPrayer.Equals("Asr"))
            {
                ResetLabelBoldness();
                DhuhrTitleLabel.Font = new Font(DhuhrTitleLabel.Font.FontFamily, DhuhrTitleLabel.Font.Size, FontStyle.Bold);
                DhuhrValueLabel.Font = new Font(DhuhrValueLabel.Font.FontFamily, DhuhrValueLabel.Font.Size, FontStyle.Bold);

            }
            if (nextPrayer.Equals("Maghrib"))
            {
                ResetLabelBoldness();
                AsrTitleLabel.Font = new Font(AsrTitleLabel.Font.FontFamily, AsrTitleLabel.Font.Size, FontStyle.Bold);
                AsrValueLabel.Font = new Font(AsrValueLabel.Font.FontFamily, AsrValueLabel.Font.Size, FontStyle.Bold);
            }
            if (nextPrayer.Equals("Isha"))
            {
                ResetLabelBoldness();
                MaghribTitleLabel.Font = new Font(MaghribTitleLabel.Font.FontFamily, MaghribTitleLabel.Font.Size, FontStyle.Bold);
                MahribValueLabel.Font = new Font(MahribValueLabel.Font.FontFamily, MahribValueLabel.Font.Size, FontStyle.Bold);
            }
            if (nextPrayer.Equals("Fajr"))
            {
                ResetLabelBoldness();
                IshaTitleLabel.Font = new Font(IshaTitleLabel.Font.FontFamily, IshaTitleLabel.Font.Size, FontStyle.Bold);
                IshaValueLabel.Font = new Font(IshaValueLabel.Font.FontFamily, IshaValueLabel.Font.Size, FontStyle.Bold);
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
            DhuhrTitleLabel.Refresh();
            DhuhrValueLabel.Refresh();

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
    }

}
