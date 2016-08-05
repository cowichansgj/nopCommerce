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
                RedirectUrl = "~/signin-oidc",
                LogoutRedirectUrl = "~/"
            };
            _settingService.SaveSetting(settings);

            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.Login", "Login using Another account");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.ClientId", "Client ID");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.ClientId.Hint", "Enter your client ID here. It should be given to you by your OIDC Provider.");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.ClientSecret", "Client Secret");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.ClientSecret.Hint", "Enter your client secret here. It should be shared between you and your OIDC Provider.");

            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.OpenIdConnectServer", "OpenID Connect Server");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.OpenIdConnectServer.Hint", "Enter your OIDC server address here");

            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.RedirectUrl", "Redirect URL");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.RedirectUrl.Hint", "Enter the redirect URL here. You should only need to change this if you've modified it.");

            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.LogoutRedirectUrl", "Logout Redirect URL");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.LogoutRedirectUrl.Hint", "Enter the logout redirect URL here. This is where a user will be redirected when logged out.");
            base.Install();
        }

        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<OpenIdConnectExternalAuthSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.Login");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.ClientId");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.ClientId.Hint");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.ClientSecret");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.ClientSecret.Hint");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.RedirectUrl");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.RedirectUrl.Hint");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.LogoutRedirectUrl");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.LogoutRedirectUrl.Hint");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.OpenIdConnectServer");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OpenIdConnect.OpenIdConnectServer.Hint");

            base.Uninstall();
        }
    }
}
