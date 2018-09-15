using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
                Categories = db.Categories.ToList(),
                Products = db.Products.ToList()
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
    }
}
