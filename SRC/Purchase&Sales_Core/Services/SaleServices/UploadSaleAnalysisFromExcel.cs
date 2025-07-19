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
    public class UploadSaleAnalysisFromExcel(ISaleAdder _saleAdder,IGetAllProducts _getAllProducts,IProductAdder _productAdder) : IUploadSaleAnalysisFromExcel
    {
        const int batchSize = 20000;
        public async Task<int> UploadSaleData(IFormFile saleFile)
        {
            
                MemoryStream stream = new MemoryStream();
                await saleFile.CopyToAsync(stream);
                ExcelPackage.License.SetNonCommercialPersonal("Eltwab");
                using (ExcelPackage excelpackage = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = excelpackage.Workbook.Worksheets[0];
                    int insertedSales = 0;
                    int numberOfRows = worksheet.Dimension.Rows;
                    List<Product> allProducts = await _getAllProducts.GetProductsAsync();
                HashSet<string> allProductsNames = new HashSet<string>(
                        allProducts.Select(p => p.name),
                        StringComparer.OrdinalIgnoreCase
                                                                      );
                List<ProductAddDTO> addedProducts = new List<ProductAddDTO>();
                    List<SaleAddDTO> salesToAdd = new List<SaleAddDTO>();
                    for (int row = 2; row <= numberOfRows; row++)
                    {
                        SaleAddDTO rowSale = new SaleAddDTO();
                        string? cellValue = worksheet.GetValue(row, 9).ToString();
                        if (!string.IsNullOrEmpty(cellValue))
                        {
                        string productName = worksheet.GetValue<string>(row, 11);
                        var trimedName = productName.Trim();
                        bool isNewProduct = !allProductsNames.Contains(trimedName) &&
                                !addedProducts.Any(p => p.name == trimedName);
                        if (isNewProduct)
                            {
                                ProductAddDTO UnExistedProduct = new ProductAddDTO()
                                {
                                    name = trimedName,
                                    purchasePrice = 0,
                                    updatedAt = DateTime.Now
                                };

                                addedProducts.Add(UnExistedProduct);
                                allProductsNames.Add(trimedName);
                        }

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
                            insertedSales++;
                            if (salesToAdd.Count >= batchSize)
                            {
                                if (addedProducts.Any())
                                {
                                    await _productAdder.AddPulkOfProducts(addedProducts);
                                    addedProducts.Clear();
                                }
                                await _saleAdder.AddPulkOfSales(salesToAdd);
                                allProducts=await _getAllProducts.GetProductsAsync();
                                salesToAdd.Clear();
                            }
                        }
                    }
                    if (addedProducts.Any())
                    {
                        await _productAdder.AddPulkOfProducts(addedProducts);
                    }
                    if (salesToAdd.Any())
                    {
                        await _saleAdder.AddPulkOfSales(salesToAdd);
                    }

                    return insertedSales;
                }
            
        }
            
        
    }
}
