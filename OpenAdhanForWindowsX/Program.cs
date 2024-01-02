using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenAdhanForWindowsX
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form = new Form1();
            PrayerTimesControl.Instance.scheduleAdhans(form);
            form.updatePrayerTimesDisplay();
            RegistrySettingsHandler rsh = new RegistrySettingsHandler(false);
            if (rsh.SafeLoadBoolRegistryValue(RegistrySettingsHandler.minimizeOnStartupKey))
            {
                Application.Run();
            }
            else
            {
                Application.Run(form);
            }

        }
    }

    public sealed class PrayerTimesControl
    {
        private static readonly PrayerTimesControl instance = new PrayerTimesControl();
        private System.Media.SoundPlayer adhanPlayer;
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

        public void calculatePrayerTimes()
        {
            PrayTime p = new PrayTime();
            double lat = this.rsh.SafeLoadFloatRegistryValue(RegistrySettingsHandler.latitudeKey);
            double lon = this.rsh.SafeLoadFloatRegistryValue(RegistrySettingsHandler.longitudeKey);
            int y = 0, m = 0, d = 0, tz = 0;

            tz = this.rsh.SafeLoadIntRegistryValue(RegistrySettingsHandler.timezoneKey);

            DateTime cc = DateTime.Now;

            y = cc.Year;
            m = cc.Month;
            d = cc.Day;

            p.setCalcMethod(this.rsh.SafeLoadIntRegistryValue(RegistrySettingsHandler.calculationMethodKey));
            p.setAsrMethod(this.rsh.SafeLoadIntRegistryValue(RegistrySettingsHandler.juristicMethodKey));
            String[] prayer_times_strings = p.getDatePrayerTimes(y, m, d, lon, lat, tz);


            // Parse DateTimes
            this.prayer_datetimes = new DateTime[7];
            // this.date_times[fajr] = DateTime.Parse(prayer_times[fajr]);
            // this.date_times[shurook] = DateTime.Parse(prayer_times[shurook]);
            // this.date_times[dhuhr] = DateTime.Parse(prayer_times[dhuhr]);
            // this.date_times[asr] = DateTime.Parse(prayer_times[asr]);
            // this.date_times[sunset] = DateTime.Parse(prayer_times[sunset]);
            // this.date_times[maghrib] = DateTime.Parse(prayer_times[maghrib]);
            // this.date_times[isha] = DateTime.Parse(prayer_times[isha]);
            try
            {
                this.prayer_datetimes[fajr] = DateTime.Parse(prayer_times_strings[fajr]).AddMinutes(this.rsh.SafeLoadFloatRegistryValue(RegistrySettingsHandler.fajrAdjustmentKey));
                this.prayer_datetimes[shurook] = DateTime.Parse(prayer_times_strings[shurook]).AddMinutes(this.rsh.SafeLoadFloatRegistryValue(RegistrySettingsHandler.shurookAdjustmentKey));
                this.prayer_datetimes[dhuhr] = DateTime.Parse(prayer_times_strings[dhuhr]).AddMinutes(this.rsh.SafeLoadFloatRegistryValue(RegistrySettingsHandler.dhuhrAdjustmentKey));
                this.prayer_datetimes[asr] = DateTime.Parse(prayer_times_strings[asr]).AddMinutes(this.rsh.SafeLoadFloatRegistryValue(RegistrySettingsHandler.asrAdjustmentKey));
                this.prayer_datetimes[sunset] = DateTime.Parse(prayer_times_strings[sunset]);
                this.prayer_datetimes[maghrib] = DateTime.Parse(prayer_times_strings[maghrib]).AddMinutes(this.rsh.SafeLoadFloatRegistryValue(RegistrySettingsHandler.maghribAdjustmentKey));
                this.prayer_datetimes[isha] = DateTime.Parse(prayer_times_strings[isha]).AddMinutes(this.rsh.SafeLoadFloatRegistryValue(RegistrySettingsHandler.ishaAdjustmentKey));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unable to calculate prayer times with specified latitude ({lat}), longitude ({lon}), and timezone ({tz}) values, error: {ex.Message}");
                this.prayer_datetimes = null;
                return;
            }

        }

        public DateTime[] getPrayerDateTimes()
        {
            if (this.prayer_datetimes is null) { this.calculatePrayerTimes(); }
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

            Tuple<string, TimeSpan> nextPrayerTuple = getNextPrayerNotification();
            invokeFormBoldnessUpdate(form, nextPrayerTuple.Item1);

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
                form.SetBold("Dhuhr");
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
            if (adhan is null)
                adhan = rsh.LoadRegistryValue(RegistrySettingsHandler.normalAdhanFilePathkey);

            try
            {
                adhanPlayer = new System.Media.SoundPlayer(adhan);
                adhanPlayer.Play();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                throw e;
            }
        }
        public void playFajrAdhan(string adhan_fajr = null)
        {
            if (adhan_fajr is null)
                adhan_fajr = rsh.LoadRegistryValue(RegistrySettingsHandler.fajrAdhanFilePathKey); ;
            try
            {
                adhanPlayer = new System.Media.SoundPlayer(adhan_fajr);
                adhanPlayer.Play();
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
                if(!(adhanPlayer is null))
                    adhanPlayer.Stop();
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

        public Tuple<string, TimeSpan> getNextPrayerNotification()
        {
            PrayerTimesControl pti = PrayerTimesControl.Instance;
            DateTime[] prayerDateTimes = pti.getPrayerDateTimes();
            DateTime now = DateTime.Now;

            TimeSpan timeToFajr = getPrayerTimeDuration(prayerDateTimes[pti.fajr], now);
            TimeSpan timeToShurook = getPrayerTimeDuration(prayerDateTimes[pti.shurook], now);
            TimeSpan timeToDhuhr = getPrayerTimeDuration(prayerDateTimes[pti.dhuhr], now);
            TimeSpan timeToAsr = getPrayerTimeDuration(prayerDateTimes[pti.asr], now);
            TimeSpan timeToMaghrib = getPrayerTimeDuration(prayerDateTimes[pti.maghrib], now);
            TimeSpan timeToIsha = getPrayerTimeDuration(prayerDateTimes[pti.isha], now);

            //System.Diagnostics.Debug.WriteLine("Fajr: " + timeToFajr.ToString());
            //System.Diagnostics.Debug.WriteLine("Shurook: " + timeToShurook.ToString());
            //System.Diagnostics.Debug.WriteLine("Dhuhr: " + timeToDhuhr.ToString());
            //System.Diagnostics.Debug.WriteLine("Asr: " + timeToAsr.ToString());
            //System.Diagnostics.Debug.WriteLine("Maghrib: " + timeToMaghrib.ToString());
            //System.Diagnostics.Debug.WriteLine("Isha: " + timeToIsha.ToString());

            if (timeToFajr < timeToShurook && timeToIsha > timeToFajr)
            {
                return new Tuple<string, TimeSpan>("Fajr", timeToFajr);
            }

            if (timeToShurook < timeToDhuhr && timeToFajr > timeToShurook)
            {
                return new Tuple<string, TimeSpan>("Shurook", timeToShurook);
            }
            if (timeToDhuhr < timeToAsr && timeToShurook > timeToDhuhr)
            {
                return new Tuple<string, TimeSpan>("Dhuhr", timeToDhuhr);
            }
            if (timeToAsr < timeToMaghrib && timeToDhuhr > timeToAsr)
            {
                return new Tuple<string, TimeSpan>("Asr", timeToAsr);
            }
            if (timeToMaghrib < timeToIsha && timeToAsr > timeToMaghrib)
            {
                return new Tuple<string, TimeSpan>("Maghrib", timeToMaghrib);
            }
            if (timeToIsha < timeToFajr && timeToMaghrib > timeToIsha)
            {
                return new Tuple<string, TimeSpan>("Isha", timeToIsha);
            }
            return new Tuple<string, TimeSpan>("Fail", timeToFajr);
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
