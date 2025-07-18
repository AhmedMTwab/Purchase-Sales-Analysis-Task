using System;
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
                HashSet<string> allProductsNames = allProducts.Select(p=>p.name).ToHashSet();
                List<ProductAddDTO> addedProducts= new List<ProductAddDTO>();
                List<SaleAddDTO> salesToAdd= new List<SaleAddDTO>();
                for (int row = 2; row <= numberOfRows; row++)
                {
                    SaleAddDTO rowSale = new SaleAddDTO();
                    string? cellValue = worksheet.GetValue(row, 9).ToString();
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        string productName= worksheet.GetValue<string>(row, 11);
                        var existedProduct = allProductsNames.Contains(productName);
                        if (existedProduct)
                        {
                            rowSale.productName = productName; 
                        }
                        else
                        {
                            ProductAddDTO UnExistedProduct = new ProductAddDTO()
                            {
                                name = productName,
                                purchasePrice = 0,
                                updatedAt= DateTime.Now
                            };
                            if (!addedProducts.Select(p=>p.name).ToHashSet().Contains(UnExistedProduct.name))
                            {
                                addedProducts.Add(UnExistedProduct);
                            }
                            rowSale.productName = UnExistedProduct.name;
                        }
                         var quantityCell = worksheet.Cells[row,12].Value;
                        if (!decimal.TryParse(quantityCell.ToString(), out decimal quantity))
                        {
                            continue;
                        }

                        rowSale.quantity = (int)quantity;
                        var priceCell= worksheet.Cells[row,13].Value;
                        if (!decimal.TryParse(priceCell.ToString(), out decimal price))
                        {
                            continue;
                        }
                        rowSale.price = price;
                        salesToAdd.Add(rowSale);
                        insertedSales++;
                    }
                }
                await _productAdder.AddPulkOfProducts(addedProducts);
                await _saleAdder.AddPulkOfSales(salesToAdd);
                return insertedSales;
            }
        }
            
        
    }
}
