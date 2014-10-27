using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Security.AntiXss;
using System.Net;

using Blog.Models.Blog;
using Blog.Models.Dashboard;
using Blog.Models.Widget;
using DataRepository.Models;
using DataRepository.Repository;


namespace Blog
{
    /// <summary>
    /// Helper class to assist with retrieving and sending data to the repository
    /// Handles HTTP functionality to 'scrub' and data posted or retrieved from the database
    /// </summary>
    public class DataHelper
    {
        private readonly IBlogRepository blogRepository;
        public DataHelper(IBlogRepository repository)
        {
            blogRepository = repository;
        }

        #region search methods

        /// <summary>
        /// Retrieves posts and builds a view model for display
        /// </summary>
        /// <param name="postCount">number of posts per page</param>
        /// <param name="page">page being requested</param>
        /// <param name="filter">search filter</param>
        /// <param name="orderBy">sort for posts</param>
        /// <returns></returns>
        public async Task<BlogListVM> BuildPostListModelAsync(int itemsPerPage = 10, int page = 1, Expression<Func<BlogPost, bool>> filter = null,
                                Func<IQueryable<BlogPost>, IOrderedQueryable<BlogPost>> orderBy = null)
        {
            var postCount = await blogRepository.GetPostCountAsync(filter).ConfigureAwait(false);

            // retrieve a 'page' of items
            var startingPoint = page > 1 ? itemsPerPage * (page - 1) : 0;
            List<BlogPost> posts = await blogRepository.GetPostsAsync(startingPoint, itemsPerPage, filter, orderBy).ConfigureAwait(false);
            
            // build BlogListViewModel
            return BuildBlogListModel(itemsPerPage, page, postCount, DecodeBlogList(posts));
        }

        /// <summary>
        /// Retrieves post based on the date posted and post title
        /// </summary>
        /// <param name="dateFilter">date to filter by</param>
        /// <param name="title">title to filter by</param>
        /// <returns></returns>
        public async Task<BlogEntryVM> FindPostAsync(DateTime dateFilter, string title)
        {
            var post = await blogRepository.FindPostAsync(dateFilter, title).ConfigureAwait(false);

            var model = BlogEntryVM.BuildViewModel(DecodeHtmlForDisplay(post));
            model.PageTitle = model.Title;

            return model;
        }

        /// <summary>
        /// Retrieves a list of all of the unique tags, along with the amount of times
        /// those tags were used.  Grouped by amount
        /// </summary>
        /// <returns></returns>
        public async Task<List<TagLinkVM>> GetTagsAsync()
        {
            var tags = await blogRepository.GetDistinctTagsAsync().ConfigureAwait(false);
            return tags.Select(t => TagLinkVM.BuildTagLinkVM(t)).ToList();
        }

        /// <summary>
        /// Retrieves a list of the month and date of all of the active posts, ordered
        /// and grouped by amount
        /// </summary>
        /// <returns></returns>
        public async Task<List<ArchiveLinkVM>> GetArchiveLinksAsync()
        {
            var links = await blogRepository.GetArchiveItemsAsync().ConfigureAwait(false);
            return links.Select(l => ArchiveLinkVM.BuildArchiveLinkVM(l)).ToList();
        }

        /// <summary>
        /// Retrieves a list of the last 5 recent posts.  
        /// </summary>
        /// <returns></returns>
        public async Task<List<RecentPostLinkVM>> GetRecentPostLinksAsync()
        {
            var posts = await blogRepository.GetPostsAsync(skip: 0, pageSize: 5,
                                                    orderBy: q => q.OrderByDescending(p => p.DateTimePosted),
                                                    includeChildren: false).ConfigureAwait(false);

            return posts.Select(p => RecentPostLinkVM.BuildRecentPostLinkVM(p)).ToList();
        }

        /// <summary>
        /// Retrieves a list of blog posts based on the page properties.  For display in the 
        /// dashboard
        /// </summary>
        /// <param name="itemsPerPage">number of items to retrieve</param>
        /// <param name="page">page of items to retrieve</param>
        /// <returns></returns>
        public async Task<BlogTableVM> GetBlogItemsAsync(int itemsPerPage = 10, int page = 1)
        {
            var postCount = await blogRepository.GetPostCountAsync().ConfigureAwait(false);

            // retrieve a 'page' of items
            var startingPoint = page > 1 ? itemsPerPage * (page - 1) : 0;

            var posts = await blogRepository.GetPostsAsync(skip: startingPoint, pageSize: itemsPerPage,
                                        orderBy: q => q.OrderByDescending(p => p.DateTimePosted),
                                        includeChildren: true).ConfigureAwait(false);

            var viewModel = new BlogTableVM(itemsPerPage)
            {
                CurrentPage = page,
                PostCount = postCount,
                Posts = posts.Select(p => BlogItemVM.BuildViewModel(p)).ToList()
            };

            return viewModel;
        }

        #endregion

        #region CRUD methods

        public Task<int> InsertOrUpdateAsync(BlogPost blogPost){

            ScrubPostForStorage(blogPost);
            return blogRepository.InsertOrUpdateAsync(blogPost);
        }

        public int InsertOrUpdate(BlogPost blogPost)
        {
            
            ScrubPostForStorage(blogPost);
            return blogRepository.InsertOrUpdate(blogPost);
        }

        public BlogPost GetNewPost()
        {
            return blogRepository.GetNewPost();
        }

        public BlogPost RetrievePost(int id)
        {
            var post = blogRepository.RetrievePost(id);
            return DecodeHtmlForDisplay(post);
        }

        #endregion

        #region scrubbing and decoding

        /// <summary>
        /// encodes fields so content is safe for storage
        /// </summary>
        /// <param name="post">blog post</param>
        /// <returns>scrubbed post</returns>
        private static void ScrubPostForStorage(BlogPost post)
        {
            post.Author = AntiXssEncoder.HtmlEncode(post.Author, true);
            post.BlogText = AntiXssEncoder.HtmlEncode(post.BlogText, true);
            post.Title = AntiXssEncoder.HtmlEncode(post.Title, true);
            post.MainImageId = AntiXssEncoder.UrlEncode(post.MainImageId);
        }

        private static List<BlogPost> DecodeBlogList(List<BlogPost> blogs)
        {
            foreach (var blog in blogs)
            {
                DecodeHtmlForDisplay(blog);
            }
            return blogs;
        }

        /// <summary>
        /// Decodes previously encoded posts for display in an html page
        /// </summary>
        /// <param name="post">post from storage</param>
        /// <returns>displayable post</returns>
        private static BlogPost DecodeHtmlForDisplay(BlogPost post)
        {
            post.Author = WebUtility.HtmlDecode(post.Author);
            post.BlogText = WebUtility.HtmlDecode(post.BlogText);
            post.Title = WebUtility.HtmlDecode(post.Title);
            post.MainImageId = WebUtility.UrlDecode(post.MainImageId);

            return post;
        }

        #endregion

        #region view model building

        private BlogListVM BuildBlogListModel(int itemsPerPage, int page, int totalPostCount, IList<BlogPost> posts)
        {
            return new BlogListVM(itemsPerPage, page, totalPostCount, posts)
            {
                Heading = "Under Construction",
                SubHeading = "A Work In Progress Blog Site",
                PageTitle = "Under Construction"
            };
        }

        #endregion
    }

    public class TagComparer : IEqualityComparer<BlogTag>
    {
        public bool Equals(BlogTag x, BlogTag y)
        {
            return x.TagValue.Equals(y.TagValue);
        }

        public int GetHashCode(BlogTag obj)
        {
            return obj.TagValue.GetHashCode();
        }
    }
}