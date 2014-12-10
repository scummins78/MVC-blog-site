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
    public class ArchiveController : ApiController
    {
        private readonly DataHelper dataHelper;

        public ArchiveController(IBlogRepository repository)
        {
            dataHelper = new DataHelper(repository);
        }

        /// <summary>
        /// Retrieves a list of blog archive links 
        /// </summary>
        /// <returns>api return object with data</returns>
        [HttpGet]
        public ApiReturnDataModel Get()
        {
            // get tag list
            var tags = dataHelper.GetArchiveLinksAsync().Result;
            return new ApiReturnDataModel(tags, (int)HttpStatusCode.OK, true);
        }
    }
}
