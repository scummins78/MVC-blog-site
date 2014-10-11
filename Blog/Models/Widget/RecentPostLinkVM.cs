using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Omu.ValueInjecter;
using DataRepository.Models;

namespace Blog.Models.Widget
{
    public class RecentPostLinkVM
    {
        private const string BaseImageUrl = "http://i.imgur.com/";

        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime DateTimePosted { get; set; }
        public string MainImageId { get; set; }
        public string UrlTitle { get; set; }

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

        public string DisplayDatePosted
        {
            get
            {
                return string.Format("{0} {1}", DateTimePosted.ToShortDateString(), DateTimePosted.ToShortTimeString());
            }
        }

        public string MainImageThumbnailUrl
        {
            get
            {
                return string.Format("{0}{1}s.jpg", BaseImageUrl, MainImageId);
            }
        }

        public string PostUrl
        {
            get
            {
                // build a link to the blog tag list
                return Url.Action("Entry", "Blog",
                            new
                             {
                                year = DateTimePosted.Year,
                                month = DateTimePosted.Month,
                                day = DateTimePosted.Day,
                                title = UrlTitle
                             }
                         );
            }
        }

        public static RecentPostLinkVM BuildRecentPostLinkVM(BlogPost post)
        {
            var postLink = new RecentPostLinkVM();
            postLink.InjectFrom(post);

            return postLink;
        }
    }
}