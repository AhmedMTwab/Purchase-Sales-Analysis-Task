using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Purchase_Sales_Core.DTOs.ProductDTO;
using Purchase_Sales_Core.DTOs.SaleDTO;
using Purchase_Sales_Core.Services.ProductServices;
using Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions;
using Purchase_Sales_Core.ServicesAbstractions.SaleServicesAbstractions;
using Purchase_Sales_Domain.Models;

namespace Purchase_Sales_Core.Services.SaleServices
{
    public class UploadSaleAnalysisFromExcel(ISaleAdder _saleAdder, IGetAllProducts _getAllProducts, IProductAdder _productAdder) : IUploadSaleAnalysisFromExcel
    {
        const int batchSize = constants.BatchSize;
        public async Task<int> UploadSaleData(SalesFileMetadataDTO saleFileDTO)
        {
            int insertedSales = 0;
            int totalSalesAdded = 0;
            List<string> allProducts = await _getAllProducts.GetProductsNamesAsync();
            HashSet<string> allProductsNames = new HashSet<string>(
                allProducts,
                StringComparer.OrdinalIgnoreCase
                                                              );
            Dictionary<string, ProductAddDTO> addedProducts = new Dictionary<string, ProductAddDTO>();
            var salesToAdd = await ReadSales(saleFileDTO);
            var batchOfSales = new List<SaleAddDTO>();
            foreach (var sale in salesToAdd)
            {
                AddNewProductToAddList(sale.productName, allProductsNames, addedProducts);
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

        private async Task<List<SaleAddDTO>> ReadSales(SalesFileMetadataDTO saleFileDTO)
        {
            List<SaleAddDTO> salesToAdd = new List<SaleAddDTO>();

            MemoryStream stream = new MemoryStream();
            await saleFileDTO.salesFile.CopyToAsync(stream);
            ExcelPackage.License.SetNonCommercialPersonal("Eltwab");
            using (ExcelPackage excelpackage = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = excelpackage.Workbook.Worksheets[0];
                int numberOfRows = worksheet.Dimension.Rows;

                for (int row = saleFileDTO.headerRow + 1; row <= numberOfRows; row++)
                {
                    SaleAddDTO rowSale = new SaleAddDTO();
                    string? cellValue = worksheet.GetValue(row, 9).ToString();
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        string productName = worksheet.GetValue<string>(row, 11);
                        var trimedName = productName.Trim();


                        rowSale.productName = trimedName;

                        var quantityCell = worksheet.Cells[row, 12].Value;
                        if (!decimal.TryParse(quantityCell.ToString(), out decimal quantity))
                        {
                            continue;
                        }

                        rowSale.quantity = (int)quantity;
                        var priceCell = worksheet.Cells[row, 13].Value;
                        if (!decimal.TryParse(priceCell.ToString(), out decimal price))
                        {
                            continue;
                        }
                        rowSale.price = price;
                        salesToAdd.Add(rowSale);
                    }
                }
            }
            return salesToAdd;
        }
        private void AddNewProductToAddList(string trimedName, HashSet<string>? allProductsNames, Dictionary<string, ProductAddDTO>? addedProducts)
        {
            bool isNewProduct = !allProductsNames.Contains(trimedName) &&
                                !addedProducts.ContainsKey(trimedName);
            if (isNewProduct)
            {
                ProductAddDTO UnExistedProduct = new ProductAddDTO()
                {
                    name = trimedName,
                    purchasePrice = 0,
                    updatedAt = DateTime.Now
                };

                addedProducts.Add(UnExistedProduct.name, UnExistedProduct);
                allProductsNames.Add(trimedName);
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
        private async Task AddSalesToDB(List<SaleAddDTO>? salesToAdd)
        {
            if (salesToAdd.Any())
            {
                await _saleAdder.AddPulkOfSales(salesToAdd);
                salesToAdd.Clear();
            }
        }
    }
}
