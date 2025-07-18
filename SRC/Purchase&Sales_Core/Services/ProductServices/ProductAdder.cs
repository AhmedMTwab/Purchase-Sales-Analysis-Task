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
    public class ProductAdder(IProductRepo _productRepo) : IProductAdder
    {
        public async Task<bool> AddProduct(ProductAddDTO newProduct)
        {
            Product product=new Product() {name=newProduct.name,purchasePrice=newProduct.purchasePrice,updatedAt=newProduct.updatedAt};

            var added =await _productRepo.AddProduct(product);
            return added;

        }

        public async Task<bool> AddPulkOfProducts(List<ProductAddDTO> newProducts)
        {
            List<Product> productsToAdd=new List<Product>();
            foreach (var product in newProducts)
            {
                Product rowProduct = new Product() { name = product.name, purchasePrice = product.purchasePrice, updatedAt = product.updatedAt };
                productsToAdd.Add(rowProduct);
            }
            var added =await _productRepo.AddPulkOfProducts(productsToAdd);
            return added;
        }
    }
}
