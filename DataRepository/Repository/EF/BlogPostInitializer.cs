using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DataRepository.Models;

namespace DataRepository.Repository.EF
{
    class BlogPostInitializer : DropCreateDatabaseIfModelChanges<BlogPostContext>
    {
        protected override void Seed(BlogPostContext context)
        {
            var posts = new List<BlogPost>
            {
                new BlogPost
                {
                    Author = "Shaun Cummins",
                    BlogText =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                    DateTimePosted = DateTime.Now.AddDays(5),
                    Title = "First Post of The Board",
                    UrlTitle = "first-post-of-the-board",
                    MainImageId = "h36W3il",
                    Tags = new ObservableCollection<BlogTag>{
                        new BlogTag
                        {
                            TagValue = "FirstPost"
                        },
                        new BlogTag
                        {
                            TagValue = "Development"
                        }
                    }
                },
                new BlogPost
                {   
                    Author = "Shaun Cummins",
                    BlogText =
                        "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?",
                    DateTimePosted = DateTime.Now.AddDays(2).AddHours(5),
                    Title = "Trying Out A New Feature",
                    UrlTitle = "trying-out-a-new-feature",
                    MainImageId = "cnniukU",
                    Tags = new ObservableCollection<BlogTag>{
                        new BlogTag
                        {
                            TagValue = "SecondPost"
                        },
                        new BlogTag
                        {
                            TagValue = "Development"
                        }
                    }
                }
            };

            posts.ForEach(p => context.BlogPosts.Add(p));
            context.SaveChanges();
            
            base.Seed(context);
        }
    }
}
