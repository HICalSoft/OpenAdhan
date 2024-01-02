using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenAdhanForWindowsX
{
    public partial class Settings : Form
    {
        string normalAdhanPath = "";
        string fajrAdhanPath = "";
        Form1 mainForm;
        public Settings(Form1 form1)
        {
            InitializeComponent();
            updateAdhanSettingsFromRegistry();
            this.mainForm = form1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void updateAdhanSettingsFromRegistry()
        {
            // Load actual settings from registry.
            RegistrySettingsHandler rsh = new RegistrySettingsHandler(false);
            OpenAdhanSettingsStruct oass = rsh.LoadOpenAdhanSettings();
            this.comboBox1.SelectedIndex = oass.calculationMethod;
            this.comboBox2.SelectedIndex = oass.juristicMethod;
            this.textBox1.Text = oass.latitude;
            this.textBox2.Text = oass.longitude;
            int localTimeZone = getLocalTimeZone();
            this.label21.Text = $"Time Zone (Local={localTimeZone}):";
            this.label21.Refresh();
            this.textBox9.Text = oass.timeZone.ToString();
            this.textBox3.Text = oass.fajrAdjustment.ToString();
            this.textBox4.Text = oass.shurookAdjustment.ToString();
            this.textBox5.Text = oass.dhuhrAdjustment.ToString();
            this.textBox6.Text = oass.asrAdjustment.ToString();
            this.textBox7.Text = oass.maghribAdjustment.ToString();
            this.textBox8.Text = oass.ishaAdjustment.ToString();
            if(oass.playAdhanAtPrayerTimes)
            {
                this.radioButton1.Checked = true;
                this.radioButton2.Checked = false;
            } else
            {
                this.radioButton1.Checked = false;
                this.radioButton2.Checked = true;
            }
            if (oass.sendNotificationAtPrayerTimes)
            {
                this.radioButton3.Checked = true;
                this.radioButton4.Checked = false;
            }
            else
            {
                this.radioButton3.Checked = false;
                this.radioButton4.Checked = true;
            }
            if (oass.minimizeAtStartup)
            {
                this.radioButton5.Checked = true;
                this.radioButton6.Checked = false;
            }
            else
            {
                this.radioButton5.Checked = false;
                this.radioButton6.Checked = true;
            }
            if (oass.bismillahAtStartup)
            {
                this.radioButton7.Checked = true;
                this.radioButton8.Checked = false;
            }
            else
            {
                this.radioButton7.Checked = false;
                this.radioButton8.Checked = true;
            }
            this.button3.Text = Path.GetFileName(oass.normalAdhanFilePath);
            this.normalAdhanPath = oass.normalAdhanFilePath;
            this.button4.Text = Path.GetFileName(oass.fajrAdhanFilePath);
            this.fajrAdhanPath = oass.fajrAdhanFilePath;

        }

        private int getLocalTimeZone()
        {
            DateTime cc = DateTime.Now;

            int y = cc.Year;
            int m = cc.Month;
            int d = cc.Day;
            int tz = TimeZone.CurrentTimeZone.GetUtcOffset(new DateTime(y, m, d)).Hours;
            return tz;
        }

        private void saveAdhanSettingsToRegistry()
        {
            // Load actual settings from registry.
            RegistrySettingsHandler rsh = new RegistrySettingsHandler(false);
            OpenAdhanSettingsStruct oass = new OpenAdhanSettingsStruct();
            oass.calculationMethod = this.comboBox1.SelectedIndex;
            oass.juristicMethod = this.comboBox2.SelectedIndex;
            oass.latitude = this.textBox1.Text;
            oass.longitude = this.textBox2.Text;
            if (int.TryParse(this.textBox9.Text, out int timeZone))
            {
                oass.timeZone = timeZone;
            }
            else
            {
                MessageBox.Show($"Unable to save settings due to invalid timeZone value specified: {this.textBox9.Text}", "Unable to Save Settings!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (int.TryParse(this.textBox3.Text, out int fajrAdj))
            {
                oass.fajrAdjustment = fajrAdj;
            }
            else
            {
                MessageBox.Show($"Unable to save settings due to invalid fajrAdjustment value specified: {this.textBox3.Text}", "Unable to Save Settings!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (int.TryParse(this.textBox4.Text, out int shurookAdj))
            {
                oass.shurookAdjustment = shurookAdj;
            }
            else
            {
                MessageBox.Show($"Unable to save settings due to invalid shurookAdjustment value specified: {this.textBox4.Text}", "Unable to Save Settings!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (int.TryParse(this.textBox5.Text, out int dhuhrAdj))
            {
                oass.dhuhrAdjustment = dhuhrAdj;
            }
            else
            {
                MessageBox.Show($"Unable to save settings due to invalid dhuhrAdjustment value specified: {this.textBox5.Text}", "Unable to Save Settings!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (int.TryParse(this.textBox6.Text, out int asrAdj))
            {
                oass.asrAdjustment = asrAdj;
            }
            else
            {
                MessageBox.Show($"Unable to save settings due to invalid asrAdjustment value specified: {this.textBox6.Text}", "Unable to Save Settings!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (int.TryParse(this.textBox7.Text, out int maghribAdj))
            {
                oass.maghribAdjustment = maghribAdj;
            }
            else
            {
                MessageBox.Show($"Unable to save settings due to invalid maghribAdjustment value specified: {this.textBox7.Text}", "Unable to Save Settings!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (int.TryParse(this.textBox8.Text, out int ishaAdj))
            {
                oass.ishaAdjustment = ishaAdj;
            }
            else
            {
                MessageBox.Show($"Unable to save settings due to invalid ishaAdjustment value specified: {this.textBox8.Text}", "Unable to Save Settings!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(this.radioButton1.Checked == true)
            {
                oass.playAdhanAtPrayerTimes = true;
            }
            else
            {
                oass.playAdhanAtPrayerTimes = false;
            }
            if(this.radioButton3.Checked == true)
            {
                oass.sendNotificationAtPrayerTimes = true;
            }
            else
            {
                oass.sendNotificationAtPrayerTimes = false;
            }
            if (this.radioButton5.Checked == true)
            {
                oass.minimizeAtStartup = true;
            }
            else
            {
                oass.minimizeAtStartup = false;
            }
            if (this.radioButton7.Checked == true)
            {
                oass.bismillahAtStartup = true;
            }
            else
            {
                oass.bismillahAtStartup = false;
            }
            oass.normalAdhanFilePath = this.normalAdhanPath;
            oass.fajrAdhanFilePath = this.fajrAdhanPath;
            try
            {
                rsh.saveSettingsToRegistry(oass);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to save settings to registry! Error: {ex.Message}", "Unable to Save Settings!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // success -- close the settings window.
            this.Close();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            PrayerTimesControl pti = PrayerTimesControl.Instance;
            try
            {
                pti.playAdhan(this.normalAdhanPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to play Adhan! Error: {ex.Message}", "Unable to Play Adhan Sound!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            PrayerTimesControl pti = PrayerTimesControl.Instance;
            pti.StopAdhan();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            PrayerTimesControl pti = PrayerTimesControl.Instance;
            pti.StopAdhan();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            PrayerTimesControl pti = PrayerTimesControl.Instance;
            try
            {
                pti.playFajrAdhan(this.fajrAdhanPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to play Fajr Adhan! Error: {ex.Message}", "Unable to Play Fajr Adhan Sound!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.normalAdhanPath = openFileDialog1.FileName;
                this.button3.Text = Path.GetFileName(this.normalAdhanPath);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.fajrAdhanPath = openFileDialog1.FileName;
                this.button4.Text = Path.GetFileName(this.fajrAdhanPath);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PrayerTimesControl pti = PrayerTimesControl.Instance;
            this.button3.Text = Path.GetFileName(pti.getDefaultNormalAdhanFilePath());
            this.normalAdhanPath = pti.getDefaultNormalAdhanFilePath();
            this.button4.Text = Path.GetFileName(pti.getDefaultFajrAdhanFilePath());
            this.fajrAdhanPath = pti.getDefaultFajrAdhanFilePath();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveAdhanSettingsToRegistry();
            PrayerTimesControl pti = PrayerTimesControl.Instance;
            pti.clearAdhans();
            pti.scheduleAdhans(this.mainForm);
            this.mainForm.updatePrayerTimesDisplay();

        }
    }
}
