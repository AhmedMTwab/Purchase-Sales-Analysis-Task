using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions;
using Purchase_Sales_Domain.Models;
using Purchase_Sales_Domain.RepositoryAbstractions;

namespace Purchase_Sales_Core.Services.ProductServices
{
    public class GetAllProducts(IProductRepo _productRepo) : IGetAllProducts
    {
        public async Task<List<Product>> GetProductsAsync()
        {
            var products=await _productRepo.GetAllProducts();
            return products;
        }
    }
}
