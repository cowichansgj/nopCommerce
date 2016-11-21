using System.Web.Mvc;
using Nop.Admin.Controllers;
using Nop.Core;
using Nop.Plugin.Api.Constants;
using Nop.Plugin.Api.Domain;
using Nop.Plugin.Api.MappingExtensions;
using Nop.Plugin.Api.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using System;

namespace Nop.Plugin.Api.Controllers.Admin
{
    [AdminAuthorize]
    public class ApiAdminController : BaseAdminController
    {
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly ISettingService _settingService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;

        public ApiAdminController(
            IStoreService storeService,
            IWorkContext workContext,
            ISettingService settingService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService)
        {
            _storeService = storeService;
            _workContext = workContext;
            _settingService = settingService;
            _customerActivityService = customerActivityService;
            _localizationService = localizationService;
        }
        
        [HttpGet]
        public ActionResult Settings()
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);

            ApiSettings apiSettings = _settingService.LoadSetting<ApiSettings>(storeScope);

            ConfigurationModel model = apiSettings.ToModel();

            // Store Settings
            model.ActiveStoreScopeConfiguration = storeScope;

            if (model.Authority_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(apiSettings, x => x.Authority, storeScope, false);

            if (model.ClientId_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(apiSettings, x => x.ClientId, storeScope, false);

            if (model.ClientSecret_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(apiSettings, x => x.ClientSecret, storeScope, false);

            if (model.EnableApi_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(apiSettings, x => x.EnableApi, storeScope, false);
            if (model.AllowRequestsFromSwagger_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(apiSettings, x => x.AllowRequestsFromSwagger, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            return View(ViewNames.AdminApiSettings, model);
        }

        [HttpPost]
        public ActionResult Settings(ConfigurationModel configurationModel)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);

            ApiSettings settings = configurationModel.ToEntity();

            /* We do not clear cache after each setting update.
            * This behavior can increase performance because cached settings will not be cleared 
            * and loaded from database after each update */

            if (configurationModel.Authority_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(settings, x => x.Authority, storeScope, false);

            if (configurationModel.ClientId_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(settings, x => x.ClientId, storeScope, false);

            if (configurationModel.ClientSecret_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(settings, x => x.ClientSecret, storeScope, false);

            if (configurationModel.EnableApi_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(settings, x => x.EnableApi, storeScope, false);
            if (configurationModel.AllowRequestsFromSwagger_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(settings, x => x.AllowRequestsFromSwagger, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            _customerActivityService.InsertActivity("EditApiSettings", "Edit Api Settings");

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return View(ViewNames.AdminApiSettings, configurationModel);
        }
    }
}