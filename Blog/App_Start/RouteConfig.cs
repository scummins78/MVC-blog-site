using System.Web.Mvc;
using System.Web.Routing;

namespace Blog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("BlogEntry",
                            "posts/{year}/{month}/{day}/{title}",
                            new { controller = "Blog", action = "Entry" }
                //new {year = @"(19|20)\d\d", month = @"\d\d", day = @"\d\d" }
                );

            routes.MapRoute("BlogsByDayFirstPage",
                            "posts/{year}/{month}/{day}",
                            new { controller = "Blog", action = "ByDay", page = 1 },
                            new { year = @"(19|20)\d\d", month = @"\d\d", day = @"\d\d" }
                );

            routes.MapRoute("BlogsByDay",
                            "posts/{year}/{month}/{day}/page/{page}",
                            new { controller = "Blog", action = "ByDay" },
                            new { year = @"(19|20)\d\d", month = @"\d\d", day = @"\d\d" }
                );

            routes.MapRoute("BlogsByMonthFirstPage",
                            "posts/{year}/{month}",
                            new { controller = "Blog", action = "ByMonth", page = 1 },
                            new { year = @"(19|20)\d\d", month = @"\d\d" }
                );

            routes.MapRoute("BlogsByMonth",
                            "posts/{year}/{month}/page/{page}",
                            new { controller = "Blog", action = "ByMonth" },
                            new { year = @"(19|20)\d\d", month = @"\d\d" }
                );

            routes.MapRoute("BlogsByYearFirstPage",
                            "posts/{year}",
                            new { controller = "Blog", action = "ByYear", page = 1 },
                            new { year = @"(19|20)\d\d" }
                );

            routes.MapRoute("BlogsByYear",
                            "posts/{year}/page/{page}",
                            new { controller = "Blog", action = "ByYear" },
                            new { year = @"(19|20)\d\d" }
                );

            routes.MapRoute("TagFirstPage",
                            "tag/{tag}",
                            new { controller = "Blog", action = "ByTag", page = 1 }
                );

            routes.MapRoute("Tag",
                            "tag/{tag}/page/{page}",
                            new { controller = "Blog", action = "ByTag" }
                );

            routes.MapRoute("SearchFirstPage",
                            "search/{term}",
                            new { controller = "Blog", action = "Search", page = 1 }
                );

            routes.MapRoute("Search",
                            "search/{term}/page/{page}",
                            new { controller = "Blog", action = "Search" }
                );


            routes.MapRoute("About",
                            "about",
                            new { controller = "Home", action = "About" }
                );

            routes.MapRoute(
                name: "Account",
                url: "Account/{action}",
                defaults: new { controller = "Account", action = "Login"}
            );

            routes.MapRoute(
                name: "DashboardPosts",
                url: "Dashboard/BlogList/{page}",
                defaults: new { controller = "Dashboard", action = "BlogList", page = UrlParameter.Optional}
            );

            routes.MapRoute(
                name: "Dashboard",
                url: "Dashboard/{action}/{id}",
                defaults: new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional}
            );

            routes.MapRoute("CategoryFirstPage",
                            "{category}",
                            new { controller = "Blog", action = "Index", page = 1 }
                );

            routes.MapRoute("Category",
                            "{category}/page/{page}",
                            new { controller = "Blog", action = "Index" }
                );

            routes.MapRoute("DefaultFirstPage",
                            "",
                            new { controller = "Blog", action = "Index", category = "default", page = 1 }
                );

            routes.MapRoute("Default",
                            "page/{page}",
                            new { controller = "Blog", action = "Index", category = "default" }
                );



            // fall through for if any other paths don't get picked up
            routes.MapRoute(
                name: "Secondary",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}