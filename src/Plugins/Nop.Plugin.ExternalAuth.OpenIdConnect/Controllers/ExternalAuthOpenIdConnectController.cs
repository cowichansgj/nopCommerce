using Nop.Core;
using Nop.Plugin.ExternalAuth.OpenIdconnect.Models;
using Nop.Services.Configuration;
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

        public ExternalAuthOpenIdConnectController(
            ISettingService settingService,
            IPermissionService permissionService,
            IStoreService storeService,
            IWorkContext workContext
            )
        {
            _settingService = settingService;
            _permissionService = permissionService;
            _storeService = storeService;
            _workContext = workContext;

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

    }
}
