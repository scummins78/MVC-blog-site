using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataRepository.Models;

namespace DataRepository.Repository
{
    public interface IBlogRepository
    {
        #region Search

        List<BlogPost> GetPosts(int skip = 0, int pageSize = 10, Expression<Func<BlogPost, bool>> filter = null,
                                Func<IQueryable<BlogPost>, IOrderedQueryable<BlogPost>> orderBy = null);

        Task<List<BlogPost>> GetPostsAsync(int skip = 0, int pageSize = 10, Expression<Func<BlogPost, bool>> filter = null,
                                Func<IQueryable<BlogPost>, IOrderedQueryable<BlogPost>> orderBy = null);

        int GetPostCount(Expression<Func<BlogPost, bool>> filter = null);

        Task<int> GetPostCountAsync(Expression<Func<BlogPost, bool>> filter = null);

        BlogPost FindPost(DateTime dateFilter, string title);

        Task<BlogPost> FindPostAsync(DateTime dateFilter, string title);

        List<TagItem> GetDistinctTags();

        Task<List<TagItem>> GetDistinctTagsAsync();

        #endregion

        #region CRUD actions

        int InsertOrUpdate(BlogPost blogPost);

        Task<int> InsertOrUpdateAsync(BlogPost blogPost);

        BlogPost GetNewPost();

        #endregion

        void Dispose();
    }
}

