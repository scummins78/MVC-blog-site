using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Blog.Models.Home;

namespace Blog.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult About()
        {
            // get about info
            var viewModel = new AboutVM()
            {
                AboutText = ConfigurationManager.AppSettings["AboutText"],
                AvatarEmail = ConfigurationManager.AppSettings["AboutEmail"],
                ContactEmail= ConfigurationManager.AppSettings["AboutEmail"],
                LinkedInLink = ConfigurationManager.AppSettings["LinkedInUrl"],
                GitHubLink = ConfigurationManager.AppSettings["GitUrl"]
            };

            return View(viewModel);
        }
    }
}