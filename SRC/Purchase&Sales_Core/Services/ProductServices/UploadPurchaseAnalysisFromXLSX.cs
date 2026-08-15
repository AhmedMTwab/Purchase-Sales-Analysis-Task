using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using Purchase_Sales_Core.DTOs.ProductDTO;
using Purchase_Sales_Core.DTOs.SaleDTO;
using Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions;
using Purchase_Sales_Core.ServicesAbstractions.SaleServicesAbstractions;
using Purchase_Sales_Domain.Models;

namespace Purchase_Sales_Core.Services.ProductServices
{
    public class UploadPurchaseAnalysisFromXLSX(IProductAdder _productAdder, IGetAllProducts _getAllProducts, IGetExistingProductByName _getProductByName, IProductUpdater _productUpdater) : IUploadPurchaseAnalysisFromExcel
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
        public Dictionary<string, int> ReadWorksheetHeader(ExcelWorksheet worksheet, int headerRowIndex)
        {
            var headers = new Dictionary<string, int>(
                StringComparer.OrdinalIgnoreCase);

            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
            {
                var header = worksheet.Cells[headerRowIndex, col].Text.Trim();

                if (!string.IsNullOrEmpty(header))
                    headers[header] = col;
            }
            return headers;
        }
        public async Task<List<ProductAddDTO>> ReadPurchase(PurchaseFileMetadataDTO purchaseFileDTO)
        {
            var purchaseList = new List<ProductAddDTO>();

            MemoryStream stream = new MemoryStream();
            await purchaseFileDTO.purchaseFile.CopyToAsync(stream);
            ExcelPackage.License.SetNonCommercialPersonal("Eltwab");
            using (ExcelPackage excelpackage = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = excelpackage.Workbook.Worksheets[0];

                int numberOfRows = worksheet.Dimension.Rows;
                var headers = ReadWorksheetHeader(worksheet, purchaseFileDTO.headerRow);
                for (int row = purchaseFileDTO.headerRow + 1; row <= numberOfRows; row++)
                {
                    ProductAddDTO rowProduct = new ProductAddDTO();
                    string? cellValue = worksheet.Cells[row, 18].Value.ToString();
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        var purchasePrice = worksheet.Cells[row, headers[purchaseFileDTO.priceHeader]].Value;
                        if (!decimal.TryParse(purchasePrice.ToString(), out decimal totalPurchase))
                        {
                            continue;
                        }
                        var productName = worksheet.GetValue<string>(row, headers[purchaseFileDTO.productNameHeader]).Trim();
                        rowProduct.updatedAt = DateTime.Now;
                        purchaseList.Add(new ProductAddDTO
                        {
                            name = productName,
                            purchasePrice = totalPurchase,
                            updatedAt = DateTime.Now
                        });
                    }
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
        // public async Task<int> UploadPurchaseData(IFormFile purchaseFile)
        // {

        // MemoryStream stream = new MemoryStream();
        // await purchaseFile.CopyToAsync(stream);
        // ExcelPackage.License.SetNonCommercialPersonal("Eltwab");
        // using (ExcelPackage excelpackage = new ExcelPackage(stream))
        // {
        //     ExcelWorksheet worksheet = excelpackage.Workbook.Worksheets[0];
        //     int insertedProducts = 0;
        //     int numberOfRows = worksheet.Dimension.Rows;
        //     List<Product> allProducts=await _getAllProducts.GetProductsAsync();
        //     var productsDict = allProducts.ToDictionary(p => p.name, p => p, StringComparer.OrdinalIgnoreCase);
        //     HashSet<string> allProductsNames=allProducts.Select(p=>p.name).ToHashSet();
        //     Dictionary<string,ProductAddDTO> productsToAdd= new Dictionary<string, ProductAddDTO>();
        //     Dictionary<string,Product> productsToUpdate = new Dictionary<string, Product>();
        //     for (int row = 6; row <= numberOfRows; row++)
        //     {
        //         ProductAddDTO rowProduct = new ProductAddDTO();
        //         string? cellValue = worksheet.Cells[row,18].Value.ToString();
        //         if (!string.IsNullOrEmpty(cellValue))
        //         {
        //             var PurchaseCell = worksheet.Cells[row,1].Value;
        //             if (!decimal.TryParse(PurchaseCell.ToString(), out decimal totalPurchase))
        //             {
        //                 continue;
        //             }
        //             rowProduct.name = worksheet.GetValue<string>(row, 12).Trim();
        //             rowProduct.updatedAt = DateTime.Now;
        //             var existedProduct = allProductsNames.Contains(rowProduct.name);
        //             if (!existedProduct)
        //             {
        //                 if (productsToAdd.ContainsKey(rowProduct.name))
        //                 {
        //                     productsToAdd[rowProduct.name].purchasePrice += totalPurchase;
        //                     continue;
        //                 }
        //                 rowProduct.purchasePrice = totalPurchase;
        //                 productsToAdd.Add(rowProduct.name,rowProduct);
        //             }
        //             else
        //             {
        //                 var changedProduct = allProducts.FirstOrDefault(p => p.name == rowProduct.name);
        //                 if (!productsToUpdate.ContainsValue(changedProduct))
        //                 {
        //                     changedProduct.purchasePrice += totalPurchase;
        //                     productsToUpdate.Add(changedProduct.name, changedProduct);
        //                 }
        //                 else
        //                 {
        //                     productsToUpdate[rowProduct.name].purchasePrice += totalPurchase;
        //                     continue;
        //                 }
        //             }
        //             insertedProducts++;
        //         }

        //     }
        //     await _productAdder.AddPulkOfProducts(productsToAdd.Values.ToList());
        //     await _productUpdater.UpdatePulkOfProduct(productsToUpdate.Values.ToList());
        //     return insertedProducts;
        // }
        // }
    }
}
