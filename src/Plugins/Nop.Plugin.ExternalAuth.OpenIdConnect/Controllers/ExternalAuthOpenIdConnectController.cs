using Nop.Core;
using Nop.Plugin.ExternalAuth.OpenIdconnect.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nop.Plugin.ExternalAuth.OpenIdConnect.Core;
using Nop.Services.Authentication.External;
using Nop.Core.Domain.Customers;
using Nop.Web.Framework;

namespace Nop.Plugin.ExternalAuth.OpenIdConnect.Controllers
{
    public class ExternalAuthOpenIdConnectController : BasePluginController
    {
        private ISettingService _settingService;
        private readonly IOAuth2ProviderOpenIdConnectAuthorizer _oAuthProviderOpenIdConnectAuthorizer;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;
        private IPermissionService _permissionService;
        private IStoreService _storeService;
        private IWorkContext _workContext;
        private ILocalizationService _localizationService;

        public ExternalAuthOpenIdConnectController(
            ISettingService settingService,
            IOAuth2ProviderOpenIdConnectAuthorizer oAuthProviderOpenIdConnectAuthorizer,
            IOpenAuthenticationService openAuthenticationService,
            ExternalAuthenticationSettings externalAuthenticationSettings,
        IPermissionService permissionService,
            IStoreService storeService,
            IWorkContext workContext,
            ILocalizationService localizationService
            )
        {
            _settingService = settingService;
            _oAuthProviderOpenIdConnectAuthorizer = oAuthProviderOpenIdConnectAuthorizer;
            _openAuthenticationService = openAuthenticationService;
            _externalAuthenticationSettings = externalAuthenticationSettings;
            _permissionService = permissionService;
            _storeService = storeService;
            _workContext = workContext;
            _localizationService = localizationService;

        }

        [ChildActionOnly]
        public ActionResult PublicInfo()
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var oidcExternalAuthSettings = _settingService.LoadSetting<OpenIdConnectExternalAuthSettings>(storeScope);

            var model = new ConfigurationModel();
            model.ClientId = oidcExternalAuthSettings.ClientId;
            model.ClientSecret = oidcExternalAuthSettings.ClientSecret;
            model.RedirectUrl = oidcExternalAuthSettings.RedirectUrl;

            return View("~/Plugins/ExternalAuth.OpenIdConnect/Views/ExternalAuthOpenIdConnect/PublicInfo.cshtml", oidcExternalAuthSettings);
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return Content("Access denied");

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var oidcExternalAuthSettings = _settingService.LoadSetting<OpenIdConnectExternalAuthSettings>(storeScope);

            var model = new ConfigurationModel
            {
                OpenIdConnectServer = oidcExternalAuthSettings.OpenIdConnectServer,
                RedirectUrl = oidcExternalAuthSettings.RedirectUrl,
                LogoutRedirectUrl = oidcExternalAuthSettings.LogoutRedirectUrl,
                ClientId = oidcExternalAuthSettings.ClientId,
                ClientSecret = oidcExternalAuthSettings.ClientSecret
            };

            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.OpenIdConnectServer_OverrideForStore = _settingService.SettingExists(oidcExternalAuthSettings, x => x.OpenIdConnectServer, storeScope);
                model.RedirectUrl_OverrideForStore = _settingService.SettingExists(oidcExternalAuthSettings, x => x.RedirectUrl, storeScope);
                model.LogoutRedirectUrl_OverrideForStore = _settingService.SettingExists(oidcExternalAuthSettings, x => x.LogoutRedirectUrl, storeScope);
                model.ClientId_OverrideForStore = _settingService.SettingExists(oidcExternalAuthSettings, x => x.ClientId, storeScope);
                model.ClientSecret_OverrideForStore = _settingService.SettingExists(oidcExternalAuthSettings, x => x.ClientSecret, storeScope);
            }

            return View("~/Plugins/ExternalAuth.OpenIdConnect/Views/ExternalAuthOpenIdConnect/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return Content("Access denied");

            if (!ModelState.IsValid)
                return Configure();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var oidcExternalAuthSettings = _settingService.LoadSetting<OpenIdConnectExternalAuthSettings>(storeScope);

            //save settings
            oidcExternalAuthSettings.OpenIdConnectServer = model.OpenIdConnectServer;
            oidcExternalAuthSettings.RedirectUrl = model.RedirectUrl;
            oidcExternalAuthSettings.LogoutRedirectUrl = model.LogoutRedirectUrl;
            oidcExternalAuthSettings.ClientId = model.ClientId;
            oidcExternalAuthSettings.ClientSecret = model.ClientSecret;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            _settingService.SaveSettingOverridablePerStore(oidcExternalAuthSettings, x => x.OpenIdConnectServer, model.OpenIdConnectServer_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(oidcExternalAuthSettings, x => x.RedirectUrl, model.RedirectUrl_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(oidcExternalAuthSettings, x => x.LogoutRedirectUrl, model.LogoutRedirectUrl_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(oidcExternalAuthSettings, x => x.ClientId, model.ClientId_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(oidcExternalAuthSettings, x => x.ClientSecret, model.ClientSecret_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }
        public ActionResult SignIn(string code, string returnUrl = "~/")
        {
            var result = _oAuthProviderOpenIdConnectAuthorizer.Authorize(returnUrl, false);


            switch(result.AuthenticationStatus)
            {
                case OpenAuthenticationStatus.AutoRegisteredStandard:
                    {
                        return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Standard });
                    }
                default:break;
            }
            return HttpContext.Request.IsAuthenticated ? new RedirectResult(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/") : new RedirectResult(Url.LogOn(returnUrl));

            //var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            //var oidcExternalAuthSettings = _settingService.LoadSetting<OpenIdConnectExternalAuthSettings>(storeScope);

            //var tokenRequestParameters = new Dictionary<string, string>()
            //{
            //    { "client_id", oidcExternalAuthSettings.ClientId },
            //    { "redirect_uri", oidcExternalAuthSettings.RedirectUrl }, //"http://localhost:15536/Plugins/ExternalAuthOpenIdConnect/LoginCallback" },
            //    { "code", code },
            //    { "client_secret", oidcExternalAuthSettings.ClientSecret },
            //    { "grant_type", "authorization_code" },
            //};

            //var requestContent = new FormUrlEncodedContent(tokenRequestParameters);

            //var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{oidcExternalAuthSettings.OpenIdConnectServer}connect/token");
            //requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //requestMessage.Content = requestContent;

            //using (var client = new HttpClient())
            //{
            //    var response = client.SendAsync(requestMessage).Result;

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var oauth = OAuthTokenResponse.Success(JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result));
            //    }

            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}
        }

        public ActionResult LoginCallBack(string returnUrl = null, string code = null)
        {
            return Json(new object());
        }
            
    }
}
