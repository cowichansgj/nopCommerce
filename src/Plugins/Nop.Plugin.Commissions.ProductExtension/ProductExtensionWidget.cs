using Nop.Core.Plugins;
using Nop.Plugin.Commissions.ProductExtension.Data;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Nop.Plugin.Commissions.ProductExtension
{
    public class ProductExtensionWidget : BasePlugin, IWidgetPlugin
    {
        private readonly CommissionsProductExtensionContext _context;
        public ProductExtensionWidget(CommissionsProductExtensionContext context)
        {
            _context = context;
        }
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = null;
            controllerName = null;
            routeValues = null;
        }

        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out System.Web.Routing.RouteValueDictionary routeValues)
        {
            if (widgetZone == "commissions_product_attributes")
            {
                actionName = "Edit";
                controllerName = "CommissionsProductExtension";
                routeValues = new RouteValueDictionary
                {
                    {"Namespaces", "Nop.Plugin.Commissions.ProductExtension" },
                    {"area", null }
                };
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IList<string> GetWidgetZones()
        {
            return new List<string> { "commissions_product_attributes" };
        }

        public override void Install()
        {
            _context.Install();

            this.AddOrUpdatePluginLocaleResource("Plugins.Commissions.ProductExtension.PersonalVolume", "Personal Volume");
            this.AddOrUpdatePluginLocaleResource("Plugins.Commissions.ProductExtension.GroupVolume", "Group Volume");
            this.AddOrUpdatePluginLocaleResource("Plugins.Commissions.ProductExtension.AdditionalVolume", "Additional Volume");

            base.Install();
        }

        public override void Uninstall()
        {
            _context.Uninstall();
            
            this.DeletePluginLocaleResource("Plugins.Commissions.ProductExtension.PersonalVolume");
            this.DeletePluginLocaleResource("Plugins.Commissions.ProductExtension.GroupVolume");
            this.DeletePluginLocaleResource("Plugins.Commissions.ProductExtension.AdditionalVolume");

            base.Uninstall();
        }
    }
}
