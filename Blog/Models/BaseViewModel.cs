using System;
using System.Linq;
using DataRepository.Models;

namespace Blog.Models
{
    public class BaseViewModel
    {
        public bool CanCreatePost { get; set; }
        public TwitterWidget TwitterFeed { get; set; }
    }
}