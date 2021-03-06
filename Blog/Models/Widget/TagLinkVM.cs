﻿using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Omu.ValueInjecter;
using DataRepository.Models;

namespace Blog.Models.Widget
{
    public class TagLinkVM
    {
        public string TagValue { get; set; }
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
        

        public string TagUrl
        {
            get
            {
                // build a link to the blog tag list
                return Url.Action("ByTag", "Blog", new { tag=TagValue });
            }
        }

        public static TagLinkVM BuildTagLinkVM(TagItem tag)
        {
            var tagLink = new TagLinkVM();
            tagLink.InjectFrom(tag);

            return tagLink;
        }
    }
}