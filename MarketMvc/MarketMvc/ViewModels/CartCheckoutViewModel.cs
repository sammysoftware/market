using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NorthwindEntitiesLib;

namespace MarketMvc.ViewModels
{
    public class CartCheckoutViewModel
    {
        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<Shipper> Shippers { get; set; }

        public string CustomerID { get; set; }
        public int? EmployeeID { get; set; }
        public int? ShipperID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }
}
