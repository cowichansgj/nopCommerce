using Nop.Web.Framework;

namespace Nop.Plugin.Api.Models
{
    public class ConfigurationModel
    {
        [NopResourceDisplayName("Plugins.Api.Admin.Authority")]
        public string Authority { get; set; }
        public bool Authority_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Api.Admin.Client.ClientId")]
        public string ClientId { get; set; }
        public bool ClientId_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Api.Admin.Client.ClientSecret")]
        public string ClientSecret { get; set; }
        public bool ClientSecret_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Api.Admin.EnableApi")]
        public bool EnableApi { get; set; }
        public bool EnableApi_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Api.Admin.AllowRequestsFromSwagger")]
        public bool AllowRequestsFromSwagger { get; set; }
        public bool AllowRequestsFromSwagger_OverrideForStore { get; set; }

        public int ActiveStoreScopeConfiguration { get; set; }
    }
}