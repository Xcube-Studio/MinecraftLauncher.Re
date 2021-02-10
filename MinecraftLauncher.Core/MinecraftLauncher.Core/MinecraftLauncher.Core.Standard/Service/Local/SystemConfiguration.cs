using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncher.Core.Standard.Service.Local
{
    public class SystemConfiguration
    {
        public static string Arch { get 
            {
                if (Environment.Is64BitOperatingSystem)
                    return "64";
                else return "32";
            } 
        }
    }
}
