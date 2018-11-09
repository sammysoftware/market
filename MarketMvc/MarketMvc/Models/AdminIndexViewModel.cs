using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketMvc.Entities;

namespace MarketMvc.Models
{
    public class AdminIndexViewModel
    {
        public IList<Order> NewOrders { get; set; }
    }
}
