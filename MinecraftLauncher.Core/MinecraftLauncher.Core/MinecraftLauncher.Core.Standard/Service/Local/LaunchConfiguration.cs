using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncher.Core.Standard.Service.Local
{
    public class LaunchConfiguration
    {
        public string Version { get; set; }

        public string FrontArgs { get; set; }

        public string GameFolder { get; set; }

        public string JavaPath { get; set; }

        public string NativesFolder { get; set; }

        public int MaxMemory { get; set; }

        public int? MinMemory { get; set; }
    }
}
