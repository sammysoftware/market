using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;  //IMemoryCache
using MarketMvc.Models;
using MarketMvc.ViewModels;
using MarketMvc.DAL;
using NorthwindEntitiesLib;

namespace MarketMvc.Controllers
{
    [Route("cart")]
    public class CartController : Controller
    {
        readonly ILogger<HomeController> _logger;
        private NorthwindDAL _NorthwindDAL;

        public CartController(NorthwindDbContext northwindCtx, IMemoryCache memoryCache, ILogger<HomeController> logger)
        {
            _logger = logger;
            _NorthwindDAL = new NorthwindDAL(northwindCtx, memoryCache, _logger);
        }

        [Route("index")]
        public IActionResult Index()
        {
            var cart = CartDAL.GetCart(HttpContext.Session);
            ViewBag.cart = cart;
            ViewBag.total = cart.Sum(item => item.Product.Price * item.Quantity);
            return View();
        }

        [Route("buy/{id}")]
        public async Task<IActionResult> BuyAsync(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a product ID in the route, for example, /cart/buy/21");
            }

            List<CartItem> cart = CartDAL.GetCart(HttpContext.Session);
            int index = isExist(id.Value);
            if (index != -1)
            {
                cart[index].Quantity++;
            }
            else
            {
                CartProduct cartProduct = await MapNorthwindToCartAsync(id.Value);
                cart.Add(new CartItem { Product = cartProduct, Quantity = 1 });
            }
            CartDAL.SetCart(HttpContext.Session, cart);
 
            return RedirectToAction("Index");
        }

        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            List<CartItem> cart = CartDAL.GetCart(HttpContext.Session);
            int index = isExist(id);
            cart.RemoveAt(index);
            CartDAL.SetCart(HttpContext.Session, cart);
            return RedirectToAction("Index");
        }

        [Route("checkout")]
        public async Task<IActionResult> Checkout()
        {
            // retrieve customer, employee, and shipper options
            /*            
                        var cart = CartDAL.GetCart(HttpContext.Session);
                        for (int i = 0; i < cart.Count; i++)
                        {
                            //retrieve Northwind
                            Product product = await _NorthwindDAL.GetProductAsync(cart[i].Product.Id);
                        }
            */
            //create order
            //Order order = new Order();
            //order.
            //_NorthwindDAL.AddOrder(order);

            CartCheckoutViewModel checkoutViewModel = new CartCheckoutViewModel
            {
                Customers = await _NorthwindDAL.GetCustomersAsync(),
                Employees = await _NorthwindDAL.GetEmployeessAsync(),
                Shippers = await _NorthwindDAL.GetShippersAsync()
            };
            return View(checkoutViewModel);
        }

        private int isExist(int id)
        {
            List<CartItem> cart = CartDAL.GetCart(HttpContext.Session);
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        private async Task<CartProduct> MapNorthwindToCartAsync(int id)
        {
            Product product = await _NorthwindDAL.GetProductAsync(id);
            CartProduct cartProduct = new CartProduct
            {
                Id = product.ProductID,
                Name = product.ProductName,
                Price = product.UnitPrice.Value
            };
            return cartProduct;
        }
    }
}