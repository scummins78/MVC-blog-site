using System;
using System.Linq;
using System.Net;
using System.Web.Http;

using Blog.Api.Models;
using Blog.ExceptionHandling;
using DataRepository.Repository;
using System.Threading.Tasks;


namespace Blog.Api.Controllers
{
    [HandleApiException]
    public class PostsController : ApiController
    {
        private readonly DataHelper dataHelper;

        public PostsController(IBlogRepository repository)
        {
            dataHelper = new DataHelper(repository);
        }

        /// <summary>
        /// Retrieves a list of recent posts
        /// </summary>
        /// <returns>api return object with data</returns>
        [HttpGet]
        public async Task<ApiReturnDataModel> Recent()
        {
            // get tag list
            var tags = await dataHelper.GetRecentPostLinksAsync();
            return new ApiReturnDataModel(tags, (int)HttpStatusCode.OK, true);
        }

        [HttpGet]
        public async Task<ApiReturnDataModel> Get(int page = 1, int itemsPerPage = 25, string sort = "DatePosted")
        {
            // get tag list
            var viewModel = await dataHelper.GetBlogItemsAsync(itemsPerPage, page);
            return new ApiReturnDataModel(viewModel, (int)HttpStatusCode.OK, true);
        }
    }
}
