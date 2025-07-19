using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Purchase_Sales_Domain.Models;

namespace Purchase_Sales_Core.DTOs.ProductDTO
{
    public class ProductAddDTO
    {
        public string name { get; set; }
        public decimal purchasePrice { get; set; }
        public DateTime updatedAt { get; set; }
    }
    public static class ProductDTOExtentions
    {
        public static ProductAddDTO ProductTODTO(this  Product product)
        {
            return new ProductAddDTO {name = product.name, purchasePrice = product.purchasePrice, updatedAt = product.updatedAt};
        }

    }

}
