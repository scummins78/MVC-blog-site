using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Omu.ValueInjecter;
using ValueInjecterExtensions;
using DataRepository.Models;

namespace Blog.Models.Blog
{
    public class BlogEntryVM : BlogPostVM
    {
        private readonly List<string> allowedCategories = new List<string>()
        {
            "Development",
            "Brewing"
        };
        
        public int ID { get; set; }
        public string PageTitle { get; set; }
        public string BlogTags { get; set; }
        public bool IsNew { get; set; }

        public IEnumerable<SelectListItem> PossibleCategories
        {
            get
            {
                return allowedCategories.Select(c => new SelectListItem
                {
                    Value = c,
                    Text = c
                });
            }
        }

        public static new BlogEntryVM BuildViewModel(BlogPost post)
        {
            var viewModel = new BlogEntryVM();
            viewModel.InjectFrom<DeepCloneInjection>(post);

            // flatten tags

            viewModel.BlogTags = String.Join<string>(",", 
                                    post.Tags.Select(t => t.TagValue).ToArray());

            // check to see if this is new
            if (post.ID > 0)
            {
                viewModel.IsNew = false;
            }
            else
            {
                viewModel.IsNew = true;
            }

            // clear up url
            viewModel.UrlTitle = StripHtml(post.UrlTitle, false);
            return viewModel;
        }
    }
}