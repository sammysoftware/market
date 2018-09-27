using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;   //[Authorize]
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;  //IMemoryCache
using MarketMvc.DAL;
using MarketMvc.Models;
using MarketMvc.ViewModels;
using NorthwindEntitiesLib;

namespace MarketMvc.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        readonly ILogger<HomeController> _logger;
        private NorthwindDAL _NorthwindDAL;

        public AdminController(NorthwindDbContext northwindCtx, IMemoryCache memoryCache, ILogger<HomeController> logger)
        {
            _logger = logger;
            _NorthwindDAL = new NorthwindDAL(northwindCtx, memoryCache, _logger);
        }

        [Authorize]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var model = new AdminIndexViewModel
            {
                NewOrders = await _NorthwindDAL.GetNewOrders()
            };
            return View(model);
        }

        [Authorize]
        [Route("order/{id}")]
        public IActionResult Order(int id)
        {
            var model = new AdminOrderViewModel
            {
                Order = _NorthwindDAL.GetOrder(id),
                OrderDetails = _NorthwindDAL.GetOrderDetails(id)
            };
            return View(model);
        }

        [Authorize]
        [Route("ship/{id}")]
        public IActionResult Ship(int id)
        {
            _NorthwindDAL.SetOrderShipDate(id, DateTime.Now);

            return RedirectToAction("Index");
        }
    }
}