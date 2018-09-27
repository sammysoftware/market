﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NorthwindEntitiesLib;

namespace MarketMvc.ViewModels
{
    public class AdminOrderViewModel
    {
        public Order Order { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
