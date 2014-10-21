using System.Web;
using System.Web.Optimization;

namespace Blog
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/libraries").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/knockout-3.2.0.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/widgets").Include(
                            "~/Scripts/widgets/RecentPostsVM.js",
                            "~/Scripts/widgets/ArchiveListVM.js",
                            "~/Scripts/widgets/TagListVM.js"));

            bundles.Add(new ScriptBundle("~/bundles/val").Include(
                        "~/Scripts/jquery.validate*"));

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

            #if DEBUG
                BundleTable.EnableOptimizations = false;
            #else
                BundleTable.EnableOptimizations = true;
            #endif
        }
    }
}
