using System;
using System.Linq;
using DataRepository.Models;

namespace Blog.Models
{
    public class BaseViewModel
    {
        public bool CanCreatePost { get; set; }
        public TwitterWidget TwitterFeed { get; set; }
        public string GitUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string FacebookUrl { get; set; }
    }
}