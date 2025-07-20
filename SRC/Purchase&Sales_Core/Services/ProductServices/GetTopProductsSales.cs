using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Purchase_Sales_Core.DTOs.ProductDTO;
using Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions;
using Purchase_Sales_Domain.Models;
using Purchase_Sales_Domain.RepositoryAbstractions;

namespace Purchase_Sales_Core.Services.ProductServices
{
    public class GetTopProductsSales(IProductRepo _productRepo) : IGetTopProductsSales
    {
        public async Task<List<string>> GetTopSoldProducts(int NoOfProducts)
        {
            var topSold =await _productRepo.GetTopSoldProductsNames(NoOfProducts);
            return topSold;
        }
    }
}
