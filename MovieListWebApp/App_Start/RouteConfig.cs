using System.Web.Mvc;
using System.Web.Routing;

namespace MovieListWebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "MovieDetails",
                url: "Movies/Details/{id}",
                defaults: new { controller = "Movies", action = "Details", id = 0 }
            );


            routes.MapRoute(
                name: "UserDetails",
                url: "Users/Details/{id}",
                defaults: new { controller = "Users", action = "Details", id = 0 }
            );

            routes.MapRoute(
                name: "UserCreate",
                url: "Users/Create",
                defaults: new { controller = "Users", action = "Create" }
            );

            routes.MapRoute(
                name: "MovieCreate",
                url: "Movies/Create",
                defaults: new { controller = "Movies", action = "Create" }
            );

            routes.MapRoute(
                name: "UserDetailsRedirect",
                url: "Users/{create}",
                defaults: new { controller = "Users", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
