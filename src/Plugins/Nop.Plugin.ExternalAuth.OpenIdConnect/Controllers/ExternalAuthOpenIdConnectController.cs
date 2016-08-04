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
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nop.Plugin.ExternalAuth.OpenIdConnect.Controllers
{
    public class ExternalAuthOpenIdConnectController : BasePluginController
    {
        private ISettingService _settingService;
        private IPermissionService _permissionService;
        private IStoreService _storeService;
        private IWorkContext _workContext;
        private ILocalizationService _localizationService;

        public ExternalAuthOpenIdConnectController(
            ISettingService settingService,
            IPermissionService permissionService,
            IStoreService storeService,
            IWorkContext workContext,
            ILocalizationService localizationService
            )
        {
            _settingService = settingService;
            _permissionService = permissionService;
            _storeService = storeService;
            _workContext = workContext;
            _localizationService = localizationService;

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

            var model = new ConfigurationModel();
            model.ClientKeyIdentifier = oidcExternalAuthSettings.ClientId;
            model.ClientSecret = oidcExternalAuthSettings.ClientSecret;

            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.ClientKeyIdentifier_OverrideForStore = _settingService.SettingExists(oidcExternalAuthSettings, x => x.ClientId, storeScope);
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
            oidcExternalAuthSettings.ClientId = model.ClientKeyIdentifier;
            oidcExternalAuthSettings.ClientSecret = model.ClientSecret;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            _settingService.SaveSettingOverridablePerStore(oidcExternalAuthSettings, x => x.ClientId, model.ClientKeyIdentifier_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(oidcExternalAuthSettings, x => x.ClientSecret, model.ClientSecret_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

    }
}
