using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MinecraftLauncher.Core.Standard.Events
{
    public class ProcessExitedEventArgs
    {
        public int ExitCode { get; set; }

        public ProcessExitedEventArgs() { }

        public ProcessExitedEventArgs(int exitcode)
        {
            this.ExitCode = exitcode;
        }
    }
}
