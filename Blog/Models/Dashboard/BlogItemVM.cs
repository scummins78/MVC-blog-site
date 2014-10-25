using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Blog.Models.Blog;
using Omu.ValueInjecter;
using ValueInjecterExtensions;
using DataRepository.Models;

namespace Blog.Models.Dashboard
{
    public class BlogItemVM
    {
        // properties
        public int ID { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public DateTime DateTimePosted { get; set; }
        public string Title { get; set; }

        public ICollection<BlogTagVM> Tags { get; set; }

        // assist in building url values
        private UrlHelper url = null;
        private UrlHelper Url
        {
            get
            {
                if (url == null)
                {
                    url = new UrlHelper(HttpContext.Current.Request.RequestContext);
                }

                return url;
            }
        }

        public string TagList
        {
            get
            {
                return String.Join(",", Tags.Select(t => t.TagValue).ToList());
            }
        }

        public string DatePosted
        {
            get
            {
                return DateTimePosted.ToString("MM/dd/yyyy HH:mm:ss.fff",
                                CultureInfo.InvariantCulture);
            }
        }

        public string EditPostUrl
        {
            get
            {
                // build a link to the blog tag list
                return Url.Action("BlogPost", "Dashboard", new { id=ID });
            }
        }

        // methods
        public static BlogItemVM BuildViewModel(BlogPost post)
        {
            var viewModel = new BlogItemVM();
            viewModel.InjectFrom<DeepCloneInjection>(post);

            return viewModel;
        }
    }
}