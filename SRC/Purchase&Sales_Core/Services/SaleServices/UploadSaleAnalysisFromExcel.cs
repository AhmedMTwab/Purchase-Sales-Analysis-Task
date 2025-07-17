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
                List<ProductAddDTO> addedProducts= new List<ProductAddDTO>();
                for (int row = 2; row <= numberOfRows; row++)
                {
                    SaleAddDTO rowSale = new SaleAddDTO();
                    string? cellValue = worksheet.GetValue(row, 9).ToString();
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        string productName= worksheet.GetValue<string>(row, 11);
                        //var existedProduct=await _getExistingProductByName.GetProductByName(productName);
                        var existedProduct = allProducts.FirstOrDefault(p => p.name == productName);
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
                            if (addedProducts.FirstOrDefault(p=>p.name == UnExistedProduct.name)==null)
                            {
                                await _productAdder.AddProduct(UnExistedProduct);
                                addedProducts.Add(UnExistedProduct);
                            }
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
