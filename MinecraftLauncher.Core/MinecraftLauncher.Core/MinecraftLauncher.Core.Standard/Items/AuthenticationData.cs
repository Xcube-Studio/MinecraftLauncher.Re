using MinecraftLauncher.Core.Standard.Items;
using MinecraftLauncher.Core.Standard.Enums;

namespace MinecraftLauncher.Core.Standard.Items
{
    public class AuthenticationData
    {
        public string AccessToken { get; set; }

        public string ClientToken { get; set; }

        public Uuid Uuid { get; set; }

        public string UserName { get; set; }

        public UserType UserType { get; set; }
    }
}
