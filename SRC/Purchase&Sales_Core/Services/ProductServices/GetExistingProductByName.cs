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
    public class GetExistingProductByName(IProductRepo _productRepo) : IGetExistingProductByName
    {
        public Task<Product> GetProductByName(string productName)
        {
           var product= _productRepo.GetProductByName(productName);
            return product;
        }
    }
}
