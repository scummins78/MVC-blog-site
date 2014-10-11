using System.Web;
using System.Web.Optimization;

namespace Blog
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/knockout-3.2.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // add app style and javascript
            bundles.Add(new ScriptBundle("~/js").Include(
                "~/Scripts/navigation.js",
                "~/Scripts/socialmediaapi.js"));

            // scripts needed for blog editing
            bundles.Add(new ScriptBundle("~/bundles/editor").Include(
                        "~/Scripts/tinymce/tinymce.js",
                        "~/Scripts/texteditor.js"));
            BundleTable.EnableOptimizations = false;
        }
    }
}
