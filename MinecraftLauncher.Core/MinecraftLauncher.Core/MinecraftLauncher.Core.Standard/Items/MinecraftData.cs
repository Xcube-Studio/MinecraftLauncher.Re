using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftLauncher.Core.Standard.Enums;
using MinecraftLauncher.Core.Standard.Service.Local;

namespace MinecraftLauncher.Core.Standard.Items
{
    public class MinecraftData
    {
        public List<MinecraftLibrary> MinecraftLibraries { get; set; }

        public string MinecraftMainLibraryPath { get; set; }

        public AuthenticationData AuthenticationData { get; set; }

        public MinecraftEntity MinecraftEntity { get; set; }

        public MinecraftEntity MinecraftInheritEntity { get; set; }

        public MinecraftEntityType EntityType { get; set; }

        public LaunchConfiguration LaunchConfiguration { get; set; }
    }
}
