using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Purchase_Sales_Domain.Models;
using Purchase_Sales_Domain.RepositoryAbstractions;
using Purchase_Sales_Infrastructure.Context;
using Purchase_Sales_Infrastructure.Migrations;

namespace Purchase_Sales_Infrastructure.Repositories
{
    public class ProductRepo(ApplicationDbContext db) : IProductRepo
    {
        
        public async Task<bool> AddProduct(Product product)
        {
            await db.products.AddAsync(product);
            await db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddPulkOfProducts(List<Product> products)
        {
            await db.BulkInsertAsync(products);
            return true;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var products=await db.products.AsNoTracking().ToListAsync();
            return products;
        }
        public async Task<List<string>> GetAllProductsNames()
        {
            var products = await db.products.Select(p=>p.name).AsNoTracking().ToListAsync();
            return products;
        }
        public async Task<List<string>> GetTopSoldProductsNames(int noOfProducts)
        {
            var products = await db.sales.GroupBy(p=>p.productName).Select(g => new
            {
                ProductName = g.Key,
                TotalSales = g.Sum(s => s.price)
            })
            .OrderByDescending(x => x.TotalSales)
            .Take(noOfProducts)
            .Select(x => x.ProductName).AsNoTracking().ToListAsync();
            return products;
        }
        public async Task<List<Product>> GetAllProductsWithSales()
        {
            var products = await db.products.Include(p=>p.sales).AsNoTracking().ToListAsync();
            return products;
        }
        public async Task<decimal> GetProductTotalSales(string productName)
        {

            var products = await db.sales.Where(s => s.productName == productName)
                .SumAsync(s => (decimal?)s.price)??0;
            return products;
        }
        public async Task<List<string>> GetDeadstockProducts()
        {

            var products = await db.products
                            .Where(p => !p.sales.Any())
                            .Select(p => p.name)
                            .ToListAsync();
            return products;
        }


        public async Task<Product> GetProductByName(string Name)
        {
            var product = await db.products.AsNoTracking().FirstOrDefaultAsync(p => p.name == Name);
            return product;
        }

        public async Task<Product> GetProductByNameWithSales(string Name)
        {
            var product = await db.products.Include(p=>p.sales).AsNoTracking().FirstOrDefaultAsync(p => p.name == Name);
            return product;
        }
        public async Task<List<Product>> GetListOfProductsByName(string Name)
        {
            var matchedProducts = await db.products
                                        .Where(p => p.name != null && p.name.Contains(Name))
                                        .ToListAsync();
            return matchedProducts;
        }
        public async Task<bool> UpdateProduct(string productName,Product product)
        {
            var existedProduct = await GetProductByName(productName);
            existedProduct.purchasePrice = product.purchasePrice;
            existedProduct.updatedAt = product.updatedAt;
            await db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdatePulkOfProduct(List<Product> products)
        {
            await db.BulkUpdateAsync(products);
            return true;
        }
    }
}
