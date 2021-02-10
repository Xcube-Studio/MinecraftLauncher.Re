using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftLauncher.Core.Standard.Items;
using MinecraftLauncher.Core.Standard.Service.Local;

namespace MinecraftLauncher.Core.Standard
{
    public interface IArgsGenerator
    {
        MinecraftData MinecraftData { get; set; }

        string BulidArguments();

        string GetFrontArguments();

        string GetBehindArguments();

        string GetClasspath();
    }
}
