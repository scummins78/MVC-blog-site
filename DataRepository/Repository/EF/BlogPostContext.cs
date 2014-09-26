using System;
using System.Data.Entity;
using System.Linq;
using DataRepository.Models;

namespace DataRepository.Repository.EF
{
    public class BlogPostContext : BaseContext<BlogPostContext>
    {
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogImageUrl> ImageUrls { get; set; }
        public DbSet<BlogTag> Tags { get; set; } 
    }
}
