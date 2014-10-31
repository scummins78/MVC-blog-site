using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Omu.ValueInjecter;
using DataRepository.Models;

namespace Blog.Models.Widget
{
    public class ArchiveLinkVM
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string MonthName { get; set; }
        public int Instances { get; set; }

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
        
        public string ArchiveUrl
        {
            get
            {
                // build a link to the blog tag list
                return Url.Action("ByMonth", "Blog", new
                             {
                                year = Year.ToString(),
                                month = Month.ToString().Length == 1 ? "0" + Month.ToString() : Month.ToString()
                             });
            }
        }

        public static ArchiveLinkVM BuildArchiveLinkVM(ArchiveItem archive)
        {
            var archiveLink = new ArchiveLinkVM();
            archiveLink.InjectFrom(archive);

            return archiveLink;
        }

    }
}