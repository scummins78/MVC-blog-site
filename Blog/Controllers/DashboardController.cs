using System;
using System.Collections.ObjectModel;
using System.Web.Security.AntiXss;
using System.Linq;
using System.Web;

using System.Web.Mvc;

using Blog.Models.Blog;
using DataRepository.Repository;
using DataRepository.Models;
using NLog;

namespace Blog.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private readonly DataHelper dataHelper;
        private readonly Logger logger = LogManager.GetLogger("DashboardController");

        public DashboardController(IBlogRepository repository)
        {
            dataHelper = new DataHelper(repository);
        }


        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        #region editing and creating posts

        /// <summary>
        /// displays blog entry screen
        /// </summary>
        /// <returns></returns>
        public ActionResult BlogPost(int id = 0)
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
                BlogPost post;
                if (id > 0)
                {
                    post = dataHelper.RetrievePost(id);
                }
                else
                {
                    post = dataHelper.GetNewPost();
                }

                var model = BlogEntryVM.BuildViewModel(post);
                model.PageTitle = "Blog Post";

                return View("Entry", model);
            }
            catch (Exception ex)
            {
                return HandleExceptions("Error occurred in NewPost Get.", ex, logger);
            }
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult BlogPost(BlogEntryVM model)
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
                return HandleExceptions("Error occurred in NewPost POST.", ex, logger);
            }
        }

        #endregion

        #region CRUD Helper methods

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

        
    }
}