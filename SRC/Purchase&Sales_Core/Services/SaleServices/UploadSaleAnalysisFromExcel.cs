using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Purchase_Sales_Core.DTOs.ProductDTO;
using Purchase_Sales_Core.DTOs.SaleDTO;
using Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions;
using Purchase_Sales_Core.ServicesAbstractions.SaleServicesAbstractions;

namespace Purchase_Sales_Core.Services.SaleServices
{
    public class UploadSaleAnalysisFromExcel(ISaleAdder _saleAdder,IGetExistingProductByName _getExistingProductByName,IProductAdder _productAdder) : IUploadSaleAnalysisFromExcel
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
                for (int row = 2; row <= numberOfRows; row++)
                {
                    SaleAddDTO rowSale = new SaleAddDTO();
                    string? cellValue = worksheet.GetValue(row, 9).ToString();
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        string productName= worksheet.GetValue<string>(row, 11);
                        var existedProduct=await _getExistingProductByName.GetProductByName(productName);
                        if (existedProduct != null)
                        {
                            rowSale.productName = existedProduct.name; 
                        }
                        else
                        {
                            ProductAddDTO UnExistedProduct = new ProductAddDTO()
                            {
                                name = productName,
                                purchasePrice = 0,
                                updatedAt= DateTime.Now
                            };
                            await _productAdder.AddProduct(UnExistedProduct);
                            rowSale.productName = UnExistedProduct.name;
                        }
                        rowSale.quantity = worksheet.GetValue<int>(row, 12);
                        rowSale.price = worksheet.GetValue<decimal>(row, 13);
                        await _saleAdder.AddSale(rowSale);
                        insertedSales++;
                    }
                }
                return insertedSales;
            }
        }
            
        
    }
}
