using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMvc.ViewModels
{
    public class CartReceiptViewModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
