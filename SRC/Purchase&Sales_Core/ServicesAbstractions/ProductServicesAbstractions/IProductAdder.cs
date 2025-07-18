using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Purchase_Sales_Core.DTOs.ProductDTO;

namespace Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions
{
    public interface IProductAdder
    {
        public Task<bool> AddProduct(ProductAddDTO newProduct);
        public Task<bool> AddPulkOfProducts(List<ProductAddDTO> newProducts);
    }
}
