using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase_Sales_Core.DTOs.SaleDTO
{
    public class SaleAddDTO
    {
        public int id { get; set; }
        public string productName { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
    }
}
