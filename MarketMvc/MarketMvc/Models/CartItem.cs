using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMvc.Models
{
    public class CartItem
    {
        public CartProduct Product { get; set; }
        public short Quantity { get; set; }
    }
}
