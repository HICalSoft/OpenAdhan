using global::Microsoft.VisualStudio.TestTools.UnitTesting;
using global::System;
using global::System.Collections.Generic;
using global::System.IO;
using global::System.Linq;
using global::System.Net.Http;
using global::System.Threading;
using global::System.Threading.Tasks;
using OpenAdhanForWindowsX;
using System.Diagnostics;

namespace OpenAdhanTests
{
    [TestClass]
    public class PrayerTimeNotificationTests
    {
        [TestMethod]
        public void TestTimeToNextPrayerNotification_DSTOn()
        {
            OpenAdhanSettingsStruct oass = new OpenAdhanSettingsStruct();
            // Eugene, OR test location
            oass.latitude = "-123.0868";
            oass.longitude = "44.0521";
            oass.juristicMethod = 0;
            oass.calculationMethod = 4;
            oass.timeZone = -7;
            oass.automaticDaylightSavingsAdjustment = true;
            OpenAdhanForWindowsX.PrayerTimesControl pti = OpenAdhanForWindowsX.PrayerTimesControl.Instance;
            DateTime june22_2024_2_15am_beforeFajr = new DateTime(2024, 6, 22, 2, 15, 0);
            DateTime june22_2024_3_11_30_am_beforeShurook = new DateTime(2024, 6, 22, 3, 11, 30);
            DateTime june22_2024_12_pm_beforeDhuhr = new DateTime(2024, 6, 22, 12, 0, 0);
            DateTime june22_2024_3_36pm_beforeAsr = new DateTime(2024, 6, 22, 15, 36, 15);
            DateTime june22_2024_8_15pm_beforeMaghrib = new DateTime(2024, 6, 22, 20, 15, 0);
            DateTime june22_2024_9_45pm_beforeIsha = new DateTime(2024, 6, 22, 21, 45, 0);


            pti.calculatePrayerTimes(june22_2024_2_15am_beforeFajr, oass);

            // Debug Print Fajr Time
            Debug.WriteLine("Fajr Time: " + pti.getPrayerDateTimes()[pti.fajr]);
            Debug.WriteLine("Shurook Time: " + pti.getPrayerDateTimes()[pti.shurook]);
            Debug.WriteLine("Dhuhr Time: " + pti.getPrayerDateTimes()[pti.dhuhr]);
            Debug.WriteLine("Asr Time: " + pti.getPrayerDateTimes()[pti.asr]);
            Debug.WriteLine("Maghrib Time: " + pti.getPrayerDateTimes()[pti.maghrib]);
            Debug.WriteLine("Isha Time: " + pti.getPrayerDateTimes()[pti.isha]);

            Tuple<string, TimeSpan> nextPrayerTupleFajr = pti.getNextPrayerNotification(june22_2024_2_15am_beforeFajr);
            Assert.AreEqual("Fajr", nextPrayerTupleFajr.Item1);
            Assert.AreEqual(new TimeSpan(0, 43, 0), nextPrayerTupleFajr.Item2);


            Tuple<string, TimeSpan> nextPrayerTupleShurook = pti.getNextPrayerNotification(june22_2024_3_11_30_am_beforeShurook);
            Assert.AreEqual("Shurook", nextPrayerTupleShurook.Item1);
            Assert.AreEqual(new TimeSpan(2, 18, 30), nextPrayerTupleShurook.Item2);
            
            Tuple<string, TimeSpan> nextPrayerTupleDhuhr = pti.getNextPrayerNotification(june22_2024_12_pm_beforeDhuhr);
            Assert.AreEqual("Dhuhr", nextPrayerTupleDhuhr.Item1);
            Assert.AreEqual(new TimeSpan(1, 15, 0), nextPrayerTupleDhuhr.Item2);

            Tuple<string, TimeSpan> nextPrayerTupleAsr = pti.getNextPrayerNotification(june22_2024_3_36pm_beforeAsr);
            Assert.AreEqual("Asr", nextPrayerTupleAsr.Item1);
            Assert.AreEqual(new TimeSpan(1, 45, 45), nextPrayerTupleAsr.Item2);

            Tuple<string, TimeSpan> nextPrayerTupleMaghrib = pti.getNextPrayerNotification(june22_2024_8_15pm_beforeMaghrib);
            Assert.AreEqual("Maghrib", nextPrayerTupleMaghrib.Item1);
            Assert.AreEqual(new TimeSpan(0, 44, 0), nextPrayerTupleMaghrib.Item2);

            Tuple<string, TimeSpan> nextPrayerTupleIsha = pti.getNextPrayerNotification(june22_2024_9_45pm_beforeIsha);
            Assert.AreEqual("Isha", nextPrayerTupleIsha.Item1);
            Assert.AreEqual(new TimeSpan(0, 44, 0), nextPrayerTupleIsha.Item2);
        }

