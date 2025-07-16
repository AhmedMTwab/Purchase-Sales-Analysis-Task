using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Purchase_Sales_Domain.Models;
using Purchase_Sales_Domain.RepositoryAbstractions;
using Purchase_Sales_Infrastructure.Context;

namespace Purchase_Sales_Infrastructure.Repositories
{
    public class ProductRepo(ApplicationDbContext db) : IProductRepo
    {
        
        public async Task<bool> AddProduct(Product product)
        {
            await db.AddAsync(product);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var products=await db.products.ToListAsync();
            return products;
        }

        public async Task<Product> GetProductById(string id)
        {
            var product =await  db.products.FirstOrDefaultAsync(p=>p.productId==id);
            return product;
        }

        public async Task<bool> UpdateProduct(string productId,Product product)
        {
            var existedProduct = await GetProductById(productId);
            existedProduct.purchasePrice = product.purchasePrice;
            existedProduct.updatedAt = product.updatedAt;
            await db.SaveChangesAsync();
            return true;
        }
    }
}
