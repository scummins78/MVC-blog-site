using System;
using System.Collections.ObjectModel;
using System.Web.Security.AntiXss;
using System.Linq;

using System.Web.Mvc;

using DataRepository.Repository;
using DataRepository.Models;

using Blog.Models.BlogView;

namespace Blog.Controllers
{
    public class BlogController : BaseController
    {
        private const int PostsPerPage = 10;

        
        private readonly DataHelper dataHelper;
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
            // find the specific blog post requested
            var datePosted = DateTime.Parse(string.Format("{0}-{1}-{2}", year, month, day));
            var viewModel = dataHelper.FindPostAsync(datePosted, title).Result;

            return View("Entry", viewModel);
        }

        /// <summary>
        /// Gets a list of the latest blogs
        /// </summary>
        public ActionResult Index(int page)
        {
            try
            {
                if (page < 1)
                    throw new ArgumentOutOfRangeException("page", page, "page cannot be below 1");

                // display all the posts
                var viewModel = dataHelper.BuildPostListModelAsync(itemsPerPage: PostsPerPage, page: page, 
                                                        orderBy: q => q.OrderByDescending(p => p.DateTimePosted)).Result;

                return View("List", viewModel);
            }
            catch (Exception ex)
            {
                // add logging

                return View("Error");
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
            var dy = Convert.ToInt32(day);
            var mn = Convert.ToInt32(month);
            var yr = Convert.ToInt32(year);

            // get posts on a given day
            var viewModel = dataHelper.BuildPostListModelAsync(PostsPerPage, page, 
                                                            p => p.DateTimePosted.Year == yr &&
                                                            p.DateTimePosted.Month == mn &&
                                                            p.DateTimePosted.Day == dy,
                                                     q => q.OrderByDescending(p => p.DateTimePosted)).Result;
            return View("List", viewModel);
        }

        /// <summary>
        /// Retrieves blog posts by month
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public ActionResult ByMonth(string year, string month, int page)
        {
            var mn = Convert.ToInt32(month);
            var yr = Convert.ToInt32(year);

            // get posts done in a given month
            var viewModel = dataHelper.BuildPostListModelAsync(PostsPerPage, page,
                                                        p => p.DateTimePosted.Year == yr &&
                                                             p.DateTimePosted.Month == mn,
                                                        q => q.OrderByDescending(p => p.DateTimePosted)).Result;
            return View("List", viewModel);
        }

        /// <summary>
        /// Retrieves blog posts by year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public ActionResult ByYear(string year, int page)
        {
            // get posts in a given year
            // get posts done in a given month
            var yr = Convert.ToInt32(year);
            var viewModel = dataHelper.BuildPostListModelAsync(PostsPerPage, page,
                                                    p => p.DateTimePosted.Year == yr,
                                                    q => q.OrderByDescending(p => p.DateTimePosted)).Result;
            return View("List", viewModel);
        }

        /// <summary>
        /// Retrieves blog posts by tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public ActionResult ByTag(string tag, int page)
        {
            // get posts based on tag
            var searchTag = tag.ToLower();
            var viewModel = dataHelper.BuildPostListModelAsync(PostsPerPage, page,
                                                    p => p.Tags.Any(t => t.TagValue.ToLower() == searchTag),
                                                    q => q.OrderByDescending(p => p.DateTimePosted)).Result;

            // add code here
            return View("List", viewModel);
        }

        #region editing and creating posts

        /// <summary>
        /// displays blog entry screen
        /// </summary>
        /// <returns></returns>
        public ActionResult NewPost()
        {
            // check to see if user can create posts
            if (!CanCreateNewPost())
            {
                Response.StatusCode = 404;
                return View("Error");
            }

            var model = BlogEntryVM.BuildViewModel(dataHelper.GetNewPost());
            model.PageTitle = "Blog Post";

            return View("NewEntry", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NewPost(BlogEntryVM model)
        {
            model.DateTimePosted = DateTime.Now;
            model.Author = "Shaun Cummins";
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

        protected override void Dispose(bool disposing)
        {
            // TODO: add dispose
        }
    }
}