using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftLauncher.Core.Standard.Enums
{
    public enum LaunchState
    {
        Undefined = 0,
        Initialized = 1,
        Running = 2,
        Stopped = 3,
        Cancelled = 4
    }
}
