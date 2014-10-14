using System;
using System.Linq;
using DataRepository.Models;

namespace Blog.Models
{
    public class BaseVM
    {
        /* user account related properties */
        public string DisplayName { get; set; }
        public bool CanCreatePost { get; set; }

        /* page links and settings */
        public TwitterWidget TwitterFeed { get; set; }
        public string GitUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string FacebookUrl { get; set; }
    }
}