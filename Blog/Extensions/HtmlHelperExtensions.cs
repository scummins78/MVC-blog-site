using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Blog.Extensions
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// add a requirejs module that contains app config settings.  Specifically we need
        /// the base url set here
        /// </summary>
        /// <param name="helper"></param>
        public static MvcHtmlString AddConfigModule(this HtmlHelper helper)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            
            var script = new StringBuilder();
            script.AppendLine(" <script type='text/javascript'>");
            script.AppendLine("     define('config', [],");
            script.AppendLine("         function () {");
            script.AppendLine("             return {");
            script.AppendFormat("               baseUrl: '{0}'", urlHelper.Content("~"));
            script.AppendLine("             };");
            script.AppendLine("         }");
            script.AppendLine("     );");
            script.AppendLine("</script>");

            return new MvcHtmlString(script.ToString());
        }
    }
}