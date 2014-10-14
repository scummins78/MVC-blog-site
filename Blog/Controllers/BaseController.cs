﻿using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Authentication.Managers;

using Blog.Models;
using Authentication.Models;
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

        private ApplicationUser appUser = null;
        private ApplicationUser AppUser
        {
            get
            {
                if (appUser == null)
                {
                    var id = User.Identity.GetUserId();
                    if (id == null)
                    {
                        return null;
                    }

                    appUser = userManager.FindById(id);
                }

                return appUser;
            }
        }


        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            // retireve data
            var data = new BaseVM()
            {
                DisplayName = GetUserDisplayName(),
                CanCreatePost = CanCreateNewPost(),
                GitUrl = ConfigurationManager.AppSettings["GitUrl"],
                TwitterUrl = ConfigurationManager.AppSettings["TwitterAccount"],
                LinkedInUrl = ConfigurationManager.AppSettings["LinkedInUrl"],
                FacebookUrl = ConfigurationManager.AppSettings["FacebookUrl"],
                TwitterFeed = GetWidgetSettings()
            };

            // set to viewbag
            ViewBag.UserData = data;
        }

        #region authorization helper

        internal bool CanCreateNewPost()
        {
            if (AppUser == null)
            {
                return false;
            }

            return AppUser.CanPostBlogEntry;
        } 

        internal string GetUserDisplayName()
        {
            if (AppUser == null)
            {
                return string.Empty;
            }

            return AppUser.DisplayName;
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