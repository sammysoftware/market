using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarketMvc.Models;
using NorthwindEntitiesLib;

namespace MarketMvc.Controllers
{
    public class HomeController : Controller
    {
        // add Northwind DB
        private Northwind db;

        public HomeController(Northwind injectedContext)
        {
            db = injectedContext;
        }

        public IActionResult Index()
        {
            // controller gets the model and passes it to the view
            var model = new HomeIndexViewModel
            {
                VisitorCount = (new Random()).Next(101, 1001),
                //Categories = db.Categories.ToList(),
                Categories = db.Categories.OrderBy(c => c.CategoryName).ToList(),
                //Products = db.Products.ToList()
                Products = db.Products.OrderBy(p => p.ProductName).ToList()
            };
            return View(model); // pass model to view 

            //return View();
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
        public IActionResult ProductDetail(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a product ID in the route, for example, /Home/ProductDetail/21");
            }
            var model = db.Products.SingleOrDefault(p => p.ProductID == id);
            if (model == null)
            {
                return NotFound($"A product with the ID of {id} was not found.");
            }
            return View(model); // pass model to view 
        }

        // used Microsoft.EntityFrameworkCore to use the Include extension method.
        public IActionResult ProductsThatCostMoreThan(decimal? price)
        {
            if (!price.HasValue)
            {
                return NotFound("You must pass a product price in the query string, for example, /Home/ProductsThatCostMoreThan?price=50");
            }
            var model = db.Products.Include(p => p.Category).Include(
              p => p.Supplier).Where(p => p.UnitPrice > price).ToArray();
            if (model.Count() == 0)
            {
                return NotFound($"No products cost more than {price:C}.");
            }
            ViewData["MaxPrice"] = price.Value.ToString("C");
            return View(model); // pass model to view 
        }
    }
}
