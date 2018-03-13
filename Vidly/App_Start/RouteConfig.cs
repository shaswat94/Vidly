using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Vidly
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            //Custom Route
            //routes.MapRoute(
            //    "MovieByReleaseDate",
            //    "movies/released/{year}/{month}",
            //    new { controller="Movies", action= "ByReleaseDate" },
            //    //Custom Route constraint (only 2017 and 2018 considered for year and 2 digits for month)
            //    new { year= @"2017|2018", month= @"\d{2}" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
