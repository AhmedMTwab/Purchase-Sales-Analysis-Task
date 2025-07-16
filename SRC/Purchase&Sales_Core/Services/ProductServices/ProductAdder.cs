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
        public Task<bool> AddProduct(ProductAddDTO newProduct)
        {
            Product product=new Product() {productId=newProduct.id,name=newProduct.name,purchasePrice=newProduct.purchasePrice,updatedAt=newProduct.updatedAt};

            var added = _productRepo.AddProduct(product);
            return added;

        }
    }
}
