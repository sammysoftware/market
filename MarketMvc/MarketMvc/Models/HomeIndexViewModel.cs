using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NorthwindEntitiesLib;

namespace MarketMvc.Models
{
    public class HomeIndexViewModel
    {
        public int VisitorCount;
        public IList<Category> Categories { get; set; }
        public IList<Product> Products { get; set; }
    }
}
