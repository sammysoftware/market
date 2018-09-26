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
using NorthwindEntitiesLib;

namespace MarketMvc.Controllers
{
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
        public async Task<IActionResult> Index()
        {
            var model = new AdminIndexViewModel
            {
                NewOrders = await _NorthwindDAL.GetNewOrders()
            };
            return View(model);
        }
    }
}