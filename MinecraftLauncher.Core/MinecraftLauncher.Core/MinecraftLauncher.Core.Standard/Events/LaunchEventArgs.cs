using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncher.Core.Standard.Events
{
    public class LaunchEventArgs
    {
        public IMinecraftLauncher MinecraftLauncher { get; set; }

        public LaunchEventArgs() { }

        public LaunchEventArgs(IMinecraftLauncher minecraftLauncher) => this.MinecraftLauncher = minecraftLauncher;
    }
}
