using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MarketMvc.DAL;
using MarketMvcTest.Stubs;
using MarketMvcTest.Fakes;
using MarketMvcTest.Mocks;
using NorthwindEntitiesLib;

namespace MarketMvcTest
{
    [TestClass]
    public class NorthwindDALTests
    {
        [TestMethod]
        public async System.Threading.Tasks.Task GetCategoriesAsync_NotInCache_GetCategoriesFromDatabase()
        {
            //Arrange
            NorthwindDbContext stubNorthwindDBContext = StubDbContext.GetContextWithData();
            FakeCache fakeCache = new FakeCache(false);
            MockLogger mockLogger = new MockLogger();
            NorthwindDAL northwindDAL = new NorthwindDAL(stubNorthwindDBContext, fakeCache, mockLogger);

            //Act
            IList<Category> Categories = await northwindDAL.GetCategoriesAsync();

            //Assert
            Assert.IsTrue(mockLogger.DidLog("##Start## GetCategories from database."));
        }

        [TestMethod]
        public async System.Threading.Tasks.Task GetCategoriesAsync_InCache_GetCategoriesFromCacheAsync()
        {
            //Arrange
            NorthwindDbContext stubNorthwindDBContext = StubDbContext.GetContextWithData();
            FakeCache fakeCache = new FakeCache(true);
            //fakeCache.Set("", "categories");
            MockLogger mockLogger = new MockLogger();

            NorthwindDAL northwindDAL = new NorthwindDAL(stubNorthwindDBContext, fakeCache, mockLogger);

            //Act
            IList<Category> Categories = await northwindDAL.GetCategoriesAsync();

            //Assert
            //Assert.IsNotNull(Categories);
            Assert.IsTrue(mockLogger.DidLog("##Start## GetCategories from cache."));
        }
    }
}
