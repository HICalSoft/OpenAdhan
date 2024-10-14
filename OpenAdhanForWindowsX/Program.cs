using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenAdhanForWindowsX
{
    static class Program
    {
        static Form1 form;
        static PrayerTimesControl prayerTimesControl;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new Form1();
            prayerTimesControl = PrayerTimesControl.Instance;
            prayerTimesControl.scheduleAdhans(form);
            form.updatePrayerTimesDisplay();

            // Register the PowerModeChanged event (for waking up from sleep -- https://github.com/HICalSoft/OpenAdhan/issues/1)
            SystemEvents.PowerModeChanged += OnPowerChange;


            RegistrySettingsHandler rsh = new RegistrySettingsHandler(false);

            // This should handle DST changes (untested :P )  https://github.com/HICalSoft/OpenAdhan/issues/6 
            // Add condition only if automatic DST adjustment is enabled
            if (rsh.SafeLoadBoolRegistryValue(RegistrySettingsHandler.automaticDaylightSavingsAdjustmentKey))
            {
                SystemEvents.TimeChanged += OnTimeChange;
            }


            if (rsh.SafeLoadBoolRegistryValue(RegistrySettingsHandler.minimizeOnStartupKey) &&
                !rsh.SafeLoadBoolRegistryValue(RegistrySettingsHandler.initialInstallFlagKey))
            {
                Application.Run();
            }
            else
            {
                Application.Run(form);
            }

        }

        private static void OnPowerChange(object s, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Resume:
                    // The computer has woken up from sleep.
                    refreshPrayerTimes();
                    break;
            }
        }

        private static void OnTimeChange(object s, EventArgs e)
        {
            refreshPrayerTimes();
        }

        private static void refreshPrayerTimes()
        {
            prayerTimesControl.calculatePrayerTimes();
            prayerTimesControl.scheduleAdhans(form);
            form.updatePrayerTimesDisplay();
        }
    }

    public sealed class PrayerTimesControl
    {
        private static readonly PrayerTimesControl instance = new PrayerTimesControl();
        private IWavePlayer waveOut;
        private AudioFileReader audioFileReader;
        public int fajr = 0;
        public int shurook = 1;
        public int dhuhr = 2;
        public int asr = 3;
        public int sunset = 4;
        public int maghrib = 5;
        public int isha = 6;
        RegistrySettingsHandler rsh = null;

        static PrayerTimesControl()
        {
        }
        private PrayerTimesControl()
        {
            rsh = new RegistrySettingsHandler(false);
        }

        public static PrayerTimesControl Instance
        {
            get
            {
                return instance;
            }
        }

        DateTime[] prayer_datetimes = null;

        public void calculatePrayerTimes(DateTime? specificDate = null, OpenAdhanSettingsStruct? oass = null)
        {
            double lat;
            double lon;
            int calcMethod;
            int juristicMethod;
            bool autoDst;
            int y = 0, m = 0, d = 0, tz = 0;


            if (oass != null)
            {
                lat = float.Parse(oass.Value.latitude);
                lon = float.Parse(oass.Value.longitude);
                calcMethod = oass.Value.calculationMethod;
                juristicMethod = oass.Value.juristicMethod;
                tz = oass.Value.timeZone;
                autoDst = oass.Value.automaticDaylightSavingsAdjustment;
            }
            else
            {
                lat = this.rsh.SafeLoadFloatRegistryValue(RegistrySettingsHandler.latitudeKey);
                lon = this.rsh.SafeLoadFloatRegistryValue(RegistrySettingsHandler.longitudeKey);
                calcMethod = this.rsh.SafeLoadIntRegistryValue(RegistrySettingsHandler.calculationMethodKey);
                juristicMethod = this.rsh.SafeLoadIntRegistryValue(RegistrySettingsHandler.juristicMethodKey);
                tz = this.rsh.SafeLoadIntRegistryValue(RegistrySettingsHandler.timezoneKey);
                autoDst = this.rsh.SafeLoadBoolRegistryValue(RegistrySettingsHandler.automaticDaylightSavingsAdjustmentKey);
            }


            PrayTime p = new PrayTime();


            // Use specificDate if provided, otherwise use DateTime.Now
            DateTime cc = specificDate ?? DateTime.Now;

            y = cc.Year;
            m = cc.Month;
            d = cc.Day;

            p.setCalcMethod(calcMethod);
            p.setAsrMethod(juristicMethod);

            // Debug RSH
            //System.Diagnostics.Debug.WriteLine("RSH CalcMethod: " + calcMethod);
            //System.Diagnostics.Debug.WriteLine("RSH JuristicMethod: " + juristicMethod);
            //System.Diagnostics.Debug.WriteLine("RSH Lat: " + lat);
            //System.Diagnostics.Debug.WriteLine("RSH Lon: " + lon);
            //System.Diagnostics.Debug.WriteLine("RSH TZ: " + tz);
            //System.Diagnostics.Debug.WriteLine("RSH Date is DST: " + TimeZoneInfo.Local.IsDaylightSavingTime(cc));


            String[] prayer_times_strings = p.getDatePrayerTimes(y, m, d, lon, lat, tz);
            // This only returns HH:MM -- all date info is lost.

            //System.Diagnostics.Debug.WriteLine("PrayTimes Fajr: " + prayer_times_strings[fajr]);  

            double daylightSavingsTimeAdjustment = 0.0;
            if (autoDst)
            {
                if (TimeZoneInfo.Local.IsDaylightSavingTime(cc))
                {
                    daylightSavingsTimeAdjustment = +60.0;
                }
            }

            // if specific date isn't null (IE, not calculating for today):
            if (specificDate != null)
            {
                prayer_times_strings[fajr] = cc.Date.ToShortDateString() + " " + prayer_times_strings[fajr];
                prayer_times_strings[shurook] = cc.Date.ToShortDateString() + " " + prayer_times_strings[shurook];
                prayer_times_strings[dhuhr] = cc.Date.ToShortDateString() + " " + prayer_times_strings[dhuhr];
                prayer_times_strings[asr] = cc.Date.ToShortDateString() + " " + prayer_times_strings[asr];
                prayer_times_strings[sunset] = cc.Date.ToShortDateString() + " " + prayer_times_strings[sunset];
                prayer_times_strings[maghrib] = cc.Date.ToShortDateString() + " " + prayer_times_strings[maghrib];
                prayer_times_strings[isha] = cc.Date.ToShortDateString() + " " + prayer_times_strings[isha];
            }

            // Parse DateTimes
            this.prayer_datetimes = new DateTime[7];
            try
            {
                this.prayer_datetimes[fajr] = DateTime.Parse(prayer_times_strings[fajr]).AddMinutes(this.rsh.SafeLoadFloatRegistryValue(RegistrySettingsHandler.fajrAdjustmentKey)).AddMinutes(daylightSavingsTimeAdjustment);
                this.prayer_datetimes[shurook] = DateTime.Parse(prayer_times_strings[shurook]).AddMinutes(this.rsh.SafeLoadFloatRegistryValue(RegistrySettingsHandler.shurookAdjustmentKey)).AddMinutes(daylightSavingsTimeAdjustment);
                this.prayer_datetimes[dhuhr] = DateTime.Parse(prayer_times_strings[dhuhr]).AddMinutes(this.rsh.SafeLoadFloatRegistryValue(RegistrySettingsHandler.dhuhrAdjustmentKey)).AddMinutes(daylightSavingsTimeAdjustment);
                this.prayer_datetimes[asr] = DateTime.Parse(prayer_times_strings[asr]).AddMinutes(this.rsh.SafeLoadFloatRegistryValue(RegistrySettingsHandler.asrAdjustmentKey)).AddMinutes(daylightSavingsTimeAdjustment);
                this.prayer_datetimes[sunset] = DateTime.Parse(prayer_times_strings[sunset]).AddMinutes(daylightSavingsTimeAdjustment);
                this.prayer_datetimes[maghrib] = DateTime.Parse(prayer_times_strings[maghrib]).AddMinutes(this.rsh.SafeLoadFloatRegistryValue(RegistrySettingsHandler.maghribAdjustmentKey)).AddMinutes(daylightSavingsTimeAdjustment);
                this.prayer_datetimes[isha] = DateTime.Parse(prayer_times_strings[isha]).AddMinutes(this.rsh.SafeLoadFloatRegistryValue(RegistrySettingsHandler.ishaAdjustmentKey)).AddMinutes(daylightSavingsTimeAdjustment);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unable to calculate prayer times with specified latitude ({lat}), longitude ({lon}), and timezone ({tz}) values, error: {ex.Message}");
                this.prayer_datetimes = null;
                return;
            }
            //System.Diagnostics.Debug.WriteLine("Calc Fajr: " + this.prayer_datetimes[fajr]);

        }

        public DateTime[] getPrayerDateTimes()
        {
            if (this.prayer_datetimes is null) { this.calculatePrayerTimes(); }

            if (this.prayer_datetimes is null)
            {
                this.prayer_datetimes = new DateTime[] { DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue };
                MessageBox.Show("Error getting prayer times.\n\nPlease change your latitude and longitude settings in the settings menu.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return this.prayer_datetimes;
        }

        public void scheduleAdhans(Form1 form)
        {
            if (this.prayer_datetimes is null) { this.calculatePrayerTimes(); }
            TaskScheduler.Instance.ClearTimers();

            if(this.prayer_datetimes is null)
            {
                //Unable to calculate prayer times!
                return;
            }

            DateTime fajr = this.prayer_datetimes[this.fajr];
            DateTime shurook = this.prayer_datetimes[this.shurook];
            DateTime dhuhr = this.prayer_datetimes[this.dhuhr];
            DateTime asr = this.prayer_datetimes[this.asr];
            DateTime maghrib = this.prayer_datetimes[this.maghrib];
            DateTime isha = this.prayer_datetimes[this.isha];

            // Debugging Only
            //System.Diagnostics.Debug.WriteLine("Should Play At: " + DateTime.Now.Hour + ":" + (DateTime.Now.Minute + 1));
            //TaskScheduler.Instance.ScheduleTask(DateTime.Now.Hour, DateTime.Now.Minute+1, 24.0, () =>
            //{
            //    playFajrAdhan();
            //});

            Tuple<PrayerInfo, PrayerInfo> prayerInfo = getNextPrayerNotification();
            PrayerInfo nextPrayer = prayerInfo.Item1;
            PrayerInfo currentPrayer = prayerInfo.Item2;
            invokeFormBoldnessUpdate(form, nextPrayer.Name);

            TaskScheduler.Instance.ScheduleTask(fajr.Hour, fajr.Minute, 24.0, () =>
            {
                RegistrySettingsHandler rsh = new RegistrySettingsHandler(false);
                if(rsh.SafeLoadBoolRegistryValue(RegistrySettingsHandler.playAdhanAtPrayerTimesKey))
                {
                    this.playFajrAdhan();
                }
                if(rsh.SafeLoadBoolRegistryValue(RegistrySettingsHandler.sendNotificationAtPrayerTimesKey))
                {
                    form.SendPrayerNotification("Fajr",fajr);
                }
                invokeFormBoldnessUpdate(form, "Shurook");
                this.delayedUpdatePrayerTimes(form);
            });
            TaskScheduler.Instance.ScheduleTask(shurook.Hour, shurook.Minute, 24.0, () =>
            {
                if (rsh.SafeLoadBoolRegistryValue(RegistrySettingsHandler.sendNotificationAtPrayerTimesKey))
                {
                    form.SendPrayerNotification("Shurook", shurook);
                }
                invokeFormBoldnessUpdate(form, "Dhuhr");
                this.delayedUpdatePrayerTimes(form);
            });
            TaskScheduler.Instance.ScheduleTask(dhuhr.Hour, dhuhr.Minute, 24.0, () =>
            {
                RegistrySettingsHandler rsh = new RegistrySettingsHandler(false);
                if (rsh.SafeLoadBoolRegistryValue(RegistrySettingsHandler.playAdhanAtPrayerTimesKey))
                {
                    this.playAdhan();
                }
                if (rsh.SafeLoadBoolRegistryValue(RegistrySettingsHandler.sendNotificationAtPrayerTimesKey))
                {
                    form.SendPrayerNotification("Dhuhr", dhuhr);
                }
                invokeFormBoldnessUpdate(form, "Asr");
                this.delayedUpdatePrayerTimes(form);
            });
            TaskScheduler.Instance.ScheduleTask(asr.Hour, asr.Minute, 24.0, () =>
            {
                RegistrySettingsHandler rsh = new RegistrySettingsHandler(false);
                if (rsh.SafeLoadBoolRegistryValue(RegistrySettingsHandler.playAdhanAtPrayerTimesKey))
                {
                    this.playAdhan();
                }
                if (rsh.SafeLoadBoolRegistryValue(RegistrySettingsHandler.sendNotificationAtPrayerTimesKey))
                {
                    form.SendPrayerNotification("Asr", asr);
                }

                invokeFormBoldnessUpdate(form, "Maghrib");
                this.delayedUpdatePrayerTimes(form);
            });
            TaskScheduler.Instance.ScheduleTask(maghrib.Hour, maghrib.Minute, 24.0, () =>
            {
                RegistrySettingsHandler rsh = new RegistrySettingsHandler(false);
                if (rsh.SafeLoadBoolRegistryValue(RegistrySettingsHandler.playAdhanAtPrayerTimesKey))
                {
                    this.playAdhan();
                }
                if (rsh.SafeLoadBoolRegistryValue(RegistrySettingsHandler.sendNotificationAtPrayerTimesKey))
                {
                    form.SendPrayerNotification("Maghrib", maghrib);
                }
                invokeFormBoldnessUpdate(form, "Isha");
                this.delayedUpdatePrayerTimes(form);
            });
            TaskScheduler.Instance.ScheduleTask(isha.Hour, isha.Minute, 24.0, () =>
            {
                RegistrySettingsHandler rsh = new RegistrySettingsHandler(false);
                if (rsh.SafeLoadBoolRegistryValue(RegistrySettingsHandler.playAdhanAtPrayerTimesKey))
                {
                    this.playAdhan();
                }
                if (rsh.SafeLoadBoolRegistryValue(RegistrySettingsHandler.sendNotificationAtPrayerTimesKey))
                {
                    form.SendPrayerNotification("Isha", isha);
                }
                invokeFormBoldnessUpdate(form,"Fajr");
                this.delayedUpdatePrayerTimes(form);
            });
        }

        private void invokeFormBoldnessUpdate(Form1 form, string prayer)
        {
            if (form.InvokeRequired)
            {
                Action safeUpdate = delegate { form.SetBold(prayer); };
                form.Invoke(safeUpdate);
            }
            else
            {
                form.SetBold(prayer);
            }
        }
        public void clearAdhans()
        {
            TaskScheduler.Instance.ClearTimers();
            this.prayer_datetimes = null;
        }
        public void playAdhan(string adhan=null)
        {
            string adhan_file = null;
            if (adhan is null)
                adhan = rsh.LoadRegistryValue(RegistrySettingsHandler.normalAdhanFilePathkey);

            string[] builtInAdhans = RegistrySettingsHandler.getNormalAdhansFilePaths();

            if (adhan.Equals("Random"))
            {
                Random random = new Random();
                int index = random.Next(builtInAdhans.Length);
                adhan_file = builtInAdhans[index];
            }
            else // specific adhan selected, or custom adhan
            {
                foreach (string adhan_filepath in builtInAdhans)
                {
                    if (adhan_filepath.Contains(adhan))
                    {
                        adhan_file = adhan_filepath;
                        break;
                    }
                }
                if (adhan_file is null) // custom adhan
                {
                    adhan_file = adhan;
                }
            }
            playAdhanFile(adhan_file);
        }
        public void playFajrAdhan(string adhan_fajr = null)
        {
            string adhan_file = null;
            if (adhan_fajr is null)
                adhan_fajr = rsh.LoadRegistryValue(RegistrySettingsHandler.fajrAdhanFilePathKey);

            string[] builtInAdhans = RegistrySettingsHandler.getFajrAdhansFilePaths();

            if (adhan_fajr.Equals("Random"))
            {
                Random random = new Random();
                int index = random.Next(builtInAdhans.Length);
                adhan_file = builtInAdhans[index];
            }
            else // specific adhan selected, or custom adhan
            {
                foreach (string adhan_filepath in builtInAdhans)
                {
                    if (adhan_filepath.Contains(adhan_fajr))
                    {
                        adhan_file = adhan_filepath;
                        break;
                    }
                }
                if (adhan_file is null) // custom adhan
                {
                    adhan_file = adhan_fajr;
                }
            }
            playAdhanFile(adhan_file);
        }
        public void playAdhanFile(string audio_file = null)
        {
            if (audio_file is null)
                return;
            // Verify the audio file exists on disk
            if (!System.IO.File.Exists(audio_file))
            {
                System.Diagnostics.Debug.WriteLine($"Audio file not found: {audio_file}");
                throw new Exception($"Audio file not found: {audio_file}");
            }
            try
            {
                StopAdhan(); // Stop any currently playing audio
                audioFileReader = new AudioFileReader(audio_file);
                waveOut = new WaveOutEvent();
                waveOut.Init(audioFileReader);
                waveOut.Play();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                throw e;
            }
        }
        public void StopAdhan()
        {
            try
            {
                if (waveOut != null)
                {
                    waveOut.Stop();
                    waveOut.Dispose();
                    waveOut = null;
                }
                if (audioFileReader != null)
                {
                    audioFileReader.Dispose();
                    audioFileReader = null;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }
        private void delayedUpdatePrayerTimes(Form1 form)
        {
            DateTime delayedTime = DateTime.Now.AddMinutes(15); 
            TaskScheduler.Instance.ScheduleTask(delayedTime.Hour, delayedTime.Minute, 24.0, () =>
            {
                this.calculatePrayerTimes();
                this.scheduleAdhans(form);
                if(form.InvokeRequired)
                {
                    Action safeUpdate = delegate { form.updatePrayerTimesDisplay(); };
                    form.Invoke(safeUpdate);
                }
                else
                {
                    form.updatePrayerTimesDisplay();
                }
            });
        }
        public string getDefaultNormalAdhanFilePath()
        {
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\islam_sobhi_adhan.wav");
        }
        public string getDefaultFajrAdhanFilePath()
        {
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Athan_1_alafasy_Fajr.wav");
        }
        public string getDefaultBismillahFilePath()
        {
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Mishary97Bismillah.wav");
        }

        public Tuple<PrayerInfo, PrayerInfo> getNextPrayerNotification(DateTime? specificDate = null)
        {
            PrayerTimesControl pti = PrayerTimesControl.Instance;
            DateTime[] prayerDateTimes = pti.getPrayerDateTimes();
            DateTime now = specificDate ?? DateTime.Now;

            TimeSpan timeToFajr = getPrayerTimeDuration(prayerDateTimes[pti.fajr], now);
            TimeSpan timeToShurook = getPrayerTimeDuration(prayerDateTimes[pti.shurook], now);
            TimeSpan timeToDhuhr = getPrayerTimeDuration(prayerDateTimes[pti.dhuhr], now);
            TimeSpan timeToAsr = getPrayerTimeDuration(prayerDateTimes[pti.asr], now);
            TimeSpan timeToMaghrib = getPrayerTimeDuration(prayerDateTimes[pti.maghrib], now);
            TimeSpan timeToIsha = getPrayerTimeDuration(prayerDateTimes[pti.isha], now);

            PrayerInfo nextPrayer = null;
            PrayerInfo currentPrayer = null;

            if (timeToFajr < timeToShurook && timeToIsha > timeToFajr)
            {
                nextPrayer = new PrayerInfo { Name = "Fajr", TimeTo = timeToFajr };
                currentPrayer = new PrayerInfo { Name = "Isha", TimeSince = now - prayerDateTimes[pti.isha].AddDays(now < prayerDateTimes[pti.isha] ? -1 : 0) };
            }
            else if (timeToShurook < timeToDhuhr && timeToFajr > timeToShurook)
            {
                nextPrayer = new PrayerInfo { Name = "Shurook", TimeTo = timeToShurook };
                currentPrayer = new PrayerInfo { Name = "Fajr", TimeSince = now - prayerDateTimes[pti.fajr] };
            }
            else if (timeToDhuhr < timeToAsr && timeToShurook > timeToDhuhr)
            {
                nextPrayer = new PrayerInfo { Name = "Dhuhr", TimeTo = timeToDhuhr };
                currentPrayer = new PrayerInfo { Name = "Shurook", TimeSince = now - prayerDateTimes[pti.shurook] };
            }
            else if (timeToAsr < timeToMaghrib && timeToDhuhr > timeToAsr)
            {
                nextPrayer = new PrayerInfo { Name = "Asr", TimeTo = timeToAsr };
                currentPrayer = new PrayerInfo { Name = "Dhuhr", TimeSince = now - prayerDateTimes[pti.dhuhr] };
            }
            else if (timeToMaghrib < timeToIsha && timeToAsr > timeToMaghrib)
            {
                nextPrayer = new PrayerInfo { Name = "Maghrib", TimeTo = timeToMaghrib };
                currentPrayer = new PrayerInfo { Name = "Asr", TimeSince = now - prayerDateTimes[pti.asr] };
            }
            else if (timeToIsha < timeToFajr && timeToMaghrib > timeToIsha)
            {
                nextPrayer = new PrayerInfo { Name = "Isha", TimeTo = timeToIsha };
                currentPrayer = new PrayerInfo { Name = "Maghrib", TimeSince = now - prayerDateTimes[pti.maghrib] };
            }
            else
            {
                nextPrayer = new PrayerInfo { Name = "Fail", TimeTo = timeToFajr };
                currentPrayer = new PrayerInfo { Name = "Fail", TimeSince = TimeSpan.Zero };
            }

            return new Tuple<PrayerInfo, PrayerInfo>(nextPrayer, currentPrayer);
        }


        public TimeSpan getPrayerTimeDuration(DateTime nextPrayer, DateTime now)
        {
            if (nextPrayer.CompareTo(now) < 0)
            {
                nextPrayer = nextPrayer.AddDays(1);
            }
            return (nextPrayer.Subtract(now)).Duration();
        }


    }

    public class PrayerInfo
    {
        public string Name { get; set; }
        public TimeSpan TimeTo { get; set; }
        public TimeSpan TimeSince { get; set; }
    }

    public class TaskScheduler
    {
        // https://stackoverflow.com/questions/3243348/how-to-call-a-method-daily-at-specific-time-in-c

        private static TaskScheduler _instance;
        private List<System.Threading.Timer> timers = new List<System.Threading.Timer>();

        private TaskScheduler() { }

        public static TaskScheduler Instance => _instance ?? (_instance = new TaskScheduler());

        public void ScheduleTask(int hour, int min, double intervalInHour, Action task)
        {
            DateTime now = DateTime.Now;
            DateTime firstRun = new DateTime(now.Year, now.Month, now.Day, hour, min, 0, 0);
            if (now > firstRun)
            {
                firstRun = firstRun.AddDays(1);
            }

            TimeSpan timeToGo = firstRun - now;
            if (timeToGo <= TimeSpan.Zero)
            {
                timeToGo = TimeSpan.Zero;
            }

            var timer = new System.Threading.Timer(x =>
            {
                task.Invoke();
            }, null, timeToGo, TimeSpan.FromHours(intervalInHour));

            timers.Add(timer);
        }
        public void ClearTimers()
        {
            for (int i = 0; i < this.timers.Count; i++)
            {
                this.timers[i].Dispose();
            }
            this.timers.RemoveRange(0, this.timers.Count);
        }
    }
}
