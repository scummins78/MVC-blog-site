using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Security.AntiXss;
using System.Net;

using DataRepository.Models;
using DataRepository.Repository;

using Blog.Models.BlogView;
using Blog.Models.Widget;

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
        /// Retrieves posts and builds
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
        /// 
        /// </summary>
        /// <param name="dateFilter"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<BlogEntryVM> FindPostAsync(DateTime dateFilter, string title)
        {
            var post = await blogRepository.FindPostAsync(dateFilter, title).ConfigureAwait(false);

            var model = BlogEntryVM.BuildViewModel(DecodeHtmlForDisplay(post));
            model.PageTitle = "Blog Post";

            return model;
        }

        public async Task<List<TagLinkVM>> GetTagsAsync()
        {
            var tags = await blogRepository.GetDistinctTagsAsync().ConfigureAwait(false);
            return tags.Distinct(new TagComparer()).Select(t => TagLinkVM.BuildTagLinkVM(t)).ToList();
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
                Heading = "Blog Title Goes Here",
                SubHeading = "This is the Sub Title",
                PageTitle = "Blog Home"
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