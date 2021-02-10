using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftLauncher.Core.Standard.Items;

namespace MinecraftLauncher.Core.Standard
{
    public interface IAuthenticator : IDisposable
    {
        ProfileData Authenticate();

        void Refresh();
    }
}
