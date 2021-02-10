using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncher.Core.Standard.Events
{
    public class ProcessCrashedEventArgs
    {
        public string Reason { get; set; }

        public ProcessCrashedEventArgs() { }

        public ProcessCrashedEventArgs(string reason)
        {
            this.Reason = reason;
        }
    }
}
