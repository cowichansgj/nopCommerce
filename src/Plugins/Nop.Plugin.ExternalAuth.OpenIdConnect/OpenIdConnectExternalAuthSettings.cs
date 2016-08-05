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
        public string OpenIdConnectServer { get; set; } = "http://localhost:5000/";
        public string ClientId { get; set; } = "a2ba9cc";
        public string ClientSecret { get; set; } = "4bb7d4e";
        public string RedirectUrl { get; set; } = "http://localhost:15536/signin-oidc";
        public string LogoutRedirectUrl { get; set; } = "http://localhost:15536/";
    }
}
