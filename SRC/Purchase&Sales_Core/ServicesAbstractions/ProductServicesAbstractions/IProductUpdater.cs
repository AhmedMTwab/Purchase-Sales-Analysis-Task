using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Purchase_Sales_Core.DTOs.ProductDTO;
using Purchase_Sales_Domain.Models;

namespace Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions
{
    public interface IProductUpdater
    {
        public Task<bool> UpdateProduct(string productId, ProductAddDTO newProductData);
        public Task<bool> UpdatePulkOfProduct(List<Product> newProductsData);

    }
}
