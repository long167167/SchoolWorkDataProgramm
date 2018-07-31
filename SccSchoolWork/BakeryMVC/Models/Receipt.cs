using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bakery.Models
{
    public class Receipt
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Unitprice { get; set; }
        public decimal Discount { get; set; }
        public decimal SaleTax { get; set; }
        public decimal Total { get; set; }
        public int EmployeeKey { get; set; }

    }
}