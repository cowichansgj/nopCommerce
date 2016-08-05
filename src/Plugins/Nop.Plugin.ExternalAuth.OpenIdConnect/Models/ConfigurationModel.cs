using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.ExternalAuth.OpenIdconnect.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.ExternalAuth.OpenIdConnect.OpenIdConnectServer")]
        public string OpenIdConnectServer { get; set; }
        public bool OpenIdConnectServer_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.ExternalAuth.OpenIdConnect.ClientId")]
        public string ClientId { get; set; }
        public bool ClientId_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.ExternalAuth.OpenIdConnect.ClientSecret")]
        public string ClientSecret { get; set; }
        public bool ClientSecret_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.ExternalAuth.OpenIdConnect.RedirectUrl")]
        public string RedirectUrl { get; set; }
        public bool RedirectUrl_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.ExternalAuth.OpenIdConnect.LogoutRedirectUrl")]
        public string LogoutRedirectUrl { get; set; }
        public bool LogoutRedirectUrl_OverrideForStore { get; set; }
    }
}