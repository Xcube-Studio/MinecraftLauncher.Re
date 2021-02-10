using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftLauncher.Core.Standard.Items;
using MinecraftLauncher.Core.Standard.Enums;

namespace MinecraftLauncher.Core.Standard.Items
{
    public class ProfileData
    {
        public AuthenticateType AuthenticateType { get; set; }

        public Uuid Uuid { get; set; }

        public string AccessToken { get; set; }

        public SkinData SkinData { get; set; }
    }
}
