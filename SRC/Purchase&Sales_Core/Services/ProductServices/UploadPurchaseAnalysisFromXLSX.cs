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

namespace Purchase_Sales_Core.Services.ProductServices
{
    public class UploadPurchaseAnalysisFromXLSX (IProductAdder _productAdder,IProductUpdater _productUpdater,IGetExistingProductByName _getExistingProductByName): IUploadPurchaseAnalysisFromExcel
    {
        public async Task<int> UploadPurchaseData(IFormFile purchaseFile)
        {

            MemoryStream stream = new MemoryStream();
            await purchaseFile.CopyToAsync(stream);
            ExcelPackage.License.SetNonCommercialPersonal("Eltwab");
            using (ExcelPackage excelpackage = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = excelpackage.Workbook.Worksheets[0];
                int insertedProducts = 0;
                int numberOfRows = worksheet.Dimension.Rows;
                for (int row = 6; row <= numberOfRows; row++)
                {
                    ProductAddDTO rowProduct = new ProductAddDTO();
                    string? cellValue = worksheet.GetValue(row, 18).ToString();
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        int totalPurchase = worksheet.GetValue<int>(row, 5);
                        int totalQuantity = worksheet.GetValue<int>(row, 11);
                        if (totalQuantity != 0 && totalPurchase !=0)
                            rowProduct.purchasePrice = totalPurchase / totalQuantity;
                        else
                            continue;
                        rowProduct.name = worksheet.GetValue<string>(row, 12);
                        rowProduct.updatedAt = DateTime.Now;
                        var isExist = await _getExistingProductByName.GetProductByName(rowProduct.name);
                        if (isExist == null)
                            await _productAdder.AddProduct(rowProduct);
                        else
                            await _productUpdater.UpdateProduct(rowProduct.name, rowProduct);
                        insertedProducts++;
                    }
                }
                return insertedProducts;
            }
        }
    }
}
