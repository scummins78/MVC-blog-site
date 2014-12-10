using System;
using System.Linq;
using System.Net;
using System.Web.Http;

using Blog.Api.Models;
using Blog.ExceptionHandling;
using DataRepository.Repository;

namespace Blog.Api.Controllers
{
    [HandleApiException]
    public class TagsController : ApiController
    {
        private readonly DataHelper dataHelper;

        public TagsController(IBlogRepository repository)
        {
            dataHelper = new DataHelper(repository);
        }

        /// <summary>
        /// Retrieves a list of unique tags within the system
        /// </summary>
        /// <returns>api return object with data</returns>
        [HttpGet]
        public ApiReturnDataModel Get()
        {
            // get tag list
            var tags = dataHelper.GetTagsAsync().Result;
            return new ApiReturnDataModel(tags, (int)HttpStatusCode.OK, true);
        }
    }
}
