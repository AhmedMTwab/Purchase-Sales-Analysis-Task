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
    public class SearchForProductByName(IProductRepo _productRepo) : ISearchForProductByName
    {
        public async Task<List<Product>> SearchForPorductsByName(string productName)
        {
            var allProducts=await _productRepo.GetAllProducts();
            var matchedProducts = allProducts.Where(p => (p.name != null) ? p.name.Contains(productName, StringComparison.OrdinalIgnoreCase) : false).ToList();
            return matchedProducts;
        }
    }
}
