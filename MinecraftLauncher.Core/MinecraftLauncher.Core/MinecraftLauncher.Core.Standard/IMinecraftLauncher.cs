using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftLauncher.Core.Standard.Enums;
using MinecraftLauncher.Core.Standard.Items;
using MinecraftLauncher.Core.Standard.Events;
using System.Diagnostics;

namespace MinecraftLauncher.Core.Standard
{
    public interface IMinecraftLauncher : IDisposable
    {
        event LaunchDelegates.LaunchCancelledHandler LaunchCancelled;

        event LaunchDelegates.LaunchFaildHandler LaunchFaild;

        event LaunchDelegates.LaunchStartedHandler LaunchStarted;

        event LaunchDelegates.LaunchStoppedHandler LaunchStopped;

        event LaunchDelegates.ProcessCrashedHandler ProcessCrashed;

        event LaunchDelegates.ProcessExitedHandler ProcessExited;

        event LaunchDelegates.ProcessOutputDateReceivedHandler ProcessOutputDateReceived;

        event LaunchDelegates.ProcessStartedHandler ProcessStarted;

        LaunchState State { get;}

        void Start();

        void Stop();

        void Cancel();

        void DetachAllHandlers();
    }
}
