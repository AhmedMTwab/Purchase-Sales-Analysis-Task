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
            var products = await _productRepo.GetAllProductsWithSales();
            Dictionary<string,Decimal> productTotalSales = new Dictionary<string,Decimal>();
            Decimal productTotalIncome = 0;
            foreach (var product in products)
            {
                var ProductIncome = product.sales.Select(p => p.price);
                foreach(var sell in ProductIncome)
                {
                    productTotalIncome += sell;
                }
                productTotalSales.Add(product.name, productTotalIncome);
                productTotalIncome = 0;
            }
            var topSold= productTotalSales.OrderByDescending(p=>p.Value).Take(NoOfProducts).Select(p=>p.Key).ToList();
            
            return topSold;
        }
    }
}
