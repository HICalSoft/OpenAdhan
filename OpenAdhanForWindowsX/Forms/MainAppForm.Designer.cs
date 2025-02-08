using OpenAdhanForWindowsX.Components;
using OpenAdhanForWindowsX.Managers;
using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

// TODO - Need to add timer to main display when open.Timer should only run when displayed!!!!

namespace OpenAdhanForWindowsX
{
    partial class MainAppForm
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

                if (sunMoonAnimation != null)
                    sunMoonAnimation.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainAppForm));
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
            this.timeToNextPrayerTimer = new System.Timers.Timer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopAdhanPlaybackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.currentPrayerNameLabel = new System.Windows.Forms.Label();
            this.nextPrayerInTimeLabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.backgroundDarkBlueOvalShape = new Microsoft.VisualBasic.PowerPacks.OvalShape();
            this.sunMoonOvalShapeContainer = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.sunMoonOvalShape = new Microsoft.VisualBasic.PowerPacks.OvalShape();
            this.middleIconPictureBox = new System.Windows.Forms.PictureBox();
            this.currentPrayerLabel = new System.Windows.Forms.Label();
            this.nextPrayerLabel = new System.Windows.Forms.Label();
            this.nextPrayerNameLabel = new System.Windows.Forms.Label();
            this.nextPrayerInLabel = new System.Windows.Forms.Label();
            this.currentPrayerSinceLabel = new System.Windows.Forms.Label();
            this.currentPrayerSinceTimeLabel = new System.Windows.Forms.Label();
            this.resizeButton = new OpenAdhanForWindowsX.Components.ButtonNoPadding();
            this.exitButton = new OpenAdhanForWindowsX.Components.ButtonNoPadding();
            ((System.ComponentModel.ISupportInitialize)(this.timeToNextPrayerTimer)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.middleIconPictureBox)).BeginInit();
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
            this.FajrTitleLabel.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FajrTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(201)))), ((int)(((byte)(168)))));
            this.FajrTitleLabel.Location = new System.Drawing.Point(60, 169);
            this.FajrTitleLabel.Name = "FajrTitleLabel";
            this.FajrTitleLabel.Size = new System.Drawing.Size(39, 23);
            this.FajrTitleLabel.TabIndex = 0;
            this.FajrTitleLabel.Text = "Fajr";
            // 
            // ShurookTitleLabel
            // 
            this.ShurookTitleLabel.AutoSize = true;
            this.ShurookTitleLabel.Font = new System.Drawing.Font("Calibri", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShurookTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(201)))), ((int)(((byte)(168)))));
            this.ShurookTitleLabel.Location = new System.Drawing.Point(122, 197);
            this.ShurookTitleLabel.Name = "ShurookTitleLabel";
            this.ShurookTitleLabel.Size = new System.Drawing.Size(45, 13);
            this.ShurookTitleLabel.TabIndex = 1;
            this.ShurookTitleLabel.Text = "Shurook";
            // 
            // DhuhrTitleLabel
            // 
            this.DhuhrTitleLabel.AutoSize = true;
            this.DhuhrTitleLabel.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DhuhrTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(201)))), ((int)(((byte)(168)))));
            this.DhuhrTitleLabel.Location = new System.Drawing.Point(184, 169);
            this.DhuhrTitleLabel.Name = "DhuhrTitleLabel";
            this.DhuhrTitleLabel.Size = new System.Drawing.Size(59, 23);
            this.DhuhrTitleLabel.TabIndex = 2;
            this.DhuhrTitleLabel.Text = "Dhuhr";
            // 
            // AsrTitleLabel
            // 
            this.AsrTitleLabel.AutoSize = true;
            this.AsrTitleLabel.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AsrTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(201)))), ((int)(((byte)(168)))));
            this.AsrTitleLabel.Location = new System.Drawing.Point(324, 169);
            this.AsrTitleLabel.Name = "AsrTitleLabel";
            this.AsrTitleLabel.Size = new System.Drawing.Size(36, 23);
            this.AsrTitleLabel.TabIndex = 3;
            this.AsrTitleLabel.Text = "Asr";
            // 
            // MaghribTitleLabel
            // 
            this.MaghribTitleLabel.AutoSize = true;
            this.MaghribTitleLabel.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaghribTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(201)))), ((int)(((byte)(168)))));
            this.MaghribTitleLabel.Location = new System.Drawing.Point(435, 169);
            this.MaghribTitleLabel.Name = "MaghribTitleLabel";
            this.MaghribTitleLabel.Size = new System.Drawing.Size(75, 23);
            this.MaghribTitleLabel.TabIndex = 4;
            this.MaghribTitleLabel.Text = "Maghrib";
            // 
            // IshaTitleLabel
            // 
            this.IshaTitleLabel.AutoSize = true;
            this.IshaTitleLabel.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IshaTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(201)))), ((int)(((byte)(168)))));
            this.IshaTitleLabel.Location = new System.Drawing.Point(568, 169);
            this.IshaTitleLabel.Name = "IshaTitleLabel";
            this.IshaTitleLabel.Size = new System.Drawing.Size(43, 23);
            this.IshaTitleLabel.TabIndex = 5;
            this.IshaTitleLabel.Text = "Isha";
            // 
            // IshaValueLabel
            // 
            this.IshaValueLabel.AutoSize = true;
            this.IshaValueLabel.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IshaValueLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(201)))), ((int)(((byte)(168)))));
            this.IshaValueLabel.Location = new System.Drawing.Point(547, 239);
            this.IshaValueLabel.Name = "IshaValueLabel";
            this.IshaValueLabel.Size = new System.Drawing.Size(85, 23);
            this.IshaValueLabel.TabIndex = 11;
            this.IshaValueLabel.Text = "06:18 PM";
            // 
            // MahribValueLabel
            // 
            this.MahribValueLabel.AutoSize = true;
            this.MahribValueLabel.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MahribValueLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(201)))), ((int)(((byte)(168)))));
            this.MahribValueLabel.Location = new System.Drawing.Point(430, 239);
            this.MahribValueLabel.Name = "MahribValueLabel";
            this.MahribValueLabel.Size = new System.Drawing.Size(85, 23);
            this.MahribValueLabel.TabIndex = 10;
            this.MahribValueLabel.Text = "04:48 PM";
            // 
            // AsrValueLabel
            // 
            this.AsrValueLabel.AutoSize = true;
            this.AsrValueLabel.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AsrValueLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(201)))), ((int)(((byte)(168)))));
            this.AsrValueLabel.Location = new System.Drawing.Point(300, 239);
            this.AsrValueLabel.Name = "AsrValueLabel";
            this.AsrValueLabel.Size = new System.Drawing.Size(85, 23);
            this.AsrValueLabel.TabIndex = 9;
            this.AsrValueLabel.Text = "02:27 PM";
            // 
            // DhuhrValueLabel
            // 
            this.DhuhrValueLabel.AutoSize = true;
            this.DhuhrValueLabel.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DhuhrValueLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(201)))), ((int)(((byte)(168)))));
            this.DhuhrValueLabel.Location = new System.Drawing.Point(170, 239);
            this.DhuhrValueLabel.Name = "DhuhrValueLabel";
            this.DhuhrValueLabel.Size = new System.Drawing.Size(86, 23);
            this.DhuhrValueLabel.TabIndex = 8;
            this.DhuhrValueLabel.Text = "11:51 AM";
            // 
            // ShurookValueLabel
            // 
            this.ShurookValueLabel.AutoSize = true;
            this.ShurookValueLabel.Font = new System.Drawing.Font("Calibri", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShurookValueLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(201)))), ((int)(((byte)(168)))));
            this.ShurookValueLabel.Location = new System.Drawing.Point(119, 222);
            this.ShurookValueLabel.Name = "ShurookValueLabel";
            this.ShurookValueLabel.Size = new System.Drawing.Size(51, 13);
            this.ShurookValueLabel.TabIndex = 7;
            this.ShurookValueLabel.Text = "06:53 AM";
            // 
            // FajrValueLabel
            // 
            this.FajrValueLabel.AutoSize = true;
            this.FajrValueLabel.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FajrValueLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(201)))), ((int)(((byte)(168)))));
            this.FajrValueLabel.Location = new System.Drawing.Point(36, 239);
            this.FajrValueLabel.Name = "FajrValueLabel";
            this.FajrValueLabel.Size = new System.Drawing.Size(86, 23);
            this.FajrValueLabel.TabIndex = 6;
            this.FajrValueLabel.Text = "05:14 AM";
            // 
            // timeToNextPrayerTimer
            // 
            this.timeToNextPrayerTimer.Enabled = true;
            this.timeToNextPrayerTimer.Interval = 1000D;
            this.timeToNextPrayerTimer.SynchronizingObject = this;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(19)))), ((int)(((byte)(39)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(675, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(84)))), ((int)(((byte)(116)))));
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.stopAdhanPlaybackToolStripMenuItem});
            this.editToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(84)))), ((int)(((byte)(116)))));
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.SettingsToolStripMenuItem_Click_1);
            // 
            // stopAdhanPlaybackToolStripMenuItem
            // 
            this.stopAdhanPlaybackToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(84)))), ((int)(((byte)(116)))));
            this.stopAdhanPlaybackToolStripMenuItem.Name = "stopAdhanPlaybackToolStripMenuItem";
            this.stopAdhanPlaybackToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.stopAdhanPlaybackToolStripMenuItem.Text = "Stop Adhan Playback";
            this.stopAdhanPlaybackToolStripMenuItem.Click += new System.EventHandler(this.stopAdhanPlaybackToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1});
            this.aboutToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.aboutToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(84)))), ((int)(((byte)(116)))));
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem1.Text = "About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.AboutToolStripMenuItem1_Click);
            // 
            // currentPrayerNameLabel
            // 
            this.currentPrayerNameLabel.AutoSize = true;
            this.currentPrayerNameLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(28)))), ((int)(((byte)(56)))));
            this.currentPrayerNameLabel.Font = new System.Drawing.Font("Calibri", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentPrayerNameLabel.ForeColor = System.Drawing.Color.White;
            this.currentPrayerNameLabel.Location = new System.Drawing.Point(72, 44);
            this.currentPrayerNameLabel.Name = "currentPrayerNameLabel";
            this.currentPrayerNameLabel.Size = new System.Drawing.Size(145, 49);
            this.currentPrayerNameLabel.TabIndex = 14;
            this.currentPrayerNameLabel.Text = "Current";
            // 
            // nextPrayerInTimeLabel
            // 
            this.nextPrayerInTimeLabel.AutoSize = true;
            this.nextPrayerInTimeLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(28)))), ((int)(((byte)(56)))));
            this.nextPrayerInTimeLabel.Font = new System.Drawing.Font("Calibri", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextPrayerInTimeLabel.ForeColor = System.Drawing.Color.White;
            this.nextPrayerInTimeLabel.Location = new System.Drawing.Point(479, 103);
            this.nextPrayerInTimeLabel.Name = "nextPrayerInTimeLabel";
            this.nextPrayerInTimeLabel.Size = new System.Drawing.Size(113, 33);
            this.nextPrayerInTimeLabel.TabIndex = 15;
            this.nextPrayerInTimeLabel.Text = "00:00:00";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1,
            this.backgroundDarkBlueOvalShape});
            this.shapeContainer1.Size = new System.Drawing.Size(675, 297);
            this.shapeContainer1.TabIndex = 16;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(100)))), ((int)(((byte)(125)))));
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.SelectionColor = System.Drawing.Color.Transparent;
            this.lineShape1.X1 = 0;
            this.lineShape1.X2 = 675;
            this.lineShape1.Y1 = 216;
            this.lineShape1.Y2 = 216;
            // 
            // backgroundDarkBlueOvalShape
            // 
            this.backgroundDarkBlueOvalShape.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(28)))), ((int)(((byte)(56)))));
            this.backgroundDarkBlueOvalShape.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(28)))), ((int)(((byte)(56)))));
            this.backgroundDarkBlueOvalShape.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.backgroundDarkBlueOvalShape.Location = new System.Drawing.Point(-523, -50);
            this.backgroundDarkBlueOvalShape.Name = "backgroundDarkBlueOvalShape";
            this.backgroundDarkBlueOvalShape.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(28)))), ((int)(((byte)(56)))));
            this.backgroundDarkBlueOvalShape.Size = new System.Drawing.Size(1727, 206);
            // 
            // sunMoonOvalShapeContainer
            // 
            this.sunMoonOvalShapeContainer.Location = new System.Drawing.Point(0, 0);
            this.sunMoonOvalShapeContainer.Margin = new System.Windows.Forms.Padding(0);
            this.sunMoonOvalShapeContainer.Name = "sunMoonOvalShapeContainer";
            this.sunMoonOvalShapeContainer.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.sunMoonOvalShape});
            this.sunMoonOvalShapeContainer.Size = new System.Drawing.Size(675, 297);
            this.sunMoonOvalShapeContainer.TabIndex = 16;
            this.sunMoonOvalShapeContainer.TabStop = false;
            // 
            // sunMoonOvalShape
            // 
            this.sunMoonOvalShape.BackColor = System.Drawing.Color.Transparent;
            this.sunMoonOvalShape.BorderColor = System.Drawing.Color.Gold;
            this.sunMoonOvalShape.FillColor = System.Drawing.Color.Orange;
            this.sunMoonOvalShape.FillGradientColor = System.Drawing.Color.Yellow;
            this.sunMoonOvalShape.FillGradientStyle = Microsoft.VisualBasic.PowerPacks.FillGradientStyle.Central;
            this.sunMoonOvalShape.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.sunMoonOvalShape.Location = new System.Drawing.Point(-100, 176);
            this.sunMoonOvalShape.Name = "ovalShape2";
            this.sunMoonOvalShape.SelectionColor = System.Drawing.Color.Transparent;
            this.sunMoonOvalShape.Size = new System.Drawing.Size(30, 30);
            // 
            // middleIconPictureBox
            // 
            this.middleIconPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(28)))), ((int)(((byte)(56)))));
            this.middleIconPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.middleIconPictureBox.ErrorImage = global::OpenAdhanForWindowsX.Properties.Resources.islamic_mosque_holy_makkah_hajj_umrah_kaaba_icon_2586900_64x64;
            this.middleIconPictureBox.ImageLocation = "OpenAdhanForWindowsX.Properties.Resources.islamic_mosque_holy_makkah_hajj_umrah_k" +
    "aaba_icon_258690";
            this.middleIconPictureBox.InitialImage = global::OpenAdhanForWindowsX.Properties.Resources.islamic_mosque_holy_makkah_hajj_umrah_kaaba_icon_2586900_64x64;
            this.middleIconPictureBox.Location = new System.Drawing.Point(304, 44);
            this.middleIconPictureBox.Name = "middleIconPictureBox";
            this.middleIconPictureBox.Size = new System.Drawing.Size(64, 64);
            this.middleIconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.middleIconPictureBox.TabIndex = 18;
            this.middleIconPictureBox.TabStop = false;
            // 
            // currentPrayerLabel
            // 
            this.currentPrayerLabel.AutoSize = true;
            this.currentPrayerLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(28)))), ((int)(((byte)(56)))));
            this.currentPrayerLabel.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentPrayerLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.currentPrayerLabel.Location = new System.Drawing.Point(78, 36);
            this.currentPrayerLabel.Name = "currentPrayerLabel";
            this.currentPrayerLabel.Size = new System.Drawing.Size(51, 17);
            this.currentPrayerLabel.TabIndex = 19;
            this.currentPrayerLabel.Text = "Current";
            // 
            // nextPrayerLabel
            // 
            this.nextPrayerLabel.AutoSize = true;
            this.nextPrayerLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(28)))), ((int)(((byte)(56)))));
            this.nextPrayerLabel.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextPrayerLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.nextPrayerLabel.Location = new System.Drawing.Point(482, 36);
            this.nextPrayerLabel.Name = "nextPrayerLabel";
            this.nextPrayerLabel.Size = new System.Drawing.Size(35, 17);
            this.nextPrayerLabel.TabIndex = 21;
            this.nextPrayerLabel.Text = "Next";
            // 
            // nextPrayerNameLabel
            // 
            this.nextPrayerNameLabel.AutoSize = true;
            this.nextPrayerNameLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(28)))), ((int)(((byte)(56)))));
            this.nextPrayerNameLabel.Font = new System.Drawing.Font("Calibri", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextPrayerNameLabel.ForeColor = System.Drawing.Color.White;
            this.nextPrayerNameLabel.Location = new System.Drawing.Point(476, 44);
            this.nextPrayerNameLabel.Name = "nextPrayerNameLabel";
            this.nextPrayerNameLabel.Size = new System.Drawing.Size(97, 49);
            this.nextPrayerNameLabel.TabIndex = 20;
            this.nextPrayerNameLabel.Text = "Next";
            // 
            // nextPrayerInLabel
            // 
            this.nextPrayerInLabel.AutoSize = true;
            this.nextPrayerInLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(28)))), ((int)(((byte)(56)))));
            this.nextPrayerInLabel.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextPrayerInLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.nextPrayerInLabel.Location = new System.Drawing.Point(482, 93);
            this.nextPrayerInLabel.Name = "nextPrayerInLabel";
            this.nextPrayerInLabel.Size = new System.Drawing.Size(16, 13);
            this.nextPrayerInLabel.TabIndex = 22;
            this.nextPrayerInLabel.Text = "In";
            // 
            // currentPrayerSinceLabel
            // 
            this.currentPrayerSinceLabel.AutoSize = true;
            this.currentPrayerSinceLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(28)))), ((int)(((byte)(56)))));
            this.currentPrayerSinceLabel.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentPrayerSinceLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.currentPrayerSinceLabel.Location = new System.Drawing.Point(78, 93);
            this.currentPrayerSinceLabel.Name = "currentPrayerSinceLabel";
            this.currentPrayerSinceLabel.Size = new System.Drawing.Size(32, 13);
            this.currentPrayerSinceLabel.TabIndex = 24;
            this.currentPrayerSinceLabel.Text = "Since";
            // 
            // currentPrayerSinceTimeLabel
            // 
            this.currentPrayerSinceTimeLabel.AutoSize = true;
            this.currentPrayerSinceTimeLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(28)))), ((int)(((byte)(56)))));
            this.currentPrayerSinceTimeLabel.Font = new System.Drawing.Font("Calibri", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentPrayerSinceTimeLabel.ForeColor = System.Drawing.Color.White;
            this.currentPrayerSinceTimeLabel.Location = new System.Drawing.Point(75, 103);
            this.currentPrayerSinceTimeLabel.Name = "currentPrayerSinceTimeLabel";
            this.currentPrayerSinceTimeLabel.Size = new System.Drawing.Size(113, 33);
            this.currentPrayerSinceTimeLabel.TabIndex = 23;
            this.currentPrayerSinceTimeLabel.Text = "00:00:00";
            // 
            // resizeButton
            // 
            this.resizeButton.BackColor = System.Drawing.Color.MintCream;
            this.resizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resizeButton.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resizeButton.Location = new System.Drawing.Point(622, 2);
            this.resizeButton.Name = "resizeButton";
            this.resizeButton.Size = new System.Drawing.Size(20, 20);
            this.resizeButton.TabIndex = 25;
            this.resizeButton.Text = "◰";
            this.resizeButton.UseVisualStyleBackColor = false;
            this.resizeButton.Click += new System.EventHandler(this.resizeButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exitButton.BackColor = System.Drawing.Color.Red;
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitButton.Location = new System.Drawing.Point(648, 2);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(20, 20);
            this.exitButton.TabIndex = 17;
            this.exitButton.Text = "X";
            this.exitButton.UseVisualStyleBackColor = false;
            this.exitButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button1_MouseClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(36)))), ((int)(((byte)(72)))));
            this.ClientSize = new System.Drawing.Size(675, 297);
            this.Controls.Add(this.resizeButton);
            this.Controls.Add(this.currentPrayerSinceLabel);
            this.Controls.Add(this.currentPrayerSinceTimeLabel);
            this.Controls.Add(this.nextPrayerInLabel);
            this.Controls.Add(this.nextPrayerLabel);
            this.Controls.Add(this.nextPrayerNameLabel);
            this.Controls.Add(this.currentPrayerLabel);
            this.Controls.Add(this.middleIconPictureBox);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.nextPrayerInTimeLabel);
            this.Controls.Add(this.currentPrayerNameLabel);
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
            this.Controls.Add(this.sunMoonOvalShapeContainer);
            this.Controls.Add(this.shapeContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
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
            ((System.ComponentModel.ISupportInitialize)(this.middleIconPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        private void updateTimeToNextPrayerNotification()
        {
            PrayerTimesControl pti = PrayerTimesControl.Instance;
            Tuple<PrayerInfo, PrayerInfo> prayerInfo = pti.getNextPrayerNotification();
            PrayerInfo nextPrayer = prayerInfo.Item1;
            PrayerInfo currentPrayer = prayerInfo.Item2;

            if (nextPrayer.Name.Equals("Shurook"))
            {
                this.notifyIcon.Text = $"{getPreviousPrayerString(nextPrayer.Name)} passed, {nextPrayer.TimeTo.Hours.ToString()} hours and {nextPrayer.TimeTo.Minutes.ToString()} minutes to {nextPrayer.Name}.";
            }
            else
            {
                this.notifyIcon.Text = $"In {getPreviousPrayerString(nextPrayer.Name)}, {nextPrayer.TimeTo.Hours.ToString()} hours and {nextPrayer.TimeTo.Minutes.ToString()} minutes to {nextPrayer.Name}.";
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

            if (prayerTimes is null)
            {
                MessageBox.Show($"Unable to calculate prayer times with specified latitude, longitude, and timezone values. Update settings with valid values.", "Unable to calculate prayer times!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetInvalidPrayerTimes();
                return;
            }

            UpdatePrayerTimeLabels(prayerTimes, prayerTimesInstance);

            // If we have a valid sunMoonAnimation, update it with the new prayer times
            if (sunMoonAnimation != null)
            {
                sunMoonAnimation.UpdatePrayerTimes();
            }
        }

        private void SetInvalidPrayerTimes()
        {
            FajrValueLabel.Text = "Invalid";
            ShurookValueLabel.Text = "Invalid";
            DhuhrValueLabel.Text = "Invalid";
            AsrValueLabel.Text = "Invalid";
            MahribValueLabel.Text = "Invalid";
            IshaValueLabel.Text = "Invalid";

            FajrValueLabel.Refresh();
            ShurookValueLabel.Refresh();
            DhuhrValueLabel.Refresh();
            AsrValueLabel.Refresh();
            MahribValueLabel.Refresh();
            IshaValueLabel.Refresh();
        }

        private void UpdatePrayerTimeLabels(DateTime[] prayerTimes, PrayerTimesControl pti)
        {
            FajrValueLabel.Text = prayerTimes[pti.fajr].ToString(@"hh\:mm tt");
            ShurookValueLabel.Text = prayerTimes[pti.shurook].ToString(@"hh\:mm tt");
            DhuhrValueLabel.Text = prayerTimes[pti.dhuhr].ToString(@"hh\:mm tt");
            AsrValueLabel.Text = prayerTimes[pti.asr].ToString(@"hh\:mm tt");
            MahribValueLabel.Text = prayerTimes[pti.maghrib].ToString(@"hh\:mm tt");
            IshaValueLabel.Text = prayerTimes[pti.isha].ToString(@"hh\:mm tt");

            FajrValueLabel.Refresh();
            ShurookValueLabel.Refresh();
            DhuhrValueLabel.Refresh();
            AsrValueLabel.Refresh();
            MahribValueLabel.Refresh();
            IshaValueLabel.Refresh();
        }

        private void CustomizeMenuStrip()
        {
            menuStrip1.Renderer = new CustomMenuRenderer();
            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                item.BackColor = Color.Transparent;
                item.ForeColor = Color.White;
            }
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
        private System.Timers.Timer timeToNextPrayerTimer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private Label nextPrayerInTimeLabel;
        private Label currentPrayerNameLabel;
        private System.Windows.Forms.Timer timer1;
        private ToolStripMenuItem stopAdhanPlaybackToolStripMenuItem;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer sunMoonOvalShapeContainer;
        private Microsoft.VisualBasic.PowerPacks.OvalShape backgroundDarkBlueOvalShape;
        private ButtonNoPadding exitButton;
        private PictureBox middleIconPictureBox;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Microsoft.VisualBasic.PowerPacks.OvalShape sunMoonOvalShape;
        private Label currentPrayerLabel;
        private Label nextPrayerInLabel;
        private Label nextPrayerLabel;
        private Label nextPrayerNameLabel;
        private Label currentPrayerSinceLabel;
        private Label currentPrayerSinceTimeLabel;
        private ButtonNoPadding resizeButton;
    }

    public class CustomMenuRenderer : ToolStripProfessionalRenderer
    {
        public CustomMenuRenderer() : base(new CustomColorTable()) { }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (!e.Item.Selected && !e.Item.Pressed)
            {
                base.OnRenderMenuItemBackground(e);
                return;
            }

            Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
            Color fillColor;

            if (e.Item.Pressed)
            {
                fillColor = Color.FromArgb(60, 84, 116); // Color when item is clicked
            }
            else if (e.Item.Selected)
            {
                fillColor = Color.FromArgb(40, 64, 96); // Hover color
            }
            else
            {
                fillColor = Color.Transparent;
            }

            using (SolidBrush brush = new SolidBrush(fillColor))
            {
                e.Graphics.FillRectangle(brush, rc);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = Color.White; // Set text color to white for all states
            base.OnRenderItemText(e);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            // Do nothing to avoid drawing the border
        }
    }

    public class CustomColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected
        {
            get { return Color.FromArgb(40, 64, 96); } // Hover color
        }

        public override Color MenuItemBorder
        {
            get { return Color.Transparent; }
        }

        public override Color ToolStripDropDownBackground
        {
            get { return Color.FromArgb(6, 36, 72); }
        }

        public override Color MenuBorder
        {
            get { return Color.Transparent; }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get { return Color.FromArgb(40, 64, 96); } // Hover color
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get { return Color.FromArgb(40, 64, 96); } // Hover color
        }

        public override Color MenuItemPressedGradientBegin
        {
            get { return Color.FromArgb(60, 84, 116); } // Pressed color
        }

        public override Color MenuItemPressedGradientEnd
        {
            get { return Color.FromArgb(60, 84, 116); } // Pressed color
        }
    }
}

