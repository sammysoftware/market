using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NorthwindEntitiesLib;

namespace MarketMvcTest.Stubs
{
    class StubDbContext
    {
        public static NorthwindDbContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<NorthwindDbContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;
            var context = new NorthwindDbContext(options);

            var beerCategory = new Category { CategoryID = 1, CategoryName = "Beers" };
            var wineCategory = new Category { CategoryID = 2, CategoryName = "Wines" };
            context.Categories.Add(beerCategory);
            context.Categories.Add(wineCategory);

            return context;
        }
    }
}
