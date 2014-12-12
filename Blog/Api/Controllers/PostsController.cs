using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Blog.Api.Models;
using Blog.ExceptionHandling;
using DataRepository.Repository;


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
        public ApiReturnDataModel Recent()
        {
            // get tag list
            var tags = dataHelper.GetRecentPostLinksAsync().Result;
            return new ApiReturnDataModel(tags, (int)HttpStatusCode.OK, true);
        }

        [HttpGet]
        public ApiReturnDataModel Get(int page = 1, int itemsPerPage = 25, string sort = "DatePosted")
        {
            // get tag list
            var viewModel = dataHelper.GetBlogItemsAsync(itemsPerPage, page).Result;
            return new ApiReturnDataModel(viewModel, (int)HttpStatusCode.OK, true);
        }
    }
}
