using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;   //[Authorize]
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

        [Authorize]
        [Route("checkout")]
        public async Task<IActionResult> Checkout()
        {
            CartCheckoutViewModel checkoutViewModel = new CartCheckoutViewModel
            {
                Customers = await _NorthwindDAL.GetCustomersAsync(),
                Employees = await _NorthwindDAL.GetEmployeessAsync(),
                Shippers = await _NorthwindDAL.GetShippersAsync()
            };
            return View(checkoutViewModel);
        }

        [Authorize]
        [Route("receipt")]
        public IActionResult Receipt(CartCheckoutViewModel cartCheckout)
        {
            //int employeeID = cartCheckout.EmployeeID;
            //int shipperID = cartCheckout.ShipperID;
            //string customerID = cartCheckout.CustomerID;

            //create order
            DateTime orderDate = DateTime.Now;
            Order order = new Order();
            var cart = CartDAL.GetCart(HttpContext.Session);

            order.CustomerID = cartCheckout.CustomerID;
            order.EmployeeID = cartCheckout.EmployeeID;
            order.Freight = 10 + (cart.Count * 1);
            order.OrderDate = orderDate;
            order.RequiredDate = DateTime.Now.AddDays(7);
            //order.ShippedDate = DateTime.Now.AddDays(5);// filled out in Admin section
            order.ShipVia = cartCheckout.ShipperID;

            Customer customer = _NorthwindDAL.GetCustomer(cartCheckout.CustomerID);
            order.ShipName = customer.ContactName;// cartCheckout.Name;
            order.ShipAddress = customer.Address;// cartCheckout.Address;
            order.ShipCity = customer.City;// cartCheckout.City;
            order.ShipRegion = customer.Region;// cartCheckout.State;
            order.ShipPostalCode = customer.PostalCode;// cartCheckout.PostalCode;
            order.ShipCountry = customer.Country;

            int orderID = _NorthwindDAL.AddOrder(order).Value;
       
            for (int i = 0; i < cart.Count; i++)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.Discount = 0;
                orderDetail.OrderID = orderID;
                orderDetail.ProductID = cart[i].Product.Id;
                orderDetail.Quantity = cart[i].Quantity;
                orderDetail.UnitPrice = cart[i].Product.Price;
                _NorthwindDAL.AddOrderDetail(orderDetail);
            }

            CartReceiptViewModel receipt = new CartReceiptViewModel
            {
                OrderID = orderID,
                OrderDate = orderDate
            };

            //clear cart
            CartDAL.SetCart(HttpContext.Session, null);

            return View(receipt);
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