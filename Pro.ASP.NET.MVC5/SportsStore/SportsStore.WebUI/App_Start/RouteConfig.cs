using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SportsStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: null,
                url: "",
                defaults: new { controller = "Product", action = "List", catagory = (string)null, page = 1 }
            );

            routes.MapRoute(
                name: null,
                url: "Page{page}",
                defaults: new { controller = "Product", action = "List", catagory = (string)null },
                constraints: new { page = @"\d+" }
            );

            routes.MapRoute(
                name: null,
                url: "{Category}",
                defaults: new { controller = "Product", action = "List", page = 1 }
            );

            routes.MapRoute(
                 name: null,
                 url: "{Category}/Page{page}",
                 defaults: new { controller = "Product", action = "List" },
                 constraints: new { page = @"\d+" }
             );

            routes.MapRoute(
                name: null,
                url: "{controller}/{action}"
            );
        }
    }
}
