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
    public class GetExistingProductById(IProductRepo _productRepo) : IGetExistingProductById
    {
       public async Task<Product> GetProductById(string Id)
        {
            var foundProduct=await _productRepo.GetProductById(Id);
            return foundProduct;
        }
    }
}
