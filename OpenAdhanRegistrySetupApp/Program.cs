namespace OpenAdhanRegistrySetupApp
{
    class Program
    {
        static void Main(string[] args)
        {
            OpenAdhanForWindowsX.RegistrySettingsHandler rsh = new OpenAdhanForWindowsX.RegistrySettingsHandler(true);
            rsh.InstallOpenAdhanRegistryKeys();
        }
    }
}
