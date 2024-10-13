using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using Microsoft.Win32;

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

    }
    class RegistrySettingsHandler
    {
        private const string RegistryKeyPath = @"Software\OpenAdhan";
        private bool console = false;

        // Registry keys
        public const string latitudeKey = "Latitude";
        public const string longitudeKey = "Longitude";
        public const string timezoneKey = "timeZone";
        public const string calculationMethodKey = "CalculationMethod";
        public const string juristicMethodKey = "JuristicMethod";
        public const string fajrAdjustmentKey = "FajrAdjustment";
        public const string shurookAdjustmentKey = "ShurookAdjustment";
        public const string dhuhrAdjustmentKey = "DhuhrAdjustment";
        public const string asrAdjustmentKey = "AsrAdjustment";
        public const string maghribAdjustmentKey = "MaghribAdjustment";
        public const string ishaAdjustmentKey = "IshaAdjustment";
        public const string minimizeOnStartupKey = "MinimizeOnStartup";
        public const string bismillahOnStartupKey = "BismillahOnStartup";
        public const string automaticDaylightSavingsAdjustmentKey = "AutomaticDaylightSavingsAdjustment";
        public const string initialInstallFlagKey = "InitialInstallFlag";
        public const string playAdhanAtPrayerTimesKey = "PlayAdhanAtPrayerTimes";
        public const string sendNotificationAtPrayerTimesKey = "SendNotificationAtPrayerTimes";
        public const string normalAdhanFilePathkey = "NormalAdhan";
        public const string fajrAdhanFilePathKey = "FajrAdhan";

        public const string windowPositionXKey = "WindowPositionX";
        public const string windowPositionYKey = "WindowPositionY";

        public RegistrySettingsHandler(bool console)
        {
            this.console = console;
        }
        public RegistrySettingsHandler() { 
        }

        public string LoadRegistryValue(string valueName)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(RegistryKeyPath))
                {
                    if (key != null)
                    {
                        // Check if the value exists before attempting to retrieve it
                        if (key.GetValue(valueName) != null)
                        {
                            return key.GetValue(valueName).ToString();
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                string error = $"Error accessing LocalMachine registry: {ex.Message}";
                if(this.console)
                {
                    Console.WriteLine(error);
                }
                else
                {
                    throw ex;
                }
            }

            return null; // Return null if the value or key doesn't exist or if there's an error accessing it
        }
        public int SafeLoadIntRegistryValue(string valueName)
        {
            try
            {
                if (int.TryParse(LoadRegistryValue(valueName), out int loadedInt))
                {
                    return loadedInt;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Loading Int Val: {ex.Message}");
                return 0;
            }
        }
        public float SafeLoadFloatRegistryValue(string valueName)
        {
            try
            {
                if (float.TryParse(LoadRegistryValue(valueName), out float loadedFloat))
                {
                    return loadedFloat;
                }
                else
                {
                    return 0.0F;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Loading Float Val: {ex.Message}");
                return 0;
            }
        }
        public bool SafeLoadBoolRegistryValue(string valueName)
        {
            try
            {
                if (LoadRegistryValue(valueName).Equals("0"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Loading Bool Val: {ex.Message}");
                return true;
            }
        }


        private void InstallRegistryValueWithPermissions(string valueName, string value, string type)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(RegistryKeyPath, RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    // Grant read and write permissions to all users
                    RegistrySecurity registrySecurity = new RegistrySecurity();
                    registrySecurity.AddAccessRule(new RegistryAccessRule("Users", RegistryRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                    key.SetAccessControl(registrySecurity);

                    RegistryValueKind kind = RegistryValueKind.String;
                    if(type.Equals("int"))
                    {
                        kind = RegistryValueKind.DWord;
                    }
                    
                    // Only set value if it's null -- IE it doesn't exist already.
                    if (key.GetValue(valueName, null) == null)
                    {
                        key.SetValue(valueName, value, kind);
                    }
                    
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                string error = $"Error accessing LocalMachine registry: {ex.Message}";
                if (this.console)
                {
                    Console.WriteLine(error);
                }
                else
                {
                    throw ex;
                }

            }
        }

        public void SaveRegistryValue(string valueName, string value, string type)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(RegistryKeyPath, RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    RegistryValueKind kind = RegistryValueKind.String;
                    if (type.Equals("int"))
                    {
                        kind = RegistryValueKind.DWord;
                    }

                    key.SetValue(valueName, value, kind);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                string error = $"Error accessing LocalMachine registry: {ex.Message}";
                if (this.console)
                {
                    Console.WriteLine(error);
                }
                else
                {
                    throw ex;
                }

            }
        }

        public bool saveSettingsToRegistry(OpenAdhanSettingsStruct oass)
        {
            try
            {
                SaveRegistryValue(latitudeKey, oass.latitude, "string");
                SaveRegistryValue(longitudeKey, oass.longitude, "string");
                SaveRegistryValue(timezoneKey, oass.timeZone.ToString(), "int");
                SaveRegistryValue(calculationMethodKey, oass.calculationMethod.ToString(), "int");
                SaveRegistryValue(juristicMethodKey, oass.juristicMethod.ToString(), "int");
                SaveRegistryValue(fajrAdjustmentKey, oass.fajrAdjustment.ToString(), "int");
                SaveRegistryValue(shurookAdjustmentKey, oass.shurookAdjustment.ToString(), "int");
                SaveRegistryValue(dhuhrAdjustmentKey, oass.dhuhrAdjustment.ToString(), "int");
                SaveRegistryValue(asrAdjustmentKey, oass.asrAdjustment.ToString(), "int");
                SaveRegistryValue(maghribAdjustmentKey, oass.maghribAdjustment.ToString(), "int");
                SaveRegistryValue(ishaAdjustmentKey, oass.ishaAdjustment.ToString(), "int");
                if(oass.playAdhanAtPrayerTimes) 
                    SaveRegistryValue(playAdhanAtPrayerTimesKey, "1", "int");
                else
                    SaveRegistryValue(playAdhanAtPrayerTimesKey, "0", "int");
                if(oass.sendNotificationAtPrayerTimes)
                    SaveRegistryValue(sendNotificationAtPrayerTimesKey, "1", "int");
                else
                    SaveRegistryValue(sendNotificationAtPrayerTimesKey, "0", "int");
                if (oass.minimizeAtStartup)
                    SaveRegistryValue(minimizeOnStartupKey, "1", "int");
                else
                    SaveRegistryValue(minimizeOnStartupKey, "0", "int");
                if (oass.bismillahAtStartup)
                    SaveRegistryValue(bismillahOnStartupKey, "1", "int");
                else
                    SaveRegistryValue(bismillahOnStartupKey, "0", "int");
                if (oass.automaticDaylightSavingsAdjustment)
                    SaveRegistryValue(automaticDaylightSavingsAdjustmentKey, "1", "int");
                else
                    SaveRegistryValue(automaticDaylightSavingsAdjustmentKey, "0", "int");
                SaveRegistryValue(normalAdhanFilePathkey, oass.normalAdhanFilePath, "string");
                SaveRegistryValue(fajrAdhanFilePathKey, oass.fajrAdhanFilePath, "string");
            }
            catch (Exception e)
            {
                throw e;
            }


            return true;
        }
        public void InstallOpenAdhanRegistryKeys()
        {
            InstallRegistryValueWithPermissions(latitudeKey, "21.4224779", "string");
            InstallRegistryValueWithPermissions(longitudeKey, "39.8251832", "string");
            InstallRegistryValueWithPermissions(timezoneKey, "3", "int");
            InstallRegistryValueWithPermissions(calculationMethodKey, "4", "int");
            InstallRegistryValueWithPermissions(juristicMethodKey, "0", "int");
            InstallRegistryValueWithPermissions(fajrAdjustmentKey, "0", "int");
            InstallRegistryValueWithPermissions(shurookAdjustmentKey, "0", "int");
            InstallRegistryValueWithPermissions(dhuhrAdjustmentKey, "0", "int");
            InstallRegistryValueWithPermissions(asrAdjustmentKey, "0", "int");
            InstallRegistryValueWithPermissions(maghribAdjustmentKey, "0", "int");
            InstallRegistryValueWithPermissions(ishaAdjustmentKey, "0", "int");
            InstallRegistryValueWithPermissions(playAdhanAtPrayerTimesKey, "1", "int");
            InstallRegistryValueWithPermissions(sendNotificationAtPrayerTimesKey, "0", "int");
            InstallRegistryValueWithPermissions(minimizeOnStartupKey, "1", "int");
            InstallRegistryValueWithPermissions(bismillahOnStartupKey, "1", "int");
            if (System.TimeZoneInfo.Local.SupportsDaylightSavingTime)
            {
                InstallRegistryValueWithPermissions(automaticDaylightSavingsAdjustmentKey, "1", "int");
            }
            else
            {
                InstallRegistryValueWithPermissions(automaticDaylightSavingsAdjustmentKey, "0", "int");
            }
            InstallRegistryValueWithPermissions(initialInstallFlagKey, "1", "int");
            InstallRegistryValueWithPermissions(normalAdhanFilePathkey, System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\islam_sobhi_adhan.wav"), "string");
            InstallRegistryValueWithPermissions(fajrAdhanFilePathKey, System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Athan_1_alafasy_Fajr.wav"), "string");
            InstallRegistryValueWithPermissions(windowPositionXKey, "100", "int");
            InstallRegistryValueWithPermissions(windowPositionYKey, "100", "int");
        }
        public OpenAdhanSettingsStruct LoadOpenAdhanSettings()
        {
            OpenAdhanSettingsStruct oass = new OpenAdhanSettingsStruct();
            oass.latitude = LoadRegistryValue(latitudeKey);
            oass.longitude = LoadRegistryValue(longitudeKey);
            int.TryParse(LoadRegistryValue(timezoneKey), out int timeZoneInt);
            oass.timeZone = timeZoneInt;
            int.TryParse(LoadRegistryValue(calculationMethodKey), out int calcMethodInt);
            oass.calculationMethod = calcMethodInt;
            int.TryParse(LoadRegistryValue(juristicMethodKey), out int juristicMethodInt);
            oass.juristicMethod = juristicMethodInt;
            int.TryParse(LoadRegistryValue(fajrAdjustmentKey), out int fajrAdjustInt);
            oass.fajrAdjustment = fajrAdjustInt;
            int.TryParse(LoadRegistryValue(shurookAdjustmentKey), out int shurookAdjustInt);
            oass.shurookAdjustment = shurookAdjustInt;
            int.TryParse(LoadRegistryValue(dhuhrAdjustmentKey), out int dhuhrAdjustInt);
            oass.dhuhrAdjustment = dhuhrAdjustInt;
            int.TryParse(LoadRegistryValue(asrAdjustmentKey), out int asrAdjustInt);
            oass.asrAdjustment = asrAdjustInt;
            int.TryParse(LoadRegistryValue(maghribAdjustmentKey), out int maghribAdjustInt);
            oass.maghribAdjustment = maghribAdjustInt;
            int.TryParse(LoadRegistryValue(ishaAdjustmentKey), out int ishaAdjustInt);
            oass.ishaAdjustment = ishaAdjustInt;
            int.TryParse(LoadRegistryValue(playAdhanAtPrayerTimesKey), out int playAdhanInt);
            oass.playAdhanAtPrayerTimes = (playAdhanInt != 0);
            int.TryParse(LoadRegistryValue(sendNotificationAtPrayerTimesKey), out int notificationInt);
            oass.sendNotificationAtPrayerTimes = (notificationInt != 0);
            int.TryParse(LoadRegistryValue(minimizeOnStartupKey), out int minimizeInt);
            oass.minimizeAtStartup = (minimizeInt != 0);
            int.TryParse(LoadRegistryValue(bismillahOnStartupKey), out int bismillahInt);
            oass.bismillahAtStartup = (bismillahInt != 0);
            int.TryParse(LoadRegistryValue(automaticDaylightSavingsAdjustmentKey), out int autoDaylightInt);
            oass.automaticDaylightSavingsAdjustment = (autoDaylightInt != 0);
            oass.normalAdhanFilePath = LoadRegistryValue(normalAdhanFilePathkey);
            oass.fajrAdhanFilePath = LoadRegistryValue(fajrAdhanFilePathKey);
            return oass;
        }

        public void SaveWindowPosition(int x, int y)
        {
            SaveRegistryValue(windowPositionXKey, x.ToString(), "int");
            SaveRegistryValue(windowPositionYKey, y.ToString(), "int");
        }

        public (int x, int y) LoadWindowPosition()
        {
            int x = SafeLoadIntRegistryValue(windowPositionXKey);
            int y = SafeLoadIntRegistryValue(windowPositionYKey);
            return (x, y);
        }

        public static string[] getNormalAdhansFilePaths()
        {
            return ListFilesInDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "adhans-normal"));
        }

        public static string[] getFajrAdhansFilePaths()
        {
            return ListFilesInDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "adhans-fajr"));
        }

        public static string[] ListFilesInDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                // Get all files in the directory
                string[] filePaths = Directory.GetFiles(directoryPath);
                return filePaths;
            }
            else
            {
                throw new DirectoryNotFoundException($"The directory '{directoryPath}' does not exist.");
            }
        }

    }
}
