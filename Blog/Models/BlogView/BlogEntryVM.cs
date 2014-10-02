using System;
using System.Linq;
using Omu.ValueInjecter;
using ValueInjecterExtensions;
using DataRepository.Models;

namespace Blog.Models.BlogView
{
    public class BlogEntryVM : BlogPostVM
    {
        public int ID { get; set; }
        public string PageTitle { get; set; }
        public string BlogTags { get; set; }

        public static BlogEntryVM BuildViewModel(BlogPost post)
        {
            var viewModel = new BlogEntryVM();
            viewModel.InjectFrom<DeepCloneInjection>(post);

            // clear up url
            viewModel.UrlTitle = StripHtml(post.UrlTitle, false);
            return viewModel;
        }
    }
}