using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blog.ExceptionHandling;
using Blog.Models.Blog;
using DataRepository.Repository;
using NLog;

namespace Blog.Controllers
{
    [HandleException]
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
        public async Task<ActionResult> Entry(string year, string month, string day, string title)
        {
            // find the specific blog post requested
            var datePosted = DateTime.Parse(string.Format("{0}-{1}-{2}", year, month, day));
            var viewModel = await dataHelper.FindPostAsync(datePosted, title);

            return View("Entry", viewModel);
        }
        
        /// <summary>
        /// Gets a list of the latest blogs
        /// </summary>
        public async Task<ActionResult> Index(string category, int page)
        {
            if (page < 1)
                throw new ArgumentOutOfRangeException("page", page, "page cannot be below 1");
                
            // create filter if needed
            BlogListVM viewModel;
            if (category == "default"){
                viewModel = await dataHelper.BuildPostListModelAsync(itemsPerPage: PostsPerPage, page: page,
                                                    orderBy: q => q.OrderByDescending(p => p.DateTimePosted));
            }
            else
            {
                viewModel = await dataHelper.BuildPostListModelAsync(itemsPerPage: PostsPerPage, page: page,
                                                    filter: p => p.Category == category, 
                                                    orderBy: q => q.OrderByDescending(p => p.DateTimePosted));
            }

            // set sub title based on category
            viewModel.PageSubTitle = string.Format("Posts in '{0}' category", category);

            // if this is page 1 with the default category, show splash
            if (page == 1 && category == "default")
            {
                return View("Splash", viewModel);
            }

            return View("List", viewModel);
        }

        /// <summary>
        /// Retrieves blog list entered on a given day
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public async Task<ActionResult> ByDay(string year, string month, string day, int page)
        {
            if (page < 1)
                throw new ArgumentOutOfRangeException("page", page, "page cannot be below 1");
                
            var dy = Convert.ToInt32(day);
            var mn = Convert.ToInt32(month);
            var yr = Convert.ToInt32(year);

            // get posts on a given day
            var viewModel = await dataHelper.BuildPostListModelAsync(itemsPerPage: PostsPerPage, page: page,
                                                    filter: p => p.DateTimePosted.Year == yr &&
                                                            p.DateTimePosted.Month == mn &&
                                                            p.DateTimePosted.Day == dy,
                                                        orderBy: q => q.OrderByDescending(p => p.DateTimePosted));

            // set sub title based on date
            viewModel.PageSubTitle = string.Format("Posts made on '{0}/{1}/{2}'", month, day, year);

            return View("List", viewModel);
        }

        /// <summary>
        /// Retrieves blog posts by month
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task<ActionResult> ByMonth(string year, string month, int page)
        {
            if (page < 1)
                throw new ArgumentOutOfRangeException("page", page, "page cannot be below 1");

            var mn = Convert.ToInt32(month);
            var yr = Convert.ToInt32(year);

            // get posts done in a given month
            var viewModel = await dataHelper.BuildPostListModelAsync(itemsPerPage: PostsPerPage, page: page,
                                                        filter: p => p.DateTimePosted.Year == yr &&
                                                                p.DateTimePosted.Month == mn,
                                                        orderBy: q => q.OrderByDescending(p => p.DateTimePosted));

            // set sub title based on date
            viewModel.PageSubTitle = string.Format("Posts made during '{0}, {1}'", 
                                CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mn),  year);

            return View("List", viewModel);
            
        }

        /// <summary>
        /// Retrieves blog posts by year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<ActionResult> ByYear(string year, int page)
        {
            if (page < 1)
                throw new ArgumentOutOfRangeException("page", page, "page cannot be below 1");
                
            // get posts in a given year
            // get posts done in a given month
            var yr = Convert.ToInt32(year);
            var viewModel = await dataHelper.BuildPostListModelAsync(PostsPerPage, page,
                                                    p => p.DateTimePosted.Year == yr,
                                                    q => q.OrderByDescending(p => p.DateTimePosted));

            // set sub title based on date
            viewModel.PageSubTitle = string.Format("Posts made during '{0}'", year);

            return View("List", viewModel); 
        }

        /// <summary>
        /// Retrieves blog posts by tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task<ActionResult> ByTag(string tag, int page)
        {
            if (page < 1)
                    throw new ArgumentOutOfRangeException("page", page, "page cannot be below 1");
            
            // get posts based on tag
            var searchTag = tag.ToLower();
            var viewModel = await dataHelper.BuildPostListModelAsync(PostsPerPage, page,
                                                    p => p.Tags.Any(t => t.TagValue.ToLower() == searchTag),
                                                    q => q.OrderByDescending(p => p.DateTimePosted));

            // set sub title based on date
            viewModel.PageSubTitle = string.Format("Posts tagged @'{0}'", searchTag);

            // add code here
            return View("List", viewModel);
        }

        public async Task<ActionResult> Search(string term, int page)
        {
            if (page < 1)
                    throw new ArgumentOutOfRangeException("page", page, "page cannot be below 1");
            
            var viewModel = await dataHelper.SearchPostsAsync(term);
            
            // set sub title based on search criteria
            viewModel.PageSubTitle = string.Format("Posts Containing '{0}'", term);

            return View("List", viewModel);
        }

        protected override void Dispose(bool disposing)
        {
            // TODO: add dispose
        }
    }
}