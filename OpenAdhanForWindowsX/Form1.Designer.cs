using System;
using System.Timers;
using System.Windows.Forms;

// TODO - Need to add timer to main display when open.Timer should only run when displayed!!!!

namespace OpenAdhanForWindowsX
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.FajrTitleLabel = new System.Windows.Forms.Label();
            this.ShurookTitleLabel = new System.Windows.Forms.Label();
            this.DhuhrTitleLabel = new System.Windows.Forms.Label();
            this.AsrTitleLabel = new System.Windows.Forms.Label();
            this.MaghribTitleLabel = new System.Windows.Forms.Label();
            this.IshaTitleLabel = new System.Windows.Forms.Label();
            this.IshaValueLabel = new System.Windows.Forms.Label();
            this.MahribValueLabel = new System.Windows.Forms.Label();
            this.AsrValueLabel = new System.Windows.Forms.Label();
            this.DhuhrValueLabel = new System.Windows.Forms.Label();
            this.ShurookValueLabel = new System.Windows.Forms.Label();
            this.FajrValueLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timeToNextPrayerTimer = new System.Timers.Timer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopAdhanPlaybackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.timeToNextPrayerTimer)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Open Adhan for Windows Prayer Times";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            this.notifyIcon.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseMove);
            // 
            // FajrTitleLabel
            // 
            this.FajrTitleLabel.AutoSize = true;
            this.FajrTitleLabel.Font = new System.Drawing.Font("Georgia", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FajrTitleLabel.Location = new System.Drawing.Point(13, 61);
            this.FajrTitleLabel.Name = "FajrTitleLabel";
            this.FajrTitleLabel.Size = new System.Drawing.Size(56, 29);
            this.FajrTitleLabel.TabIndex = 0;
            this.FajrTitleLabel.Text = "Fajr";
            // 
            // ShurookTitleLabel
            // 
            this.ShurookTitleLabel.AutoSize = true;
            this.ShurookTitleLabel.Font = new System.Drawing.Font("Georgia", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShurookTitleLabel.Location = new System.Drawing.Point(13, 90);
            this.ShurookTitleLabel.Name = "ShurookTitleLabel";
            this.ShurookTitleLabel.Size = new System.Drawing.Size(103, 29);
            this.ShurookTitleLabel.TabIndex = 1;
            this.ShurookTitleLabel.Text = "Shurook";
            // 
            // DhuhrTitleLabel
            // 
            this.DhuhrTitleLabel.AutoSize = true;
            this.DhuhrTitleLabel.Font = new System.Drawing.Font("Georgia", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DhuhrTitleLabel.Location = new System.Drawing.Point(13, 119);
            this.DhuhrTitleLabel.Name = "DhuhrTitleLabel";
            this.DhuhrTitleLabel.Size = new System.Drawing.Size(83, 29);
            this.DhuhrTitleLabel.TabIndex = 2;
            this.DhuhrTitleLabel.Text = "Dhuhr";
            // 
            // AsrTitleLabel
            // 
            this.AsrTitleLabel.AutoSize = true;
            this.AsrTitleLabel.Font = new System.Drawing.Font("Georgia", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AsrTitleLabel.Location = new System.Drawing.Point(13, 148);
            this.AsrTitleLabel.Name = "AsrTitleLabel";
            this.AsrTitleLabel.Size = new System.Drawing.Size(49, 29);
            this.AsrTitleLabel.TabIndex = 3;
            this.AsrTitleLabel.Text = "Asr";
            // 
            // MaghribTitleLabel
            // 
            this.MaghribTitleLabel.AutoSize = true;
            this.MaghribTitleLabel.Font = new System.Drawing.Font("Georgia", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaghribTitleLabel.Location = new System.Drawing.Point(13, 177);
            this.MaghribTitleLabel.Name = "MaghribTitleLabel";
            this.MaghribTitleLabel.Size = new System.Drawing.Size(103, 29);
            this.MaghribTitleLabel.TabIndex = 4;
            this.MaghribTitleLabel.Text = "Maghrib";
            // 
            // IshaTitleLabel
            // 
            this.IshaTitleLabel.AutoSize = true;
            this.IshaTitleLabel.Font = new System.Drawing.Font("Georgia", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IshaTitleLabel.Location = new System.Drawing.Point(13, 206);
            this.IshaTitleLabel.Name = "IshaTitleLabel";
            this.IshaTitleLabel.Size = new System.Drawing.Size(58, 29);
            this.IshaTitleLabel.TabIndex = 5;
            this.IshaTitleLabel.Text = "Isha";
            // 
            // IshaValueLabel
            // 
            this.IshaValueLabel.AutoSize = true;
            this.IshaValueLabel.Font = new System.Drawing.Font("Georgia", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IshaValueLabel.Location = new System.Drawing.Point(142, 206);
            this.IshaValueLabel.Name = "IshaValueLabel";
            this.IshaValueLabel.Size = new System.Drawing.Size(117, 29);
            this.IshaValueLabel.TabIndex = 11;
            this.IshaValueLabel.Text = "06:18 PM";
            // 
            // MahribValueLabel
            // 
            this.MahribValueLabel.AutoSize = true;
            this.MahribValueLabel.Font = new System.Drawing.Font("Georgia", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MahribValueLabel.Location = new System.Drawing.Point(144, 177);
            this.MahribValueLabel.Name = "MahribValueLabel";
            this.MahribValueLabel.Size = new System.Drawing.Size(121, 29);
            this.MahribValueLabel.TabIndex = 10;
            this.MahribValueLabel.Text = "04:48 PM";
            // 
            // AsrValueLabel
            // 
            this.AsrValueLabel.AutoSize = true;
            this.AsrValueLabel.Font = new System.Drawing.Font("Georgia", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AsrValueLabel.Location = new System.Drawing.Point(144, 148);
            this.AsrValueLabel.Name = "AsrValueLabel";
            this.AsrValueLabel.Size = new System.Drawing.Size(117, 29);
            this.AsrValueLabel.TabIndex = 9;
            this.AsrValueLabel.Text = "02:27 PM";
            // 
            // DhuhrValueLabel
            // 
            this.DhuhrValueLabel.AutoSize = true;
            this.DhuhrValueLabel.Font = new System.Drawing.Font("Georgia", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DhuhrValueLabel.Location = new System.Drawing.Point(144, 119);
            this.DhuhrValueLabel.Name = "DhuhrValueLabel";
            this.DhuhrValueLabel.Size = new System.Drawing.Size(108, 29);
            this.DhuhrValueLabel.TabIndex = 8;
            this.DhuhrValueLabel.Text = "11:51 AM";
            // 
            // ShurookValueLabel
            // 
            this.ShurookValueLabel.AutoSize = true;
            this.ShurookValueLabel.Font = new System.Drawing.Font("Georgia", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShurookValueLabel.Location = new System.Drawing.Point(144, 90);
            this.ShurookValueLabel.Name = "ShurookValueLabel";
            this.ShurookValueLabel.Size = new System.Drawing.Size(120, 29);
            this.ShurookValueLabel.TabIndex = 7;
            this.ShurookValueLabel.Text = "06:53 AM";
            // 
            // FajrValueLabel
            // 
            this.FajrValueLabel.AutoSize = true;
            this.FajrValueLabel.Font = new System.Drawing.Font("Georgia", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FajrValueLabel.Location = new System.Drawing.Point(144, 61);
            this.FajrValueLabel.Name = "FajrValueLabel";
            this.FajrValueLabel.Size = new System.Drawing.Size(117, 29);
            this.FajrValueLabel.TabIndex = 6;
            this.FajrValueLabel.Text = "05:14 AM";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Georgia", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 31);
            this.label1.TabIndex = 12;
            this.label1.Text = "Prayer Times:";
            // 
            // timeToNextPrayerTimer
            // 
            this.timeToNextPrayerTimer.Enabled = true;
            this.timeToNextPrayerTimer.Interval = 1000D;
            this.timeToNextPrayerTimer.SynchronizingObject = this;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(273, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.BackColor = System.Drawing.SystemColors.ControlDark;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.stopAdhanPlaybackToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.BackColor = System.Drawing.SystemColors.ControlDark;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.SettingsToolStripMenuItem_Click_1);
            // 
            // stopAdhanPlaybackToolStripMenuItem
            // 
            this.stopAdhanPlaybackToolStripMenuItem.BackColor = System.Drawing.SystemColors.ControlDark;
            this.stopAdhanPlaybackToolStripMenuItem.Name = "stopAdhanPlaybackToolStripMenuItem";
            this.stopAdhanPlaybackToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.stopAdhanPlaybackToolStripMenuItem.Text = "Stop Adhan Playback";
            this.stopAdhanPlaybackToolStripMenuItem.Click += new System.EventHandler(this.stopAdhanPlaybackToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.aboutToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem1.Text = "About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.AboutToolStripMenuItem1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Georgia", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 247);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(237, 31);
            this.label2.TabIndex = 14;
            this.label2.Text = "Time Until Prayer:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Georgia", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(74, 278);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 29);
            this.label3.TabIndex = 15;
            this.label3.Text = "00:00:00";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(273, 312);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.IshaValueLabel);
            this.Controls.Add(this.MahribValueLabel);
            this.Controls.Add(this.AsrValueLabel);
            this.Controls.Add(this.DhuhrValueLabel);
            this.Controls.Add(this.ShurookValueLabel);
            this.Controls.Add(this.FajrValueLabel);
            this.Controls.Add(this.IshaTitleLabel);
            this.Controls.Add(this.MaghribTitleLabel);
            this.Controls.Add(this.AsrTitleLabel);
            this.Controls.Add(this.DhuhrTitleLabel);
            this.Controls.Add(this.ShurookTitleLabel);
            this.Controls.Add(this.FajrTitleLabel);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Open Adhan";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.timeToNextPrayerTimer)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        private void updateTimeToNextPrayerNotification()
        {
            PrayerTimesControl pti = PrayerTimesControl.Instance;
            Tuple<string, TimeSpan> nextPrayerTuple = pti.getNextPrayerNotification();

            if (nextPrayerTuple.Item1.Equals("Shurook"))
            {
                this.notifyIcon.Text = $"{getPreviousPrayerString(nextPrayerTuple.Item1)} passed, {nextPrayerTuple.Item2.Hours.ToString()} hours and {nextPrayerTuple.Item2.Minutes.ToString()} minutes to {nextPrayerTuple.Item1}.";
            }
            else
            {
                this.notifyIcon.Text = $"In {getPreviousPrayerString(nextPrayerTuple.Item1)}, {nextPrayerTuple.Item2.Hours.ToString()} hours and {nextPrayerTuple.Item2.Minutes.ToString()} minutes to {nextPrayerTuple.Item1}.";
            }
            
        }

        private string getPreviousPrayerString(string currentPrayerString)
        {
            string[] prayers = { "Fajr", "Shurook", "Dhuhr", "Asr", "Maghrib", "Isha" };
            int curPrayerIdx = Array.FindIndex(prayers, m => m.Equals(currentPrayerString));
            if (curPrayerIdx == 0)
                curPrayerIdx = prayers.Length;
            return prayers[curPrayerIdx - 1];
        }


        public void updatePrayerTimesDisplay()
        {
            PrayerTimesControl prayerTimesInstance = PrayerTimesControl.Instance;
            DateTime[] prayerTimes = prayerTimesInstance.getPrayerDateTimes();

            if(prayerTimes is null)
            {
                MessageBox.Show($"Unable to calculate prayer times with specified latitude, longitude, and timezone values. Update settings with valid values.", "Unable to calculate prayer times!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.FajrValueLabel.Text = "Invalid";
                this.FajrValueLabel.Refresh();
                this.ShurookValueLabel.Text = "Invalid";
                this.ShurookValueLabel.Refresh();
                this.DhuhrValueLabel.Text = "Invalid";
                this.DhuhrValueLabel.Refresh();
                this.AsrValueLabel.Text = "Invalid";
                this.AsrValueLabel.Refresh();
                this.MahribValueLabel.Text = "Invalid";
                this.MahribValueLabel.Refresh();
                this.IshaValueLabel.Text = "Invalid";
                this.IshaValueLabel.Refresh();

                return;
            }

            this.FajrValueLabel.Text = prayerTimes[prayerTimesInstance.fajr].ToString(@"hh\:mm tt");
            this.FajrValueLabel.Refresh();
            this.ShurookValueLabel.Text = prayerTimes[prayerTimesInstance.shurook].ToString(@"hh\:mm tt");
            this.ShurookValueLabel.Refresh();
            this.DhuhrValueLabel.Text = prayerTimes[prayerTimesInstance.dhuhr].ToString(@"hh\:mm tt");
            this.DhuhrValueLabel.Refresh();
            this.AsrValueLabel.Text = prayerTimes[prayerTimesInstance.asr].ToString(@"hh\:mm tt");
            this.AsrValueLabel.Refresh();
            this.MahribValueLabel.Text = prayerTimes[prayerTimesInstance.maghrib].ToString(@"hh\:mm tt");
            this.MahribValueLabel.Refresh();
            this.IshaValueLabel.Text = prayerTimes[prayerTimesInstance.isha].ToString(@"hh\:mm tt");
            this.IshaValueLabel.Refresh();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Label FajrTitleLabel;
        private System.Windows.Forms.Label ShurookTitleLabel;
        private System.Windows.Forms.Label DhuhrTitleLabel;
        private System.Windows.Forms.Label AsrTitleLabel;
        private System.Windows.Forms.Label MaghribTitleLabel;
        private System.Windows.Forms.Label IshaTitleLabel;
        private System.Windows.Forms.Label IshaValueLabel;
        private System.Windows.Forms.Label MahribValueLabel;
        private System.Windows.Forms.Label AsrValueLabel;
        private System.Windows.Forms.Label DhuhrValueLabel;
        private System.Windows.Forms.Label ShurookValueLabel;
        private System.Windows.Forms.Label FajrValueLabel;
        private System.Windows.Forms.Label label1;
        private System.Timers.Timer timeToNextPrayerTimer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private Label label3;
        private Label label2;
        private System.Windows.Forms.Timer timer1;
        private ToolStripMenuItem stopAdhanPlaybackToolStripMenuItem;
    }
}

