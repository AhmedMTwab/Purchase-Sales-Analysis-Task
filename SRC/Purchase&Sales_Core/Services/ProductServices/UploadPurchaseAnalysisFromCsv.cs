using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Purchase_Sales_Core.DTOs.ProductDTO;
using Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions;
using Purchase_Sales_Domain.Models;

namespace Purchase_Sales_Core.Services.ProductServices
{
        public class UploadPurchaseAnalysisFromCsv(IProductAdder _productAdder, IGetAllProducts _getAllProducts) : IUploadPurchaseAnalysisFromCsv
        {
            const int batchSize = 20000;
            public async Task<int> UploadPurchaseData(IFormFile purchaseFile)
            {
                int insertedProducts = 0;

                List<Product> allProducts = await _getAllProducts.GetProductsAsync();
                HashSet<string> allProductsNames = new HashSet<string>(
                    allProducts.Select(p => p.name),
                    StringComparer.OrdinalIgnoreCase
                );

                List<ProductAddDTO> addedProducts = new List<ProductAddDTO>();

                using (var stream = purchaseFile.OpenReadStream())
                using (var reader = new StreamReader(stream))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    DetectDelimiter = true,
                    BadDataFound = null,
                    MissingFieldFound = null,
                    HeaderValidated = null
                }))
                {
                    int headerRow = 6;
                    for (int i = 1; i < headerRow; i++)
                    {
                        await csv.ReadAsync();
                    }
                    await csv.ReadAsync();
                    csv.ReadHeader();

                    while (await csv.ReadAsync())
                    {
                        var productName = csv.GetField<string>("إسم الصنف")?.Trim();
                        if (string.IsNullOrEmpty(productName))
                            continue;

                        decimal purchasePrice = 0;
                        var purchasePriceField = csv.GetField("صافى المشتريات");
                        if (!string.IsNullOrEmpty(purchasePriceField))
                            decimal.TryParse(purchasePriceField, out purchasePrice);

                        if (!allProductsNames.Contains(productName) &&
                            !addedProducts.Any(p => p.name.Equals(productName, StringComparison.OrdinalIgnoreCase)))
                        {
                            var productDto = new ProductAddDTO
                            {
                                name = productName,
                                purchasePrice = purchasePrice,
                                updatedAt = DateTime.Now
                            };
                            addedProducts.Add(productDto);
                            allProductsNames.Add(productName);
                            insertedProducts++;
                        }

                        if (addedProducts.Count >= batchSize)
                        {
                            await _productAdder.AddPulkOfProducts(addedProducts);
                            addedProducts.Clear();
                        }
                    }
                    if (addedProducts.Any())
                    {
                        await _productAdder.AddPulkOfProducts(addedProducts);
                    }
                }
                return insertedProducts;
            }
        }
    
}
