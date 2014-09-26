using System;
using System.Collections.Generic;
using DataRepository.Models;

namespace Blog.Models
{
    public abstract class BlogViewModel
    {
        public string Title { get; set; }
        public TwitterWidget TwitterFeed { get; set; }
    }

    public class BlogPostModel : BlogPost
    {
        public BlogPostModel() { }

        // TODO:  look for library to assist in 'flattening' objects
        public BlogPostModel(BlogPost post)
        {
            this.Author = post.Author;
            this.BlogText = post.BlogText;
            this.Title = post.Title;
            this.MainImageUrl = post.MainImageUrl;
            this.DateTimePosted = post.DateTimePosted;
            this.UrlTitle = StripHtml(post.UrlTitle, false);
        }

        /// <summary>
        /// gist of the Blog post text
        /// </summary>
        public string BlogSummary
        {
            get
            {
                if (BlogText == null) return "";
                var strippedText = StripHtml(BlogText, false);
                return strippedText.Length > 200 ? strippedText.Substring(0, 200) : strippedText;
            }
        }

        /// <summary>
        /// Sets the url title for indexing and searching (if title is set)
        /// </summary>
        public void SetUrlTitle()
        {
            if (this.Title.Length > 0)
            {
                this.Title = StripHtml(this.Title, false);
                this.UrlTitle = this.Title.Replace(' ', '-').ToLower();
            }
        }

        #region Utilities

        public static string StripHtml(string html, bool allowHarmlessTags)
        {
            if (html == null || html == string.Empty)
                return string.Empty;

            if (allowHarmlessTags)
                return System.Text.RegularExpressions.Regex.Replace(html, "", string.Empty);

            return System.Text.RegularExpressions.Regex.Replace(html, "<[^>]*>", string.Empty);
        }

        #endregion
    }

    public class BlogEntryModel : BlogViewModel
    {   
        public string BlogTags { get; set; }
        public BlogPostModel BlogPost { get; set; }
    }


    public class BlogListModel : BlogViewModel
    {
        // blog display related properties
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public IEnumerable<BlogPostModel>BlogPosts { get; set; }
        
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

        public BlogListModel(int itemsPerPage, int page, int totalPostCount, IEnumerable<BlogPost> posts)
        {
            ItemsPerPage = itemsPerPage;
            Page = page;
            TotalPostCount = totalPostCount;

            var convertedPosts = new List<BlogPostModel>();

            foreach (var post in posts)
            {
                convertedPosts.Add(new BlogPostModel(post));
            }
            BlogPosts = convertedPosts;
        }
    }
}