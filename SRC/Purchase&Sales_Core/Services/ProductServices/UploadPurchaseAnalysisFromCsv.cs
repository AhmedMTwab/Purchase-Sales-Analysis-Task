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
    public class UploadPurchaseAnalysisFromCsv(IProductAdder _productAdder, IGetAllProducts _getAllProducts, IGetExistingProductByName _getProductByName, IProductUpdater _productUpdater) : IUploadPurchaseAnalysisFromCsv
    {
        const int batchSize = constants.BatchSize;
        public async Task<int> UploadPurchaseData(PurchaseFileMetadataDTO purchaseFileDTO)
        {

            int insertedProducts = 0;

            List<string> allProducts = await _getAllProducts.GetProductsNamesAsync();
            HashSet<string> allProductsNames = new HashSet<string>(
                    allProducts,
                    StringComparer.OrdinalIgnoreCase
                );

            Dictionary<string, ProductAddDTO> addedProducts = new Dictionary<string, ProductAddDTO>();
            Dictionary<string, Product> productsToUpdate = new Dictionary<string, Product>();

            var purchaseList = await ReadPurchase(purchaseFileDTO);
            foreach (var purchase in purchaseList)
            {
                await AddUpdatedProductToUpdateList(purchase, allProductsNames, productsToUpdate);
                AddNewProductToAddList(purchase, allProductsNames, addedProducts, ref insertedProducts);

                if (addedProducts.Count >= batchSize)
                {
                    await AddProductsToDB(addedProducts);
                    await UpdateProductsInDB(productsToUpdate);
                }
            }
            await AddProductsToDB(addedProducts);
            await UpdateProductsInDB(productsToUpdate);

            return insertedProducts;

        }

        public async Task<List<ProductAddDTO>> ReadPurchase(PurchaseFileMetadataDTO purchaseFileDTO)
        {
            var purchaseList = new List<ProductAddDTO>();

            using (var stream = purchaseFileDTO.purchaseFile.OpenReadStream())
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                DetectDelimiter = true,
                BadDataFound = null,
                MissingFieldFound = null,
            }))
            {
                int headerRow = purchaseFileDTO.headerRow;
                for (int i = 1; i < headerRow; i++)
                {
                    await csv.ReadAsync();
                }
                await csv.ReadAsync();
                csv.ReadHeader();

                while (await csv.ReadAsync())
                {
                    var productName = csv.GetField<string>(purchaseFileDTO.productNameHeader)?.Trim();
                    if (string.IsNullOrEmpty(productName))
                        continue;

                    decimal purchasePrice = 0;
                    var purchasePriceField = csv.GetField(purchaseFileDTO.priceHeader);
                    if (!string.IsNullOrEmpty(purchasePriceField))
                        decimal.TryParse(purchasePriceField, out purchasePrice);

                    purchaseList.Add(new ProductAddDTO
                    {
                        name = productName,
                        purchasePrice = purchasePrice,
                        updatedAt = DateTime.Now
                    });
                }
            }

            return purchaseList;
        }
        private void AddNewProductToAddList(ProductAddDTO product, HashSet<string>? allProductNames, Dictionary<string, ProductAddDTO>? addedProducts, ref int insertedProducts)
        {
            if (!allProductNames.Contains(product.name))
            {
                if (!addedProducts.ContainsKey(product.name))
                {
                    var newProduct = new ProductAddDTO
                    {
                        name = product.name,
                        purchasePrice = product.purchasePrice,
                        updatedAt = DateTime.Now
                    };
                    addedProducts.Add(newProduct.name, newProduct);
                    insertedProducts++;
                }
                else
                {
                    addedProducts[product.name].purchasePrice += product.purchasePrice;
                }
            }
        }

        private async Task AddUpdatedProductToUpdateList(ProductAddDTO product, HashSet<string>? allProductNames, Dictionary<string, Product> productsToUpdate)
        {
            if (allProductNames.Contains(product.name))
            {
                if (!productsToUpdate.ContainsKey(product.name))
                {
                    Product existedProduct = await _getProductByName.GetProductByName(product.name);
                    existedProduct.purchasePrice += product.purchasePrice;
                    productsToUpdate.Add(product.name, existedProduct);
                }
                else
                {
                    productsToUpdate[product.name].purchasePrice += product.purchasePrice;
                }
            }
        }
        private async Task AddProductsToDB(Dictionary<string, ProductAddDTO>? addedProducts)
        {
            if (addedProducts.Any())
            {
                await _productAdder.AddPulkOfProducts(addedProducts.Values.ToList());
                addedProducts.Clear();
            }
        }
        private async Task UpdateProductsInDB(Dictionary<string, Product> productsToUpdate)
        {
            if (productsToUpdate.Any())
            {
                await _productUpdater.UpdatePulkOfProduct(productsToUpdate.Values.ToList());
                productsToUpdate.Clear();
            }
        }
    }
}
