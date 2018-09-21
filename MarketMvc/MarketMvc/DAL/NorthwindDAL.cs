using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;  //IMemoryCache
using MarketMvc.Controllers;
using NorthwindEntitiesLib;

namespace MarketMvc.DAL
{
    public class NorthwindDAL
    {
        private NorthwindDbContext _db;
        private IMemoryCache _cache;
        readonly ILogger<HomeController> _logger;
        const int cacheFromSeconds = 10;

        public NorthwindDAL(NorthwindDbContext injectedContext, IMemoryCache memoryCache, ILogger<HomeController> logger)
        {
            _db = injectedContext;
            _cache = memoryCache;
            _logger = logger;
        }

        public async Task<IList<Category>> GetCategoriesAsync()
        {
            const string CategoryKey = "_Categories";

            //IList<Category> categories = _db.Categories.OrderBy(c => c.CategoryName).ToList();
            IList<Category> categories = null;
            if (!_cache.TryGetValue(CategoryKey, out categories))
            {
                _logger.LogInformation($"##Start## GetCategories from database.");

                // Key not in cache, so get data.
                //categories = _db.Categories.OrderBy(c => c.CategoryName).ToList();
                categories = await _db.Categories.OrderBy(c => c.CategoryName).ToListAsync();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(cacheFromSeconds));

                // Save data in cache.
                _cache.Set(CategoryKey, categories, cacheEntryOptions);
            }
            else
                _logger.LogInformation($"##Start## GetCategories from cache.");

            return categories;
        }

        public async Task<IList<Product>> GetProductsAsync()
        {
            const string ProductKey = "_Products";

            //IList<Product> products = _db.Products.OrderBy(p => p.ProductName).ToList();
            IList<Product> products = null;
            if (!_cache.TryGetValue(ProductKey, out products))
            {
                _logger.LogInformation($"##Start## GetProducts from database.");

                // Key not in cache, so get data.
                //products = _db.Products.OrderBy(p => p.ProductName).ToList();
                products = await _db.Products.OrderBy(p => p.ProductName).ToListAsync();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(cacheFromSeconds));

                // Save data in cache.
                _cache.Set(ProductKey, products, cacheEntryOptions);
            }
            else
                _logger.LogInformation($"##Start## GetProducts from cache.");

            return products;
        }
    }
}
