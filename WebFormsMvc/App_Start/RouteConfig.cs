
using System.Web.Mvc;

using System.Web.Routing;

namespace WebFormsMvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "DateRange",
                url: "DateRange/Index/{id}/{range}/{rangeValue}",
                defaults: new { controller = "DateRange", action = "Index", id = "", range = "YearToDate", rangeValue = "" });
        }
    }
}
