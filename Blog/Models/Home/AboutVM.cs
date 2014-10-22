using System;
using System.Linq;

namespace Blog.Models.Home
{
    public class AboutVM
    {
        public string AvatarEmail { get; set; }
        public string ContactEmail { get; set; }
        public string LinkedInLink { get; set; }
        public string GitHubLink { get; set; }

        public string AboutText { get; set; }
    }
}