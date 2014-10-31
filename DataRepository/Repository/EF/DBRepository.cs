using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using DataRepository.Models;

namespace DataRepository.Repository.EF
{
    public class DBRepository : IBlogRepository, IDisposable
    {
        readonly BlogPostContext context;
        readonly string tagListSql = @"SELECT DISTINCT [TagValue], COUNT([TagValue]) as Instances
                                        FROM [dbo].[BlogTags]
                                        GROUP BY [TagValue]
                                        ORDER BY [Instances] DESC";

        readonly string archiveListSql = @"SELECT DATENAME(month,[DateTimePosted]) as MonthName, 
		                                        DATEPART(month,[DateTimePosted]) as Month, 
		                                        DATEPART(year,[DateTimePosted]) as Year,
		                                        COUNT([DateTimePosted]) as Instances
	                                        FROM [dbo].[BlogPosts]
	                                        GROUP BY DATEPART(year,[DateTimePosted]), 
                                                DATEPART(month,[DateTimePosted]), 
                                                DATENAME(month,[DateTimePosted]),DATENAME(year,[DateTimePosted])
	                                        ORDER BY DATEPART(year,[DateTimePosted]) DESC, 
                                                DATEPART(month,[DateTimePosted]) DESC";

        readonly string fullTextSql = @"SELECT [ID], [Title], [Author], [DateTimePosted], [MainImageId],
		                                        [BlogText], [UrlTitle], [AuthorId], [Category], RowNum 
                                        FROM
			                                (   SELECT TOP (@endRow) [ID], ROW_NUMBER() OVER(ORDER BY [DateTimePosted] DESC) AS RowNum, 
                                                    [Title], [Author], [DateTimePosted], [MainImageId], [BlogText], [UrlTitle], [AuthorId], [Category]
                                                FROM [BlogDB].[dbo].[BlogPosts]
                                                WHERE CONTAINS(([Title], [BlogText], [Category]), @searchTerm)
                                            ) AS BlogInfo
                                        WHERE RowNum BETWEEN @startRow AND @endRow";

        public DBRepository(BlogPostContext context)
        {
            this.context = context;
        }

        #region Search

        /// <summary>
        /// Gets a collection of posts based on given criteria
        /// </summary>
        /// <param name="skip">number of posts to skip for start index</param>
        /// <param name="pageSize">number of items to return</param>
        /// <param name="filter">Expression to filter list by</param>
        /// <param name="orderBy">expression to order list by</param>
        /// <param name="includeChildren">include child items</param>
        /// <returns></returns>
        public List<BlogPost> GetPosts(int skip = 0, int pageSize = 10, Expression<Func<BlogPost, bool>> filter = null,
                                Func<IQueryable<BlogPost>, IOrderedQueryable<BlogPost>> orderBy = null, bool includeChildren = true)
        {
            IQueryable<BlogPost> query = context.BlogPosts;

            // filter results
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeChildren)
            {
                query = query.Include("Tags")
                            .Include("Images");
            }

            // order results if needed
            if (orderBy != null)
            {
                return orderBy(query).Skip(skip)
                                    .Take(pageSize).ToList();
            }
            else
            {
                return query.Skip(skip)
                            .Take(pageSize).ToList();
            }
        }

        public Task<List<BlogPost>> GetPostsAsync(int skip = 0, int pageSize = 10, Expression<Func<BlogPost, bool>> filter = null,
                                Func<IQueryable<BlogPost>, IOrderedQueryable<BlogPost>> orderBy = null, bool includeChildren = true)
        {
            IQueryable<BlogPost> query = context.BlogPosts;

            // filter results
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeChildren)
            {
                query = query.Include("Tags")
                            .Include("Images");
            }

