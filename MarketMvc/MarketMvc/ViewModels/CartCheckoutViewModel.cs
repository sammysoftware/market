using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NorthwindEntitiesLib;

namespace MarketMvc.ViewModels
{
    public class CartCheckoutViewModel
    {
        public string CustomerID { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
//        public Employee Employee { get; set; }
//        public IEnumerable<Employee> Employees { get; set; }
//        public int ShipperID { get; set; }
//        public IEnumerable<Shipper> Shippers { get; set; }
    }
}
