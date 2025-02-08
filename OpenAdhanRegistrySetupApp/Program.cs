using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
