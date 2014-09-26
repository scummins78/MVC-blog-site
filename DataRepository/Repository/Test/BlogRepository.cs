using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Caching;
using DataRepository.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DataRepository.Repository.Test
{
    public class BlogRepository 
    {
        private static List<BlogPost> Blogs
        {
            get
            {
                List<BlogPost> blogs;
                if (MemoryCache.Default["BlogPosts"] == null)
                {
                    blogs = seedDataStore();
                    MemoryCache.Default["BlogPosts"] = blogs;
                }
                blogs = (List<BlogPost>)MemoryCache.Default["BlogPosts"];

                return blogs;
            }
        }

        private static void PersistBlogChanges()
        {
            MemoryCache.Default["BlogPosts"] = Blogs;
        }


        private static List<BlogPost> seedDataStore()
        {
            var blogs = new List<BlogPost>
                {
                    new BlogPost()
                        {
                            ID = 1,
                            Author = "Shaun Cummins",
                            BlogText =
                                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                            DateTimePosted = DateTime.Now.AddDays(5),
                            Title = "First Post of The Board",
                            MainImageUrl = "http://i.imgur.com/h36W3il.jpg"
                        },
                    new BlogPost()
                        {
                            ID = 2,
                            Author = "Shaun Cummins",
                            BlogText =
                                "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?",
                            DateTimePosted = DateTime.Now.AddDays(2).AddHours(5),
                            Title = "Trying Out A New Feature",
                            MainImageUrl = "http://i.imgur.com/cnniukU.png"
                        },
                    new BlogPost()
                        {
                            ID = 3,
                            Author = "Samual Jackson",
                            BlogText =
                                "The path of the righteous man is beset on all sides by the iniquities of the selfish and the tyranny of evil men. Blessed is he who, in the name of charity and good will, shepherds the weak through the valley of darkness, for he is truly his brother's keeper and the finder of lost children. And I will strike down upon thee with great vengeance and furious anger those who would attempt to poison and destroy My brothers. And you will know My name is the Lord when I lay My vengeance upon thee.",
                            DateTimePosted = DateTime.Now.AddDays(1).AddMonths(1),
                            Title = "Another Post",
                            MainImageUrl = "http://i.imgur.com/yL2jKfh.jpg"
                        },
                    new BlogPost()
                        {
                            ID = 4,
                            Author = "Samual Jackson",
                            BlogText =
                                "You think water moves fast? You should see ice. It moves like it has a mind. Like it knows it killed the world once and got a taste for murder. After the avalanche, it took us a week to climb out. Now, I don't know exactly when we turned on each other, but I know that seven of us survived the slide... and only five made it out. Now we took an oath, that I'm breaking now. We said we'd say it was the snow that killed the other two, but it wasn't. Nature is lethal but it doesn't hold a candle to man.",
                            DateTimePosted = DateTime.Now.AddDays(1).AddHours(5),
                            Title = "Guest Post",
                            MainImageUrl = "http://i.imgur.com/6kpxv5v.jpg"
                        }
                };

            return blogs;
        }



        public List<BlogPost> GetAllPosts()
        {
            return Blogs;
        }

        public List<BlogPost> FindPostsByTag(string tag)
        {
            throw new NotImplementedException();
        }

        public List<BlogPost> FindPostsByYear(int year)
        {
            var selected = Blogs.Where(x => x.DateTimePosted.Year == year).ToList();
            return selected;
        }

        public List<BlogPost> FindPostsByMonth(int year, int month)
        {
            var selected = Blogs.Where(x => x.DateTimePosted.Year == year
                                            && x.DateTimePosted.Month == month).ToList();
            return selected;
        }

        public List<BlogPost> FindPostsByDate(DateTime dateFilter)
        {
            var selected = Blogs.Where(x => x.DateTimePosted.Year == dateFilter.Year
                                            && x.DateTimePosted.Month == dateFilter.Month
                                            && x.DateTimePosted.Day == dateFilter.Day).ToList();
            return selected;
        }

        public BlogPost FindPostByTitle(DateTime dateFilter, string title)
        {
            var selected = Blogs.Where(x => x.UrlTitle == title
                                             && x.DateTimePosted.Year == dateFilter.Year
                                             && x.DateTimePosted.Month == dateFilter.Month
                                             && x.DateTimePosted.Day == dateFilter.Day);

            return selected.FirstOrDefault();
        }

        public BlogPost InsertOrUpdate(BlogPost blogPost)
        {
            if (Blogs.Exists(x => x.ID == blogPost.ID))
            {
                var updated = Blogs.Find(x => x.ID == blogPost.ID);
                Blogs.Remove(updated);
            }

            Blogs.Add(blogPost);
            PersistBlogChanges();

            return blogPost;
        }

        public BlogPost GetNewPost()
        {
            var newPost = new BlogPost { ID = Convert.ToInt32(Blogs.Count + 1), DateTimePosted = DateTime.Now };
            return newPost;
        }

        public TwitterWidget GetWidgetSettings(string widgetName)
        {
            var newWidget = new TwitterWidget
            {
                AccountUrl = ConfigurationManager.AppSettings["TwitterAccount"],
                Label = ConfigurationManager.AppSettings["TwitterLabel"],
                WidgetId = ConfigurationManager.AppSettings["TwitterWidgetId"]
            };
            return newWidget;
        }


        public Task<List<BlogPost>> GetAllPostsAsync()
        {
            var task = new Task<List<BlogPost>>(() => { return Blogs; });
            task.Start();

            return task;
            
        }

        public System.Threading.Tasks.Task<List<BlogPost>> FindPostsByTagAsync(string tag)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<List<BlogPost>> FindPostsByYearAsync(int year)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<List<BlogPost>> FindPostsByMonthAsync(int year, int month)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<List<BlogPost>> FindPostsByDateAsync(DateTime dateFilter)
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost> FindPostByTitleAsync(DateTime dateFilter, string title)
        {
            var findTask = new Task<BlogPost>(() =>
            {  
                var selected = Blogs.Where(x => x.UrlTitle == title
                                                 && x.DateTimePosted.Year == dateFilter.Year
                                                 && x.DateTimePosted.Month == dateFilter.Month
                                                 && x.DateTimePosted.Day == dateFilter.Day);

                return selected.FirstOrDefault();

            });
            findTask.Start();

            return findTask;
        }

        int IBlogRepository.InsertOrUpdate(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<int> InsertOrUpdateAsync(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }
    }
}