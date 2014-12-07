using System;
using System.Text;
using System.Linq;
using System.Web.Mvc;
using NLog;

namespace Blog.ExceptionHandling
{
    public class HandleException : HandleErrorAttribute
    {
        private readonly Logger logger = LogManager.GetLogger("BlogErrors");

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            var message = new StringBuilder();
            foreach(var item in filterContext.RouteData.Values){
                message.AppendFormat("key: {0}, value: {1}.  ", item.Key, item.Value);
            }

            logger.Error(string.Format("Error occurred in request.  Parameters-  {0}", message.ToString()), filterContext.Exception);

            var model = new HandleErrorInfo(filterContext.Exception, 
                                            filterContext.RouteData.Values["controller"].ToString(), 
                                            filterContext.RouteData.Values["action"].ToString());

            var viewResult = new ViewResult {
                ViewName = "Error",
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model)
            };
            
            // retain viewdata values
            foreach ( var value in filterContext.Controller.ViewData )
            {
                if ( !viewResult.ViewData.ContainsKey(value.Key) )
                {
                    viewResult.ViewData[value.Key] = value.Value;
                }
            }
            filterContext.Result = viewResult;
            filterContext.ExceptionHandled = true;
        }
    }
}