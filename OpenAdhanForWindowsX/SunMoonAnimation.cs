using Microsoft.VisualBasic.PowerPacks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenAdhanForWindowsX
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Microsoft.VisualBasic.PowerPacks;

    public class SunMoonAnimation : IDisposable
    {
        private OvalShape ovalShape2;
        private Timer animationTimer;
        private Timer updateTimer;
        private PrayerTimesControl prayerTimesControl;
        private DateTime[] prayerTimes;
        private const int FormWidth = 675;
        private const int YAxis = 200;
        private int[] xPositions = { 60, 200, 325, 455, 575 };

        private bool isDebugMode = false;
        private DateTime debugCurrentTime;

        public SunMoonAnimation(OvalShape shape, PrayerTimesControl pti)
        {
            ovalShape2 = shape;
            prayerTimesControl = pti;

            animationTimer = new Timer();
            animationTimer.Interval = 1000; // Update animation every second
            animationTimer.Tick += AnimationTimer_Tick;

            updateTimer = new Timer();
            updateTimer.Interval = 60000; // Check for updates every minute
            updateTimer.Tick += UpdateTimer_Tick;
        }

        public void Start()
        {
            UpdatePrayerTimes();
            animationTimer.Start();
            updateTimer.Start();
        }

        public void ToggleDebugMode(bool enable)
        {
            isDebugMode = enable;
            if (isDebugMode)
            {
                debugCurrentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                updateTimer.Interval = 1000; // Update every second in debug mode
            }
            else
            {
                updateTimer.Interval = 60000; // Reset to normal interval
            }
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (isDebugMode)
            {
                debugCurrentTime = debugCurrentTime.AddMinutes(60);
                UpdatePrayerTimes();
            }
            else
            {
                UpdatePrayerTimes();
            }
        }

        public void UpdatePrayerTimes()
        {
            DateTime[] newPrayerTimes = prayerTimesControl.getPrayerDateTimes();
            if (prayerTimes == null || !AreSamePrayerTimes(prayerTimes, newPrayerTimes))
            {
                prayerTimes = newPrayerTimes;
                // Notify the main form that prayer times have changed
                PrayerTimesUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool AreSamePrayerTimes(DateTime[] times1, DateTime[] times2)
        {
            if (times1.Length != times2.Length) return false;
            for (int i = 0; i < times1.Length; i++)
            {
                if (times1[i] != times2[i]) return false;
            }
            return true;
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            UpdatePosition();
            UpdateAppearance();
        }

        private void UpdatePosition()
        {
            DateTime now = isDebugMode ? debugCurrentTime : DateTime.Now;
            int x = CalculateXPosition(now);
            ovalShape2.Location = new Point(x, YAxis);
        }

        private int CalculateXPosition(DateTime now)
        {
            if (prayerTimes == null) return xPositions[0];

            if (now < prayerTimes[0]) // Before Fajr
            {
                var twelveamtime = new DateTime(prayerTimes[0].Year, prayerTimes[0].Month, prayerTimes[0].Day, 0, 0, 0);
                return InterpolatePosition(twelveamtime, prayerTimes[0], now, -100, xPositions[0]);
            }    
            else if (now < prayerTimes[2]) // Between Fajr and Dhuhr
                return InterpolatePosition(prayerTimes[0], prayerTimes[2], now, xPositions[0], xPositions[1]);
            else if (now < prayerTimes[3]) // Between Dhuhr and Asr
                return InterpolatePosition(prayerTimes[2], prayerTimes[3], now, xPositions[1], xPositions[2]);
            else if (now < prayerTimes[5]) // Between Asr and Maghrib
                return InterpolatePosition(prayerTimes[3], prayerTimes[5], now, xPositions[2], xPositions[3]);
            else if (now < prayerTimes[6]) // Between Maghrib and Isha
                return InterpolatePosition(prayerTimes[5], prayerTimes[6], now, xPositions[3], xPositions[4]);
            else // After Isha
                return InterpolatePosition(prayerTimes[6], prayerTimes[0].AddDays(1), now, xPositions[4], (FormWidth + 100));
        }

        private int InterpolatePosition(DateTime start, DateTime end, DateTime current, int startX, int endX)
        {
            double totalSeconds = (end - start).TotalSeconds;
            double elapsedSeconds = (current - start).TotalSeconds;
            double fraction = elapsedSeconds / totalSeconds;
            return (int)(startX + fraction * (endX - startX));
        }

        private void UpdateAppearance()
        {
            if (prayerTimes == null) return;

            DateTime now = isDebugMode ? debugCurrentTime : DateTime.Now;
            if (now < prayerTimes[5] && now > prayerTimes[0]) // Daytime
            {
                double dayProgress = (now - prayerTimes[0]).TotalSeconds / (prayerTimes[5] - prayerTimes[0]).TotalSeconds;
                UpdateSunAppearance(dayProgress);
            }
            else // Nighttime
            {
                UpdateMoonAppearance();
            }
        }

        private void UpdateSunAppearance(double progress)
        {
            Color orange = Color.Orange;
            Color yellow = Color.Yellow;

            Color sunColor;
            if (progress < 0.5) // Before Dhuhr
            {
                sunColor = InterpolateColor(orange, yellow, progress * 2);
            }
            else // After Dhuhr
            {
                sunColor = InterpolateColor(yellow, orange, (progress - 0.5) * 2);
            }

            ovalShape2.BackColor = sunColor;
            ovalShape2.BorderColor = ControlPaint.Light(sunColor);
            ovalShape2.FillColor = sunColor;
            ovalShape2.FillGradientColor = ControlPaint.LightLight(sunColor);
            ovalShape2.FillGradientStyle = FillGradientStyle.Central;
        }

        private Color InterpolateColor(Color start, Color end, double factor)
        {
            int r = (int)(start.R + factor * (end.R - start.R));
            int g = (int)(start.G + factor * (end.G - start.G));
            int b = (int)(start.B + factor * (end.B - start.B));
            return Color.FromArgb(255, r, g, b);
        }

        private void UpdateMoonAppearance()
        {
            ovalShape2.BackColor = Color.Silver;
            ovalShape2.BorderColor = Color.Gray;
            ovalShape2.FillColor = Color.LightGray;
            ovalShape2.FillGradientColor = Color.White;
            ovalShape2.FillGradientStyle = FillGradientStyle.ForwardDiagonal;
        }

        public void Dispose()
        {
            animationTimer.Stop();
            animationTimer.Dispose();
            updateTimer.Stop();
            updateTimer.Dispose();
        }

        public DateTime GetCurrentTime()
        {
            return isDebugMode ? debugCurrentTime : DateTime.Now;
        }

        public event EventHandler PrayerTimesUpdated;
    }
}
