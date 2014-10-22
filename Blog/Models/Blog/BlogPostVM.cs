using System;
using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using ValueInjecterExtensions;
using DataRepository.Models;

namespace Blog.Models.Blog
{
    public class BlogPostVM
    {
        private const string BaseImageUrl = "http://i.imgur.com/";


        // properties
        public string Author { get; set; }
        public string AuthorId { get; set; }
        public string BlogText { get; set; }
        public string Category { get; set; }
        public DateTime DateTimePosted { get; set; }
        public string MainImageId { get; set; }
        public string Title { get; set; }
        public string UrlTitle { get; set; }
        public ICollection<BlogTagVM> Tags { get; set; }
        public ICollection<ImageUrlVM> Images { get; set; }

        public string MainImageUrl
        {
            get
            {
                return string.Format("{0}{1}l.jpg", BaseImageUrl, MainImageId);
            }
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

        public BlogPost BuildModel()
        {
            var model = new BlogPost();
            model.InjectFrom<DeepCloneInjection>(this);

            return model;
        }

        // methods
        public static BlogPostVM BuildViewModel(BlogPost post)
        {
            var viewModel = new BlogPostVM();
            viewModel.InjectFrom<DeepCloneInjection>(post);

            // clear up url
            viewModel.UrlTitle = StripHtml(post.UrlTitle, false);
            return viewModel;
        }

        internal static string StripHtml(string html, bool allowHarmlessTags)
        {
            if (html == null || html == string.Empty)
                return string.Empty;

            if (allowHarmlessTags)
                return System.Text.RegularExpressions.Regex.Replace(html, "", string.Empty);

            return System.Text.RegularExpressions.Regex.Replace(html, "<[^>]*>", string.Empty);
        }

    }
}