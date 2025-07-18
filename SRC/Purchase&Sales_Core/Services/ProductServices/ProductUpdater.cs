using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Purchase_Sales_Core.DTOs.ProductDTO;
using Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions;
using Purchase_Sales_Domain.Models;
using Purchase_Sales_Domain.RepositoryAbstractions;

namespace Purchase_Sales_Core.Services.ProductServices
{
    public class ProductUpdater(IProductRepo _productRepo) : IProductUpdater
    {
        public async Task<bool> UpdateProduct(string productId, ProductAddDTO newProductData)
        {
            Product product=new Product();
            product.name = newProductData.name;
            product.purchasePrice = newProductData.purchasePrice;
            product.updatedAt = newProductData.updatedAt;
            var updated=await _productRepo.UpdateProduct(productId, product);
            return updated;
        }

        public async Task<bool> UpdatePulkOfProduct(List<Product> newProductsData)
        {
            var updated = await _productRepo.UpdatePulkOfProduct(newProductsData);
            return updated;
        }
    }
}
