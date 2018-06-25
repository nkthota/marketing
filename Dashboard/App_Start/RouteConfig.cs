using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Dashboard
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Schedules",
                url: "RoadMap/Index/{id}/{type}",
                defaults: new { controller = "RoadMap", action = "Index", id = UrlParameter.Optional, type = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "RoadMap", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
