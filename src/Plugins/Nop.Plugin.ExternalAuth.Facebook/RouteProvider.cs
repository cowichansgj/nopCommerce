using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.ExternalAuth.Facebook
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.ExternalAuth.OpenIdConnect.Login",
                 "Plugins/ExternalAuthOpenIdConnect/Login",
                 new { controller = "ExternalAuthOpenIdConnect", action = "Login" },
                 new[] { "Nop.Plugin.ExternalAuth.OpenIdConnect.Controllers" }
            );

            routes.MapRoute("Plugin.ExternalAuth.OpenIdConnect.LoginCallback",
                 "Plugins/ExternalAuthOpenIdConnect/LoginCallback",
                 new { controller = "ExternalAuthOpenIdConnect", action = "LoginCallback" },
                 new[] { "Nop.Plugin.ExternalAuth.OpenIdConnect.Controllers" }
            );
        }
        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
