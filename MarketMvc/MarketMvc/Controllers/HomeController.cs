using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;  //IMemoryCache
using Microsoft.AspNetCore.Authorization;   //[Authorize]
using MarketMvc.Models;
using NorthwindEntitiesLib;

namespace MarketMvc.Controllers
{
    public class HomeController : Controller
    {
        // add Northwind DB
        private NorthwindDbContext _db;
        readonly ILogger<HomeController> _logger;
        private IMemoryCache _cache;

        public HomeController(NorthwindDbContext injectedContext, ILogger<HomeController> logger, IMemoryCache memoryCache)
        {
            _db = injectedContext;
            _logger = logger;
            _cache = memoryCache;
        }

        //[ResponseCache(CacheProfileName = "Public5Minutes")]
        public IActionResult Index()
        //public async Task<IActionResult> Index()
        {
            // controller gets the model and passes it to the view
            var model = new HomeIndexViewModel
            {
                VisitorCount = (new Random()).Next(101, 1001),
                //Categories = db.Categories.ToList(),
                //Categories = _db.Categories.OrderBy(c => c.CategoryName).ToList(),
                //Categories = await db.Categories.OrderBy(c => c.CategoryName).ToListAsync(),
                Categories = GetCategories(),
                //Products = db.Products.ToList()
                //Products = _db.Products.OrderBy(p => p.ProductName).ToList()
                //Products = await db.Products.OrderBy(p => p.ProductName).ToListAsync()
                Products = GetProducts()
            };
            return View(model); // pass model to view 

            //return View();
        }
        
        protected IList<Category> GetCategories()
        {
            const string CategoryKey = "_Categories";

            //IList<Category> categories = _db.Categories.OrderBy(c => c.CategoryName).ToList();
            IList<Category> categories = null;
            if (!_cache.TryGetValue(CategoryKey, out categories))
            {
                // Key not in cache, so get data.
                categories = _db.Categories.OrderBy(c => c.CategoryName).ToList();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60));

                // Save data in cache.
                _cache.Set(CategoryKey, categories, cacheEntryOptions);
            }

            return categories;
        }

        protected IList<Product> GetProducts()
        {
            const string ProductKey = "_Products";

            //IList<Product> products = _db.Products.OrderBy(p => p.ProductName).ToList();
            IList<Product> products = null;
            if (!_cache.TryGetValue(ProductKey, out products))
            {
                // Key not in cache, so get data.
                products = _db.Products.OrderBy(p => p.ProductName).ToList();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60));

                // Save data in cache.
                _cache.Set(ProductKey, products, cacheEntryOptions);
            }

            return products;
        }
        

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // adding product details
        //public IActionResult ProductDetail(int? id)
        [Authorize]
        public async Task<IActionResult> ProductDetail(int? id)
        {
            _logger.LogInformation($"##Start## ProductDetail for id {id}");

            if (!id.HasValue)
            {
                return NotFound("You must pass a product ID in the route, for example, /Home/ProductDetail/21");
            }

            //var model = db.Products.SingleOrDefault(p => p.ProductID == id);
            //var model = await db.Products.SingleOrDefaultAsync(p => p.ProductID == id);
            //var model = await db.Products.Include(p => p.Category).Where(p => p.ProductID == id).SingleOrDefaultAsync();
            //var model = await db.Products.Include(p => p.Category).SingleOrDefaultAsync(p => p.ProductID == id);
            var model = await _db.Products.Include(p => p.Category).SingleOrDefaultAsync(p => p.ProductID == id);

            if (model == null)
            {
                return NotFound($"A product with the ID of {id} was not found.");
            }
            return View(model); // pass model to view 
        }

        // used Microsoft.EntityFrameworkCore to use the Include extension method.
        //public IActionResult ProductsThatCostMoreThan(decimal? price)
        public async Task<IActionResult> ProductsThatCostMoreThan(decimal? price)
        {
            _logger.LogInformation($"##Start## ProductsThatCostMoreThan for price {price}");

            if (!price.HasValue)
            {
                return NotFound("You must pass a product price in the query string, for example, /Home/ProductsThatCostMoreThan?price=50");
            }

            //var model = db.Products.Include(p => p.Category).Include(
            //  p => p.Supplier).Where(p => p.UnitPrice > price).ToArray();
            var model = await _db.Products.Include(p => p.Category).Include(
              p => p.Supplier).Where(p => p.UnitPrice > price).OrderBy(p => p.ProductName).ToArrayAsync();

            if (model.Count() == 0)
            //if (model.IsCompletedSuccessfully && model.Result.Count() == 0)
            {
                return NotFound($"No products cost more than {price:C}.");
            }
            ViewData["MaxPrice"] = price.Value.ToString("C");
            return View(model); // pass model to view 
        }

        public async Task<IActionResult> Category(int? id)
        {
            _logger.LogInformation($"##Start## Category for id {id}");

            if (!id.HasValue)
            {
                return NotFound("You must pass a category ID in the route, for example, /Home/Category/1");
            }

            var model = await _db.Products.Include(p => p.Category).Include(
              p => p.Supplier).Where(p => p.CategoryID == id).OrderBy(p => p.ProductName).ToArrayAsync();

            if (model.Count() == 0)
            {
                return NotFound($"No products are in category {id}.");
            }
            return View(model); // pass model to view 
        }
    }
}
