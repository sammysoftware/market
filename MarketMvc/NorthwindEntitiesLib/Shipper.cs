using System;
using System.Collections.Generic;
using System.Text;

namespace NorthwindEntitiesLib
{
    public class Shipper
    {
        public int ShipperID { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
//        public ICollection<Order> Orders { get; set; }
    }
}
