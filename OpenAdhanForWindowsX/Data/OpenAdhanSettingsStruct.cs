namespace OpenAdhanForWindowsX
{
    public struct OpenAdhanSettingsStruct
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
        public int timeZone { get; set; }
        public int calculationMethod { get; set; }
        public int juristicMethod { get; set; }
        public int fajrAdjustment { get; set; }
        public int shurookAdjustment { get; set; }
        public int dhuhrAdjustment { get; set; }
        public int asrAdjustment { get; set; }
        public int maghribAdjustment { get; set; }
        public int ishaAdjustment { get; set; }
        public bool playAdhanAtPrayerTimes { get; set; }
        public bool sendNotificationAtPrayerTimes { get; set; }
        public bool minimizeAtStartup { get; set; }
        public bool bismillahAtStartup { get; set; }
        public bool automaticDaylightSavingsAdjustment { get; set; }
        public string normalAdhanFilePath { get; set; }
        public string fajrAdhanFilePath { get; set; }
        public string openAdhanInstalledVersion { get; set; }
        public bool muteAllAppsOnAdhanPlaying { get; set; }
        public bool alwaysOnTop { get; set; }
        public bool smallSizeAlwaysOnTop { get; set; }

    }
}
