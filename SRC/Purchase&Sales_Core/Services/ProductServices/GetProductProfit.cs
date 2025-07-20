using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Purchase_Sales_Core.DTOs.ProductDTO;
using Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions;
using Purchase_Sales_Domain.RepositoryAbstractions;

namespace Purchase_Sales_Core.Services.ProductServices
{
    public class GetProductProfit(IProductRepo _productRepo) : IGetProductProfit
    {
       public async Task<ProfitResponseDTO> GetProductProfitAsync(string productName)
        {
           var product = await _productRepo.GetProductByName(productName);
           var purchasePrice=product.purchasePrice;
            var totalSellPrice =await _productRepo.GetProductTotalSales(productName);
            var totalProfit = totalSellPrice - purchasePrice;
            return new ProfitResponseDTO {productName=productName, purchasePrice=purchasePrice,sellPrice=totalSellPrice,profit=totalProfit};

        }

        
    }
}
