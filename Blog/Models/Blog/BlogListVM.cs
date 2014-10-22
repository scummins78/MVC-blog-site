using System;
using System.Collections.Generic;
using System.Linq;
using DataRepository.Models;

namespace Blog.Models.Blog
{
    public class BlogListVM
    {
        // blog display related properties
        public string PageTitle { get; set; }
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public IEnumerable<BlogPostVM>BlogPosts { get; set; }

        // paging related properties
        private int TotalPostCount {get; set;}
        private int ItemsPerPage {get; set;}
      
        private int PageCount
        {
            get
            {
                return TotalPostCount > 0 ? (int)Math.Ceiling(TotalPostCount / (double)ItemsPerPage) : 0;
            }
        }

        // these properties will determine if view needs previous or next links
        public int Page { get; set; }

        public bool HasPreviousPage{
            get
            {
                return Page > 1;
            }
        }

        public bool HasNextPage{
            get
            {
                return Page < PageCount;
            }
        }

        public BlogListVM(int itemsPerPage, int page, int totalPostCount, IEnumerable<BlogPost> posts)
        {
            ItemsPerPage = itemsPerPage;
            Page = page;
            TotalPostCount = totalPostCount;

            var convertedPosts = new List<BlogPostVM>();

            foreach (var post in posts)
            {
                convertedPosts.Add(BlogPostVM.BuildViewModel(post));
            }
            BlogPosts = convertedPosts;
        }
    }
}