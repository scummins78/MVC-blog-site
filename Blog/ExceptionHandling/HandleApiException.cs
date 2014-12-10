using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using NLog;

namespace Blog.ExceptionHandling
{
    public class HandleApiException : ExceptionFilterAttribute
    {
        private readonly Logger logger = LogManager.GetLogger("BlogApiErrors");

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var errorMessage = string.Format("Error occured in api call.  Controller: {0}, Action: {1}",
                                actionExecutedContext.ActionContext.ControllerContext.RouteData.Values["controller"],
                                actionExecutedContext.ActionContext.ControllerContext.RouteData.Values["action"]);
            
            logger.Error(errorMessage, actionExecutedContext.Exception);

            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError){
                ReasonPhrase = "Critical exception occurred",
                Content = new StringContent("Error occurred in api call.  Check error logs.")
            });
        }
    }
}