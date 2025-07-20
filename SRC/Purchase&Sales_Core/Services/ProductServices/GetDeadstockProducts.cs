using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions;
using Purchase_Sales_Domain.RepositoryAbstractions;

namespace Purchase_Sales_Core.Services.ProductServices
{
    public class GetDeadstockProducts(IProductRepo _productRepo) : IGetDeadstockProducts
    {
        public async Task<List<string>> GetDeadstockProductsAsync()
        {
          return await _productRepo.GetDeadstockProducts();
        }
    }
}
