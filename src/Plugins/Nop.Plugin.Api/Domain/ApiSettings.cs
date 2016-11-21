using Nop.Core.Configuration;

namespace Nop.Plugin.Api.Domain
{
    public class ApiSettings : ISettings
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public bool EnableApi { get; set; }
        public bool AllowRequestsFromSwagger { get; set; }
    }
}