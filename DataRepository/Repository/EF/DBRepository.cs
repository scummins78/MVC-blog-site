using System;
using System.Collections.Generic;
using System.Data.Entity;
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
	                                        ORDER BY DATEPART(year,[DateTimePosted]), 
                                                DATEPART(month,[DateTimePosted])";

        public DBRepository(BlogPostContext context)
        {
            this.context = context;
        }

        #region Search

        public List<BlogPost> GetPosts(int skip = 0, int pageSize = 10, Expression<Func<BlogPost, bool>> filter = null,
                                Func<IQueryable<BlogPost>, IOrderedQueryable<BlogPost>> orderBy = null)
        {
            IQueryable<BlogPost> query = context.BlogPosts;

            // filter results
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // order results if needed
            if (orderBy != null)
            {
                return orderBy(query).Skip(skip)
                                    .Take(pageSize)
                                    .Include("Tags")
                                    .Include("Images").ToList();
            }
            else
            {
                return query.Skip(skip)
                            .Take(pageSize)
                            .Include("Tags")
                            .Include("Images").ToList();
            }
        }

        public Task<List<BlogPost>> GetPostsAsync(int skip = 0, int pageSize = 10, Expression<Func<BlogPost, bool>> filter = null,
                                Func<IQueryable<BlogPost>, IOrderedQueryable<BlogPost>> orderBy = null)
        {
            IQueryable<BlogPost> query = context.BlogPosts;

            // filter results
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // order results if needed
            if (orderBy != null)
            {
                return orderBy(query).Skip(skip)
                                    .Take(pageSize)
                                    .Include("Tags")
                                    .Include("Images").ToListAsync();
            }
            else
            {
                return query.Skip(skip)
                            .Take(pageSize)
                            .Include("Tags")
                            .Include("Images").ToListAsync();
            }
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
                postEntry.CurrentValues.SetValues(post);
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
                postEntry.CurrentValues.SetValues(post);
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
