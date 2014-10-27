using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models.Dashboard
{
    public class BlogTableVM
    {
        private readonly int itemsPerPage = 0;

        public BlogTableVM(int itemsPerPage)
        {
            this.itemsPerPage = itemsPerPage;
        }
        
        public List<BlogItemVM> Posts { get; set; }
        public int PostCount { get; set; }
        public int CurrentPage { get; set; }
        
        public int TotalPages
        {
            get
            {
                var pageCount =  PostCount / itemsPerPage;
                if (PostCount % itemsPerPage != 0)
                {
                    pageCount++;
                }

                return pageCount;
            }
        }   
    }
}