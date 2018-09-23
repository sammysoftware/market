using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using MarketMvc.Controllers;
using MarketMvc.Helpers;
using MarketMvc.Models;

namespace MarketMvc.DAL
{
    public static class CartDAL
    {
        public static List<CartItem> GetCart(ISession session)
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(session, "cart");
            if (cart == null)
                cart = new List<CartItem>();
            return cart;
        }

        public static void SetCart(ISession session, List<CartItem> cart)
        {
            SessionHelper.SetObjectAsJson(session, "cart", cart);
            return;
        }

        public static int GetCartCount(ISession session)
        {
            var cart = GetCart(session);
            return cart.Count;
        }
    }
}
