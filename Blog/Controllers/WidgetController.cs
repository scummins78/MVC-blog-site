using System;
using System.Linq;
using System.Web.Mvc;
using Blog.Models;
using DataRepository.Repository;
using NLog;

namespace Blog.Controllers
{
    public class WidgetController : BaseController
    {
        private readonly DataHelper dataHelper;
        private readonly Logger logger = LogManager.GetLogger("BlogController");

        public WidgetController(IBlogRepository repository)
        {
            dataHelper = new DataHelper(repository);
        }


        /// <summary>
        /// GET Endpoint for retrieving a list of tags and the number of times they occur.  Ordered by instance count.
        /// </summary>
        /// <returns>json tag list with tag urls</returns>
        public JsonResult TagList()
        {
            try
            {
                // get tag list
                var tags = dataHelper.GetTagsAsync().Result;
                return Json(new JsonReturnObject(tags, Response.StatusCode, true), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return HandleExceptions("Error occurred retrieving tag list.", ex);
            }
        }

        /// <summary>
        /// GET endpoint for retrieving an archive list of posts.  This includes months in which posts were made
        /// and how many posts that month
        /// </summary>
        /// <returns>json archive list with date urls</returns>
        public JsonResult ArchiveList()
        {
            try
            {
                var archives = dataHelper.GetArchiveLinksAsync().Result;
                return Json(new JsonReturnObject(archives, Response.StatusCode, true), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return HandleExceptions("Error occurred retrieving archive list.", ex);
            }
        }

        /// <summary>
        /// GET endpoint for retrieving the 5 most recent posts.  Includes post title, thumbnail and url.
        /// </summary>
        /// <returns>json recent list with post urls</returns>
        public JsonResult RecentList()
        {
            try
            {
                var posts = dataHelper.GetRecentPostLinksAsync().Result;
                return Json(new JsonReturnObject(posts, Response.StatusCode, true), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return HandleExceptions("Error occurred retrieving recent list.", ex);
            }
        }

        private JsonResult HandleExceptions(string message, Exception ex)
        {
            // log error
            logger.Error(message, ex);

            Response.StatusCode = 500;
            return Json(new JsonReturnObject(null, Response.StatusCode, false), JsonRequestBehavior.AllowGet);
        }
    }
}