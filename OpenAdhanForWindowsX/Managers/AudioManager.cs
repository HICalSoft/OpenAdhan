using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAdhanForWindowsX.Managers
{
    public class AudioManager
    {
        private MMDeviceEnumerator deviceEnumerator;
        private Dictionary<string, float> originalVolumes;
        private string prayerAppProcessName;

        public bool hasMuted = false;

        public AudioManager(string appProcessName)
        {
            deviceEnumerator = new MMDeviceEnumerator();
            originalVolumes = new Dictionary<string, float>();
            prayerAppProcessName = appProcessName;
        }

        public void MuteOtherApplications()
        {
            originalVolumes = new Dictionary<string, float>();

            try
            {
                // Get all audio sessions
                using (var device = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia))
                {
                    var sessionManager = device.AudioSessionManager;
                    var sessionEnumerator = sessionManager.Sessions;

                    // Store original volumes and mute other applications
                    for (int i = 0; i < sessionEnumerator.Count; i++)
                    {
                        using (var session = sessionEnumerator[i])
                        {
                            var processName = session.GetSessionIdentifier.Split('\\').Last();

                            if (session.SimpleAudioVolume.Mute) continue;

                            // Store original volume
                            originalVolumes[processName] = session.SimpleAudioVolume.Volume;

                            // Mute if not our prayer app
                            if (!processName.ToUpperInvariant().Contains(prayerAppProcessName.ToUpperInvariant()))
                            {
                                session.SimpleAudioVolume.Mute = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error muting applications: {ex.Message}");
            }

            hasMuted = true;
        }

        public void RestoreApplicationVolumes()
        {
            if (!hasMuted) return;

            try
            {
                using (var device = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia))
                {
                    var sessionManager = device.AudioSessionManager;
                    var sessionEnumerator = sessionManager.Sessions;
                    // Restore original volumes
                    for (int i = 0; i < sessionEnumerator.Count; i++)
                    {
                        using (var session = sessionEnumerator[i])
                        {
                            var processName = session.GetSessionIdentifier.Split('\\').Last();

                            // Unmute and restore original volume if we have it stored
                            if (originalVolumes.ContainsKey(processName))
                            {
                                session.SimpleAudioVolume.Mute = false;
                                session.SimpleAudioVolume.Volume = originalVolumes[processName];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error restoring volumes: {ex.Message}");
            }

            hasMuted = false;
        }

        public void Dispose()
        {
            deviceEnumerator?.Dispose();
        }
    }
}