            // order results if needed
            if (orderBy != null)
            {
                return orderBy(query).Skip(skip)
                                    .Take(pageSize).ToListAsync();

            }
            else
            {
                return query.Skip(skip)
                            .Take(pageSize).ToListAsync();
            }
        }

        /// <summary>
        /// Does a full text search to retrieve any posts that mention a search term
        /// </summary>
        /// <param name="skip">number of posts to skip for start index</param>
        /// <param name="pageSize">number of items to return</param>
        /// <param name="searchTerm">term to search for</param>
        /// <param name="orderBy">expression to order list by</param>
        /// <param name="includeChildren">include child items</param>
        /// <returns></returns>
        public List<BlogPost> SearchPosts(string searchTerm, int skip = 0, int pageSize = 10)
        {
            var query = context.BlogPosts.SqlQuery(fullTextSql, new SqlParameter("@searchTerm", searchTerm),
                                                                new SqlParameter("@startRow", skip),
                                                                new SqlParameter("@endRow", skip + pageSize));
            return query.ToList();
        }

        public BlogPost FindPost(DateTime dateFilter, string title)
        {
            IQueryable<BlogPost> query = context.BlogPosts;
            query.Where(p => p.UrlTitle == title
                                                && p.DateTimePosted.Year == dateFilter.Year
                                                && p.DateTimePosted.Month == dateFilter.Month
                                                && p.DateTimePosted.Day == dateFilter.Day);

            return query.Include("Tags")
                        .Include("Images").FirstOrDefault();
        }

        public Task<BlogPost> FindPostAsync(DateTime dateFilter, string title)
        {
            var posts = context.BlogPosts.Where(p => p.UrlTitle == title
                                                && p.DateTimePosted.Year == dateFilter.Year
                                                && p.DateTimePosted.Month == dateFilter.Month
                                                && p.DateTimePosted.Day == dateFilter.Day);

            return posts.Include("Tags")
                        .Include("Images").FirstOrDefaultAsync();
        }

        public BlogPost RetrievePost(int id)
        {
            return context.BlogPosts.Where(p => p.ID == id)
                                    .Include("Tags")
                                    .Include("Images").FirstOrDefault();
        }

        public Task<BlogPost> RetrievePostAsync(int id)
        {
            return context.BlogPosts.Where(p => p.ID == id)
                                    .Include("Tags")
                                    .Include("Images").FirstOrDefaultAsync();
        }

        public int GetPostCount(Expression<Func<BlogPost, bool>> filter = null)
        {
            IQueryable<BlogPost> query = context.BlogPosts;

            // filter results
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Count();
        }

        public Task<int> GetPostCountAsync(Expression<Func<BlogPost, bool>> filter = null)
        {
            IQueryable<BlogPost> query = context.BlogPosts;

            // filter results
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.CountAsync();
        }

        public List<TagItem> GetDistinctTags()
        {
            return context.Database.SqlQuery<TagItem>(tagListSql).ToList();
        }

        public Task<List<TagItem>> GetDistinctTagsAsync()
        {
            return context.Database.SqlQuery<TagItem>(tagListSql).ToListAsync();
        }

        public List<ArchiveItem> GetArchiveItems()
        {
            return context.Database.SqlQuery<ArchiveItem>(archiveListSql).ToList();
        }

        public Task<List<ArchiveItem>> GetArchiveItemsAsync()
        {
            return context.Database.SqlQuery<ArchiveItem>(archiveListSql).ToListAsync();
        }

        #endregion

        #region CRUD methods

        public int InsertOrUpdate(BlogPost blogPost)
        {
            var post = context.BlogPosts.Find(blogPost.ID);
            if (post == null)
            {
                context.BlogPosts.Add(blogPost);
            }
            else
            {
                var postEntry = context.Entry(post);
                postEntry.CurrentValues.SetValues(blogPost);
            }

            return context.SaveChanges();
        }

        public Task<int> InsertOrUpdateAsync(BlogPost blogPost)
        {
            var post = context.BlogPosts.Find(blogPost.ID);
            if (post == null)
            {
                context.BlogPosts.Add(blogPost);
            }
            else
            {
                var postEntry = context.Entry(post);
                postEntry.CurrentValues.SetValues(blogPost);
            }

            return context.SaveChangesAsync();
        }

        public Models.BlogPost GetNewPost()
        {
            var newPost = new BlogPost { DateTimePosted = DateTime.Now };
            return newPost;
        }

        #endregion

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }   
    }
}
