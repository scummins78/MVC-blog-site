using System;
using System.Linq;
using System.Web.Mvc;
using DataRepository.Repository;
using NLog;

namespace Blog.Controllers
{
    public class BlogController : BaseController
    {
        private const int PostsPerPage = 10;

        private readonly DataHelper dataHelper;
        private readonly Logger logger = LogManager.GetLogger("BlogController");

        public BlogController(IBlogRepository repository)
        {
            dataHelper = new DataHelper(repository);
        }

        //
        // GET: /Blog/

        /// <summary>
        /// Retrieves a specific blog entry
        /// </summary>
        /// <param name="year">year portion of entry</param>
        /// <param name="month">month of entry</param>
        /// <param name="day">day of entry</param>
        /// <param name="title">entry title</param>
        /// <returns></returns>
        public ActionResult Entry(string year, string month, string day, string title)
        {
            try
            {
                // find the specific blog post requested
                var datePosted = DateTime.Parse(string.Format("{0}-{1}-{2}", year, month, day));
                var viewModel = dataHelper.FindPostAsync(datePosted, title).Result;

                return View("Entry", viewModel);
            }
            catch (Exception ex)
            {
                return HandleExceptions(
                    string.Format("Error occurred in Entry.  Parameters: {0}, {1}, {2}, {3}",
                    year, month, day, title), ex, logger);
            }
        }

        /// <summary>
        /// Gets a list of the latest blogs
        /// </summary>
        public ActionResult Index(string category, int page)
        {
            try
            {
                if (page < 1)
                    throw new ArgumentOutOfRangeException("page", page, "page cannot be below 1");
                
                // create filter if needed
                object viewModel = null;
                if (category == "default"){
                    viewModel = dataHelper.BuildPostListModelAsync(itemsPerPage: PostsPerPage, page: page,
                                                        orderBy: q => q.OrderByDescending(p => p.DateTimePosted)).Result;
                }
                else
                {
                    viewModel = dataHelper.BuildPostListModelAsync(itemsPerPage: PostsPerPage, page: page,
                                                        filter: p => p.Category == category, 
                                                        orderBy: q => q.OrderByDescending(p => p.DateTimePosted)).Result;
                }

                return View("List", viewModel);
            }
            catch (Exception ex)
            {
                return HandleExceptions(
                    string.Format("Error occurred in Entry.  Parameters: page- {0}", page), ex, logger);
            }
        }

        /// <summary>
        /// Retrieves blog list entered on a given day
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public ActionResult ByDay(string year, string month, string day, int page)
        {
            try
            {
                if (page < 1)
                    throw new ArgumentOutOfRangeException("page", page, "page cannot be below 1");
                
                var dy = Convert.ToInt32(day);
                var mn = Convert.ToInt32(month);
                var yr = Convert.ToInt32(year);

                // get posts on a given day
                var viewModel = dataHelper.BuildPostListModelAsync(itemsPerPage: PostsPerPage, page: page,
                                                        filter: p => p.DateTimePosted.Year == yr &&
                                                                p.DateTimePosted.Month == mn &&
                                                                p.DateTimePosted.Day == dy,
                                                         orderBy: q => q.OrderByDescending(p => p.DateTimePosted)).Result;
                return View("List", viewModel);
            }
            catch (Exception ex)
            {
                return HandleExceptions(
                    string.Format("Error occurred in ByDay.  Parameters- year: {0}, month: {1}, day: {2}, page: {3}",
                    year, month, day, page), ex, logger);
            }
        }

        /// <summary>
        /// Retrieves blog posts by month
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public ActionResult ByMonth(string year, string month, int page)
        {
            try
            {
                if (page < 1)
                    throw new ArgumentOutOfRangeException("page", page, "page cannot be below 1");

                var mn = Convert.ToInt32(month);
                var yr = Convert.ToInt32(year);

                // get posts done in a given month
                var viewModel = dataHelper.BuildPostListModelAsync(itemsPerPage: PostsPerPage, page: page,
                                                            filter: p => p.DateTimePosted.Year == yr &&
                                                                 p.DateTimePosted.Month == mn,
                                                            orderBy: q => q.OrderByDescending(p => p.DateTimePosted)).Result;
                return View("List", viewModel);
            }
            catch (Exception ex)
            {
                return HandleExceptions(
                    string.Format("Error occurred in ByMonth.  Parameters- year: {0}, month: {1}, page: {2}",
                    year, month, page), ex, logger);
            }
        }

        /// <summary>
        /// Retrieves blog posts by year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public ActionResult ByYear(string year, int page)
        {
            try
            {
                if (page < 1)
                    throw new ArgumentOutOfRangeException("page", page, "page cannot be below 1");
                
                // get posts in a given year
                // get posts done in a given month
                var yr = Convert.ToInt32(year);
                var viewModel = dataHelper.BuildPostListModelAsync(PostsPerPage, page,
                                                        p => p.DateTimePosted.Year == yr,
                                                        q => q.OrderByDescending(p => p.DateTimePosted)).Result;
                return View("List", viewModel);
            }
            catch (Exception ex)
            {
                return HandleExceptions(
                    string.Format("Error occurred in ByYear.  Parameters- year: {0} page: {1}",
                    year, page), ex, logger);
            }
        }

        /// <summary>
        /// Retrieves blog posts by tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public ActionResult ByTag(string tag, int page)
        {
            try
            {
                if (page < 1)
                        throw new ArgumentOutOfRangeException("page", page, "page cannot be below 1");
            
                // get posts based on tag
                var searchTag = tag.ToLower();
                var viewModel = dataHelper.BuildPostListModelAsync(PostsPerPage, page,
                                                        p => p.Tags.Any(t => t.TagValue.ToLower() == searchTag),
                                                        q => q.OrderByDescending(p => p.DateTimePosted)).Result;

                // add code here
                return View("List", viewModel);
            }
            catch (Exception ex)
            {
                return HandleExceptions(
                    string.Format("Error occurred in ByTag.  Parameters- tag: {0} page: {1}",
                    tag, page), ex, logger);
            }
        }

        protected override void Dispose(bool disposing)
        {
            // TODO: add dispose
        }
    }
}