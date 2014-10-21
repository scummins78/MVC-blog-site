using System;
using System.Collections.ObjectModel;
using System.Web.Security.AntiXss;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using DataRepository.Repository;
using DataRepository.Models;
using Blog.Models.BlogView;
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
                    year, month, day, title), ex);
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
                // add logging
                return HandleExceptions(
                    string.Format("Error occurred in Entry.  Parameters: page- {0}", page), ex);
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
                    year, month, day, page), ex);
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
                    year, month, page), ex);
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
                    year, page), ex);
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
                    tag, page), ex);
            }
        }

        #region editing and creating posts

        /// <summary>
        /// displays blog entry screen
        /// </summary>
        /// <returns></returns>
        public ActionResult NewPost()
        {
            try
            { 
                // check to see if user can create posts
                if (!CanCreateNewPost())
                {
                    logger.Error("User does not have the 'CanCreatePost' user right.");
                    Response.StatusCode = 404;
                    return View("Error");
                }

                var model = BlogEntryVM.BuildViewModel(dataHelper.GetNewPost());
                model.PageTitle = "Blog Post";

                return View("NewEntry", model);
            }
            catch (Exception ex)
            {
                return HandleExceptions("Error occurred in NewPost Get.", ex);
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NewPost(BlogEntryVM model)
        {
            try
            {
                // check to see if user can create posts
                if (!CanCreateNewPost())
                {
                    logger.Error("User does not have the 'CanCreatePost' user right.");
                    Response.StatusCode = 404;
                    return View("Error");
                }
                
                model.DateTimePosted = DateTime.Now;
                model.Author = string.Format("{0} {1}", AppUser.FirstName, AppUser.LastName);
                model.AuthorId = AppUser.Id;
                
                // build url title
                model.UrlTitle = model.Title.Replace(" ", "-").ToLower();

                // take the entered tags and build blog tag objects
                model.Tags = BuildTags(model.BlogTags);

                // persist blog
                var rowsCreated = dataHelper.InsertOrUpdate(FlattenBlogPostModel(model));

                if (rowsCreated == 0)
                {
                    // in here handle issues;  maybe throw exception?
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return HandleExceptions("Error occurred in NewPost POST.", ex);
            }
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Takes blogpost view model and creates data post
        /// </summary>
        /// <param name="newPost"></param>
        /// <returns></returns>
        private BlogPost FlattenBlogPostModel(BlogPostVM newPost)
        {
            return newPost.BuildModel();
        }

        /// <summary>
        /// Takes a comma delimited string and builds tag objects from it
        /// </summary>
        /// <param name="tagString">string of tags</param>
        /// <returns></returns>
        private static ObservableCollection<BlogTagVM> BuildTags(string tagString) {

            var tags = new ObservableCollection<BlogTagVM>();
            if (tagString == null || tagString.Length == 0) return tags;
            
            tagString = AntiXssEncoder.HtmlEncode(tagString, true);
            var tagArray = tagString.Split(',');
            
            
            foreach (var tag in tagArray){
                tags.Add(new BlogTagVM()
                {
                    TagValue = tag
                });
            }

            return tags;
        }

        #endregion

        private ActionResult HandleExceptions(string message, Exception ex)
        {
            // log error
            logger.Error(message, ex);

            Response.StatusCode = 500;
            return View("Error");
        }

        protected override void Dispose(bool disposing)
        {
            // TODO: add dispose
        }
    }
}