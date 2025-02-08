using Microsoft.Win32;
using OpenAdhanForWindowsX.Managers;
using System;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OpenAdhanForWindowsX
{
    static class Program
    {
        static MainAppForm form;
        static PrayerTimesControl prayerTimesControl;
        static AudioManager audioManager = null;

        [STAThread]
        static void Main()
        {
            RegistrySettingsHandler rsh = new RegistrySettingsHandler(false);
            CheckForUpdate(rsh.LoadRegistryValue(RegistrySettingsHandler.openAdhanInstalledVersionKey));

            audioManager = new AudioManager(System.Diagnostics.Process.GetCurrentProcess().ProcessName);

            // Handle app closing and killing
            _handler = new ConsoleCtrlHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            prayerTimesControl = PrayerTimesControl.Instance;
            prayerTimesControl.SetAudioManager(audioManager);
            form = new MainAppForm();           
            prayerTimesControl.scheduleAdhans(form);
            form.updatePrayerTimesDisplay();

            // Register the PowerModeChanged event (for waking up from sleep -- https://github.com/HICalSoft/OpenAdhan/issues/1)
            SystemEvents.PowerModeChanged += OnPowerChange;


            

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

        private static async void CheckForUpdate(string currentVersion)
        {
            if (string.IsNullOrWhiteSpace(currentVersion))
            {
                currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }

            if (currentVersion.EndsWith(".0"))
            {
                currentVersion = currentVersion.Substring(0, currentVersion.Length - 2);
            }

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "OpenAdhanForWindowsX");
            var response = await client.GetAsync("https://api.github.com/repos/HICalSoft/OpenAdhan/releases/latest");
            
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

               string jsonString = await response.Content.ReadAsStringAsync();

                string searchTagName = "\"tag_name\":";
                int tagNameIndex = jsonString.IndexOf(searchTagName);

                if (tagNameIndex != -1)
                {
                    int start = tagNameIndex + searchTagName.Length;
                    int end = jsonString.IndexOf(",", start);

                    string latestVersion = jsonString.ToLower().Substring(start, end - start).Trim('"').Trim('v');

                    if (latestVersion != currentVersion)
                    {
                        string message = "A new version of OpenAdhan is available.\n\n" +
                            "Current Version: " + currentVersion + "\n" +
                            "Latest Version: " + latestVersion + "\n\n" +
                            "Would you like to update now?";

                        if (MessageBox.Show(message, "OpenAdhan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start("https://github.com/HICalSoft/OpenAdhan/releases/download/V" + latestVersion + "/OpenAdhanInstaller.exe");
                        }
                    }
                }
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Error: ", error);
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

        // Make sure to have a clean close of the app even if killed
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlHandler handler, bool add);
        private delegate bool ConsoleCtrlHandler(CtrlType sig);
        static ConsoleCtrlHandler _handler;

        private enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType sig)
        {
            switch (sig)
            {
                case CtrlType.CTRL_CLOSE_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                    CleanupAndRestoreAudio();
                    return false;
                default:
                    return false;
            }
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            CleanupAndRestoreAudio();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            CleanupAndRestoreAudio();
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            CleanupAndRestoreAudio();
        }

        private static void CleanupAndRestoreAudio()
        {
            audioManager?.Dispose();

            if (audioManager != null)
            {
                if (audioManager.hasMuted)
                {
                    audioManager.RestoreApplicationVolumes();
                }
            }
        }
    }
}
