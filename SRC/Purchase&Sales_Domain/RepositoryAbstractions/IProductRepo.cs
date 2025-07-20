using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Purchase_Sales_Domain.Models;


namespace Purchase_Sales_Domain.RepositoryAbstractions
{
    public interface IProductRepo
    {
        public Task<List<Product>> GetAllProducts();
        public  Task<List<string>> GetAllProductsNames();
        public Task<List<Product>> GetListOfProductsByName(string Name);

        public Task<List<string>> GetDeadstockProducts();

        public Task<List<Product>> GetAllProductsWithSales();
        public Task<decimal> GetProductTotalSales(string productName);


        public Task<Product> GetProductByName(string Name);
        public Task<Product> GetProductByNameWithSales(string Name);
        public Task<List<string>> GetTopSoldProductsNames(int noOfProducts);

        public Task<bool> AddPulkOfProducts(List<Product> products);

        public Task<bool> AddProduct(Product product);
        public  Task<bool> UpdatePulkOfProduct(List<Product> products);

        public Task<bool> UpdateProduct(string productId,Product product);
    }
}
