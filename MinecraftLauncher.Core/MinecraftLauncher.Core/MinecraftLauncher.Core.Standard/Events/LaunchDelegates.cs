using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MinecraftLauncher.Core.Standard.Events
{
    public class LaunchDelegates
    {
        public delegate void LaunchStartedHandler(LaunchEventArgs args);

        public delegate void LaunchStoppedHandler(LaunchEventArgs args);

        public delegate void LaunchCancelledHandler(LaunchEventArgs args);

        public delegate void LaunchFaildHandler(LaunchFaildEventArgs args);

        public delegate void ProcessStartedHandler(ProcessStartedEventArgs args);

        public delegate void ProcessExitedHandler(ProcessExitedEventArgs args);

        public delegate void ProcessCrashedHandler(ProcessCrashedEventArgs args);

        public delegate void ProcessOutputDateReceivedHandler(ProcessOutputDateReceivedEventArgs args);
    }
}
