using Nop.Web.Framework.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nop.Plugin.Commissions.ProductExtension
{
    public class ProductExtensionRouteProvider : IRouteProvider
    {
        public int Priority
        {
            get
            {
                return 0;
            }
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.Commissions.ProductExtension.UpdateExtensionProperties",
                "Plugins/CommissionProductExtension/Update",
                new { controller = "CommissionsProductExtension", action = "Update" },
                new[] { "Nop.Plugin.Commissions.ProductExtension.Controllers" }
            );
        }
    }
}
