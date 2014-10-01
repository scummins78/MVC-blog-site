using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Authentication.Managers;

using Blog.Models;
using DataRepository.Models;


namespace Blog.Controllers
{

    /// <summary>
    /// Handle any methods needed by all controllers and any view model data that will be
    /// needed in the main _Layout view
    /// </summary>
    public class BaseController : Controller
    {
        private ApplicationUserManager userManager;

        public BaseController()
        {
            
        }
        
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            // retireve data
            var data = new BaseViewModel()
            {
                CanCreatePost = CanCreateNewPost(),
                TwitterFeed = GetWidgetSettings()
            };

            // set to viewbag
            ViewBag.UserData = data;
        }

        #region authorization helper

        internal bool CanCreateNewPost()
        {
            var id = User.Identity.GetUserId();
            if (id == null)
            {
                return false;
            }

            var user = userManager.FindById(id);
            if (user == null)
            {
                return false;
            }

            return user.CanPostBlogEntry;
        } 

        private TwitterWidget GetWidgetSettings()
        {
            var newWidget = new TwitterWidget
            {
                AccountUrl = ConfigurationManager.AppSettings["TwitterAccount"],
                Label = ConfigurationManager.AppSettings["TwitterLabel"],
                WidgetId = ConfigurationManager.AppSettings["TwitterWidgetId"]
            };
            return newWidget;
        }

        #endregion
    }
}