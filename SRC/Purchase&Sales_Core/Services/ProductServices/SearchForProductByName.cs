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
            var matchedProducts = await _productRepo.GetListOfProductsByName(productName);
            return matchedProducts;
        }
    }
}
