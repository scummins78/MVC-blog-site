using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataRepository.Repository;
using DataRepository.Repository.EF;

namespace DataRepository.Tests
{
    [TestClass]
    public class BlogRepositoryTest
    {
        [TestMethod]
        public void SearchPostsWithResults_Test()
        {
            try
            {
                // arrange
                var dbContext = new BlogPostContext();
                IBlogRepository repository = new DBRepository(dbContext);

                // act
                var results = repository.SearchPosts("Brewing", 0, 10);

                // asert
                Assert.AreEqual(results.Count, 2);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message, ex);
            }
        }
        
        [TestMethod]
        public void SearchPostsNoResults_Test()
        {
            try
            {
                // arrange
                var dbContext = new BlogPostContext();
                IBlogRepository repository = new DBRepository(dbContext);

                // act
                var results = repository.SearchPosts("Brewing", 20, 10);

                // asert
                Assert.AreEqual(results.Count, 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message, ex);
            }
        }
    }
}
