using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Purchase_Sales_Core.DTOs.ProductDTO;
using Purchase_Sales_Core.DTOs.SaleDTO;
using Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions;
using Purchase_Sales_Core.ServicesAbstractions.SaleServicesAbstractions;

namespace Purchase_Sales_Core.Services.SaleServices
{
    public class UploadSaleAnalysisFromCsv(ISaleAdder _saleAdder, IGetAllProducts _getAllProducts, IProductAdder _productAdder) : IUploadSaleAnalysisFromCsv
    {
        const int batchSize = 20000;
        public async Task<int> UploadSaleData(IFormFile saleFile)
        {
            int insertedSales = 0;
            var allProducts = await _getAllProducts.GetProductsNamesAsync();
            var allProductNames = new HashSet<string>(allProducts, StringComparer.OrdinalIgnoreCase);
            var addedProducts = new Dictionary<string,ProductAddDTO>();
            var salesToAdd = new List<SaleAddDTO>();

            using (var stream = saleFile.OpenReadStream())
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                DetectDelimiter = true,
                BadDataFound = null,
                MissingFieldFound = null,
            }))
            {
                csv.Read();
                csv.ReadHeader();
                while (await csv.ReadAsync())
                {
                    var productName = csv.GetField<string>("اسم الصنف")?.Trim();
                    if (string.IsNullOrEmpty(productName))
                        continue;

                    if (!allProductNames.Contains(productName) && !addedProducts.ContainsKey(productName))
                    {
                        var newProduct = new ProductAddDTO
                        {
                            name = productName,
                            purchasePrice = 0,
                            updatedAt = DateTime.Now
                        };
                        addedProducts.Add(newProduct.name,newProduct);
                        allProductNames.Add(productName);
                    }

                    var quantity = csv.TryGetField<decimal>("صافى كمية مبيعات", out var q) ? q : 0;
                    var price = csv.TryGetField<decimal>("صافى قيمة مبيعات", out var p) ? p : 0;

                    salesToAdd.Add(new SaleAddDTO
                    {
                        productName = productName,
                        quantity = (int)quantity,
                        price = price
                    });

                    insertedSales++;

                    if (salesToAdd.Count >= batchSize)
                    {
                        if (addedProducts.Any())
                        {
                            await _productAdder.AddPulkOfProducts(addedProducts.Values.ToList());
                            addedProducts.Clear();
                        }
                        await _saleAdder.AddPulkOfSales(salesToAdd);
                        salesToAdd.Clear();
                    }
                }
                if (addedProducts.Any())
                {
                    await _productAdder.AddPulkOfProducts(addedProducts.Values.ToList());
                }
                if (salesToAdd.Any())
                {
                    await _saleAdder.AddPulkOfSales(salesToAdd);
                }
            }
            return insertedSales;
        }
    }
}
