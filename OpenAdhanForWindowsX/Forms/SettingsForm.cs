using OpenAdhanForWindowsX.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenAdhanForWindowsX
{
    public partial class SettingsForm : Form
    {
        string normalAdhanPath = "";
        string fajrAdhanPath = "";
        MainAppForm mainForm;
        public SettingsForm(MainAppForm form1)
        {
            InitializeComponent();

            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            // Fixes https://github.com/HICalSoft/OpenAdhan/issues/3
            if (this.Height >= screenHeight * 0.8)
            {
                // Set the form's height to 80% of the screen's height.
                this.Height = (int)(screenHeight * 0.8);
            }

            // Load actual settings from registry.
            RegistrySettingsHandler rsh = new RegistrySettingsHandler(false);
            OpenAdhanSettingsStruct oass = rsh.LoadOpenAdhanSettings();

            string[] normalAdhans = new string[0];
            string[] fajrAdhans = new string[0];
            try
            {
                normalAdhans = RegistrySettingsHandler.getNormalAdhansFilePaths();
                fajrAdhans = RegistrySettingsHandler.getFajrAdhansFilePaths();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to load Adhan files! Error: {ex.Message}", "Unable to Load Adhan Files!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            PopulateComboBoxWithFilesAndRandom(comboBox3, normalAdhans, oass.normalAdhanFilePath);
            PopulateComboBoxWithFilesAndRandom(comboBox4, fajrAdhans, oass.fajrAdhanFilePath);
            comboBox3.SelectedIndexChanged += ComboBox3_SelectedIndexChanged;
            comboBox4.SelectedIndexChanged += ComboBox4_SelectedIndexChanged;
            updateAdhanSettingsFromRegistry(oass);
            this.mainForm = form1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void updateAdhanSettingsFromRegistry(OpenAdhanSettingsStruct oass)
        {
            this.comboBox1.SelectedIndex = oass.calculationMethod;
            this.comboBox2.SelectedIndex = oass.juristicMethod;
            this.textBox1.Text = oass.latitude;
            this.textBox2.Text = oass.longitude;
            int localTimeZone = getLocalTimeZone();
            this.label21.Text = $"Time Zone Non-DST (Local={localTimeZone}):";
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
            if (oass.automaticDaylightSavingsAdjustment)
            {
                this.radioButton9.Checked = true;
                this.radioButton10.Checked = false;
            }
            else
            {
                this.radioButton9.Checked = false;
                this.radioButton10.Checked = true;
            }
            string normalAdhanFileName = Path.GetFileName(oass.normalAdhanFilePath);
            if (this.comboBox3.Items.Contains(normalAdhanFileName))
            {
                this.comboBox3.SelectedItem = normalAdhanFileName;
            }
            else
            {
                // Saved config in registry not recognized, select Random
                this.comboBox3.SelectedItem = "Random";
                this.normalAdhanPath = "Random";
            }
            this.normalAdhanPath = oass.normalAdhanFilePath;
            string fajrAdhanFileName = Path.GetFileName(oass.fajrAdhanFilePath);
            if (this.comboBox4.Items.Contains(fajrAdhanFileName))
            {
                this.comboBox4.SelectedItem = fajrAdhanFileName;
            }
            else
            {
                this.comboBox4.SelectedItem = "Random";
                this.fajrAdhanPath = "Random";
            }
            this.fajrAdhanPath = oass.fajrAdhanFilePath;
            if (oass.muteAllAppsOnAdhanPlaying)
            {
                this.muteAllAppsOnAdhanYesRadio.Checked = true;
                this.muteAllAppsOnAdhanNoRadio.Checked = false;
            }
            else
            {
                this.muteAllAppsOnAdhanYesRadio.Checked = false;
                this.muteAllAppsOnAdhanNoRadio.Checked = true;
            }
        }

        private int getLocalTimeZone()
        {
            int tz = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
            if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now))
            {
                // TODO -- dynamically adjust for non-standard DST changes being less or more than 1 hour shift.
                tz = tz - 1; // hard coded 1 hour DST shift.
            }
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
            if (this.radioButton9.Checked == true)
            {
                oass.automaticDaylightSavingsAdjustment = true;
            }
            else
            {
                oass.automaticDaylightSavingsAdjustment = false;
            }
            if (this.muteAllAppsOnAdhanYesRadio.Checked)
            {
                oass.muteAllAppsOnAdhanPlaying = true;
            }
            else
            {
                oass.muteAllAppsOnAdhanPlaying = false;
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

        private void button5_Click(object sender, EventArgs e)
        {
            PrayerTimesControl pti = PrayerTimesControl.Instance;
            this.comboBox3.SelectedItem = "Random";
            this.normalAdhanPath = "Random";
            this.comboBox4.SelectedItem = "Random";
            this.fajrAdhanPath = "Random";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveAdhanSettingsToRegistry();
            PrayerTimesControl pti = PrayerTimesControl.Instance;
            pti.clearAdhans();
            pti.scheduleAdhans(this.mainForm);
            this.mainForm.updatePrayerTimesDisplay();

        }

        private void PopulateComboBoxWithFilesAndRandom(ComboBox comboBox, string[] builtInAdhanFilePaths, string adhanRegistryValue)
        {
                comboBox.Items.Clear();
                comboBox.Items.Add("Random");
                bool adhanIsBuiltIn = false;

                if (adhanRegistryValue == "Random")
                {
                    adhanIsBuiltIn = true;
                }

                foreach (string file in builtInAdhanFilePaths)
                {
                    string adhanFilename = Path.GetFileName(file);
                    comboBox.Items.Add(adhanFilename);
                    if ( adhanRegistryValue.Contains(adhanFilename) )
                    {
                        adhanIsBuiltIn = true;
                    }
                }
                if ( !adhanIsBuiltIn )
                {
                    comboBox.Items.Add(Path.GetFileName(adhanRegistryValue));
                }
                comboBox.Items.Add("Browse...");
        }

        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem.ToString() == "Browse...")
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFile = openFileDialog.FileName;
                    comboBox3.Items.Insert(comboBox3.Items.Count - 1, Path.GetFileName(selectedFile));
                    comboBox3.SelectedItem = Path.GetFileName(selectedFile);
                    this.normalAdhanPath = selectedFile;
                }
                else
                {
                    comboBox3.SelectedItem = "Random"; // Reset to "Random" if no file is selected
                    this.normalAdhanPath = "Random";
                }
            }
            else
            {
                this.normalAdhanPath = comboBox3.SelectedItem.ToString();
            }
        }

        private void ComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedItem.ToString() == "Browse...")
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFile = openFileDialog.FileName;
                    comboBox4.Items.Insert(comboBox4.Items.Count - 1, Path.GetFileName(selectedFile));
                    comboBox4.SelectedItem = Path.GetFileName(selectedFile);
                    this.fajrAdhanPath = selectedFile;
                }
                else
                {
                    comboBox4.SelectedItem = "Random"; // Reset to "Random" if no file is selected
                    this.fajrAdhanPath = "Random";
                }
            }
            else
            {
                this.fajrAdhanPath = comboBox4.SelectedItem.ToString();
            }
        }


    }
}
