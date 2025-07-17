using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase_Sales_Core.DTOs.ProductDTO
{
    public class ProductAddDTO
    {
        public string name { get; set; }
        public decimal purchasePrice { get; set; }
        public DateTime updatedAt { get; set; }

    }
}
