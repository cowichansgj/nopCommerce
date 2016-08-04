using Nop.Core.Plugins;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using System.Web.Routing;

namespace Nop.Plugin.ExternalAuth.OpenIdConnect
{
    public class OpenIdConnectExternalAuthMethod : BasePlugin, IExternalAuthenticationMethod
    {
        private readonly ISettingService _settingService;
        public OpenIdConnectExternalAuthMethod(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "ExternalAuthOpenIdConnect";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.ExternalAuth.OpenIdConnect.Controllers" }, { "area", null } };
        }

        public void GetPublicInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PublicInfo";
            controllerName = "ExternalAuthOpenIdConnect";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.ExternalAuth.OpenIdConnect.Controllers" }, { "area", null } };
        }

        public override void Install()
        {
            //settings
            var settings = new OpenIdConnectExternalAuthSettings
            {
                ClientId = "",
                ClientSecret = "",
            };
            _settingService.SaveSetting(settings);

            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.Login", "Login using Another account");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.ClientKeyIdentifier", "App ID/API Key");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.ClientKeyIdentifier.Hint", "Enter your app ID/API key here. You can find it on your Another application page.");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.ClientSecret", "App Secret");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.ClientSecret.Hint", "Enter your app secret here. You can find it on your Another application page.");

            base.Install();
        }

        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<OpenIdConnectExternalAuthSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.Login");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.ClientKeyIdentifier");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.ClientKeyIdentifier.Hint");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.ClientSecret");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.ClientSecret.Hint");

            base.Uninstall();
        }
    }
}