        [TestMethod]
        public void TestTimeToNextPrayerNotification_DST_Off()
        {
            OpenAdhanSettingsStruct oass = new OpenAdhanSettingsStruct();
            // Eugene, OR test location
            oass.latitude = "-123.0868";
            oass.longitude = "44.0521";
            oass.juristicMethod = 0;
            oass.calculationMethod = 4;
            oass.timeZone = -7;
            oass.automaticDaylightSavingsAdjustment = true;
            OpenAdhanForWindowsX.PrayerTimesControl pti = OpenAdhanForWindowsX.PrayerTimesControl.Instance;


            DateTime dec15_2024_1pm = new DateTime(2024, 12, 15, 13, 0, 0);

            DateTime dec15_2024_4_30_beforeFajr = new DateTime(2024, 12, 15, 4, 30, 0);
            DateTime dec15_2024_7_00_beforeShurook = new DateTime(2024, 12, 15, 7, 0, 0);
            DateTime dec15_2024_12_07_59_beforeDhuhr = new DateTime(2024, 12, 15, 12, 7, 59);
            DateTime dec15_2024_2_15_beforeAsr = new DateTime(2024, 12, 15, 14, 15, 0);
            DateTime dec15_2024_4_34_beforeMaghrib = new DateTime(2024, 12, 15, 16, 34, 0);
            DateTime dec15_2024_5_59_59_beforeIsha = new DateTime(2024, 12, 15, 17, 59, 59);

            pti.calculatePrayerTimes(dec15_2024_1pm, oass);

            // Debug Print Fajr Time
            Debug.WriteLine("Fajr Time: " + pti.getPrayerDateTimes()[pti.fajr]);
            Debug.WriteLine("Shurook Time: " + pti.getPrayerDateTimes()[pti.shurook]);
            Debug.WriteLine("Dhuhr Time: " + pti.getPrayerDateTimes()[pti.dhuhr]);
            Debug.WriteLine("Asr Time: " + pti.getPrayerDateTimes()[pti.asr]);
            Debug.WriteLine("Maghrib Time: " + pti.getPrayerDateTimes()[pti.maghrib]);
            Debug.WriteLine("Isha Time: " + pti.getPrayerDateTimes()[pti.isha]);


            Tuple<string, TimeSpan> nextPrayerTupleFajr = pti.getNextPrayerNotification(dec15_2024_4_30_beforeFajr);
            Assert.AreEqual("Fajr", nextPrayerTupleFajr.Item1);
            Assert.AreEqual(new TimeSpan(1, 23, 0), nextPrayerTupleFajr.Item2);

            Tuple<string, TimeSpan> nextPrayerTupleShurook = pti.getNextPrayerNotification(dec15_2024_7_00_beforeShurook);
            Assert.AreEqual("Shurook", nextPrayerTupleShurook.Item1);
            Assert.AreEqual(new TimeSpan(0, 41, 0), nextPrayerTupleShurook.Item2);

            Tuple<string, TimeSpan> nextPrayerTupleDhuhr = pti.getNextPrayerNotification(dec15_2024_12_07_59_beforeDhuhr);
            Assert.AreEqual("Dhuhr", nextPrayerTupleDhuhr.Item1);
            Assert.AreEqual(new TimeSpan(0, 0, 1), nextPrayerTupleDhuhr.Item2);

            Tuple<string, TimeSpan> nextPrayerTupleAsr = pti.getNextPrayerNotification(dec15_2024_2_15_beforeAsr);
            Assert.AreEqual("Asr", nextPrayerTupleAsr.Item1);
            Assert.AreEqual(new TimeSpan(0, 2, 0), nextPrayerTupleAsr.Item2);

            Tuple<string, TimeSpan> nextPrayerTupleMaghrib = pti.getNextPrayerNotification(dec15_2024_4_34_beforeMaghrib);
            Assert.AreEqual("Maghrib", nextPrayerTupleMaghrib.Item1);
            Assert.AreEqual(new TimeSpan(0, 1, 0), nextPrayerTupleMaghrib.Item2);

            Tuple<string, TimeSpan> nextPrayerTupleIsha = pti.getNextPrayerNotification(dec15_2024_5_59_59_beforeIsha);
            Assert.AreEqual("Isha", nextPrayerTupleIsha.Item1);
            Assert.AreEqual(new TimeSpan(0, 5, 1), nextPrayerTupleIsha.Item2);
        }
    }
}