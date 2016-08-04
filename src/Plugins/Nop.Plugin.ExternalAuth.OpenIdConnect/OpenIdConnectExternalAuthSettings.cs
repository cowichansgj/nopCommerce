using Nop.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.ExternalAuth.OpenIdConnect
{
    public class OpenIdConnectExternalAuthSettings : ISettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
