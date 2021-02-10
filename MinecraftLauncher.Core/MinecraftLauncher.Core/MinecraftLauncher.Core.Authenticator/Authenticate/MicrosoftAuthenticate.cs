using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftLauncher.Core.Standard;
using MinecraftLauncher.Core.Standard.Enums;
using MinecraftLauncher.Core.Standard.Items;

namespace MinecraftLauncher.Core.Authenticator.Authenticate
{
    public abstract class MicrosoftAuthenticate : IAuthenticator
    {
        public AuthenticationData Authenticate()
        {
            
        }

        public void Dispose()
        {
            
        }

        private XErrType GetXErrType(long value)
        {
            switch (value)
            {
                case 2148916233:
                    return XErrType.NoXboxAccount;
                case 2148916238:
                    return XErrType.XboxAccountUnder18;
                default:
                    throw new Exception();
            }
        }
    }
}
