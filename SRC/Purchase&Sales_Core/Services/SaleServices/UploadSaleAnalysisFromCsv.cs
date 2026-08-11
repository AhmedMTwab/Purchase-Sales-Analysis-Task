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
using Purchase_Sales_Domain.Models;

namespace Purchase_Sales_Core.Services.SaleServices
{
    public class UploadSaleAnalysisFromCsv(ISaleAdder _saleAdder, IGetAllProducts _getAllProducts, IProductAdder _productAdder) : IUploadSaleAnalysisFromCsv
    {
        const int batchSize = constants.BatchSize;
        public async Task<int> UploadSaleData(SalesFileMetadataDTO saleFileDTO)
        {
            int insertedSales = 0;
            int totalSalesAdded = 0;
            var allProducts = await _getAllProducts.GetProductsNamesAsync();
            var allProductNames = new HashSet<string>(allProducts, StringComparer.OrdinalIgnoreCase);
            var addedProducts = new Dictionary<string, ProductAddDTO>();
            var salesToAdd = await ReadSales(saleFileDTO);
            var batchOfSales = new List<SaleAddDTO>();
            foreach (var sale in salesToAdd)
            {
                AddNewProductToAddList(sale.productName, allProductNames, addedProducts);
                batchOfSales.Add(sale);
                totalSalesAdded++;
                insertedSales++;
                if (insertedSales >= batchSize)
                {
                    await AddProductsToDB(addedProducts);
                    await AddSalesToDB(batchOfSales);
                    insertedSales = 0;
                }
            }
            await AddProductsToDB(addedProducts);
            await AddSalesToDB(batchOfSales);

            return totalSalesAdded;
        }
        private async Task AddProductsToDB(Dictionary<string, ProductAddDTO>? addedProducts)
        {
            if (addedProducts.Any())
            {
                await _productAdder.AddPulkOfProducts(addedProducts.Values.ToList());
                addedProducts.Clear();
            }
        }
        private async Task AddSalesToDB(List<SaleAddDTO>? salesToAdd)
        {
            if (salesToAdd.Any())
            {
                await _saleAdder.AddPulkOfSales(salesToAdd);
                salesToAdd.Clear();
            }
        }
        private void AddNewProductToAddList(string productName, HashSet<string>? allProductNames, Dictionary<string, ProductAddDTO>? addedProducts)
        {
            if (!allProductNames.Contains(productName) && !addedProducts.ContainsKey(productName))
            {
                var newProduct = new ProductAddDTO
                {
                    name = productName,
                    purchasePrice = 0,
                    updatedAt = DateTime.Now
                };
                addedProducts.Add(newProduct.name, newProduct);
                allProductNames.Add(productName);
            }
        }
        private async Task<List<SaleAddDTO>> ReadSales(SalesFileMetadataDTO saleFileDTO)
        {
            List<SaleAddDTO> salesToAdd = new List<SaleAddDTO>();
            using (var stream = saleFileDTO.salesFile.OpenReadStream())
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                DetectDelimiter = true,
                BadDataFound = null,
                MissingFieldFound = null,
            }))
            {
                int headerRow = saleFileDTO.headerRow;
                for (int i = 1; i < headerRow; i++)
                {
                    await csv.ReadAsync();
                }
                await csv.ReadAsync();
                csv.ReadHeader();
                while (await csv.ReadAsync())
                {
                    var productName = csv.GetField<string>(saleFileDTO.productNameHeader)?.Trim();
                    if (string.IsNullOrEmpty(productName))
                        continue;
                    var quantity = csv.TryGetField<decimal>(saleFileDTO.quantityHeader, out var q) ? q : 0;
                    var price = csv.TryGetField<decimal>(saleFileDTO.priceHeader, out var p) ? p : 0;
                    salesToAdd.Add(new SaleAddDTO
                    {
                        productName = productName,
                        quantity = (int)quantity,
                        price = price
                    });
                }
            }
            return salesToAdd;
        }
    }
}
