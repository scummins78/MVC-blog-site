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


        // GET: TagList
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

        private JsonResult HandleExceptions(string message, Exception ex)
        {
            // log error
            logger.Error(message, ex);

            Response.StatusCode = 500;
            return Json(new JsonReturnObject(null, Response.StatusCode, false), JsonRequestBehavior.AllowGet);
        }
    }
}