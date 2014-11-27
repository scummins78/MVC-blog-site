using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public DateTime DateTimeUpdated { get; set; }
        public DateTime DateTimePublished { get; set; }
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

            model.UrlTitle = StripUrl(model.UrlTitle);
            return model;
        }

        // methods
        public static BlogPostVM BuildViewModel(BlogPost post)
        {
            var viewModel = new BlogPostVM();
            viewModel.InjectFrom<DeepCloneInjection>(post);

            // clear up url
            viewModel.UrlTitle = StripUrl(post.UrlTitle);
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

        internal static string StripUrl(string url) 
        {
            var reservedCharacters = "!*'();:@&=+$,/?%#[]<>";

            if (String.IsNullOrEmpty(url))
            return String.Empty;

            var sb = new StringBuilder();

            foreach (char @char in url)
            {
                if (reservedCharacters.IndexOf(@char) == -1)
                {
                    sb.Append(@char);
                }
            }
            return sb.ToString();
        }
    }
}