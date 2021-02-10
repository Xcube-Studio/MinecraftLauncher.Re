using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncher.Core.Standard.Events
{
    public class LaunchFaildEventArgs : LaunchEventArgs
    {
        public Exception Exception { get; set; }

        public LaunchFaildEventArgs() { }

        public LaunchFaildEventArgs(IMinecraftLauncher Launcher,Exception exception)
        {
            this.MinecraftLauncher = Launcher;
            this.Exception = exception;
        }
    }
}
