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
    public class UploadPurchaseAnalysisFromXLSX (IProductAdder _productAdder,IProductUpdater _productUpdater,IGetAllProducts _getAllProducts): IUploadPurchaseAnalysisFromExcel
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
                List<Product> allProducts=await _getAllProducts.GetProductsAsync();
                HashSet<string> allProductsNames=allProducts.Select(p=>p.name).ToHashSet();
                List<ProductAddDTO> productsToAdd= new List<ProductAddDTO>();
                List<Product> productsToUpdate = new List<Product>();
                for (int row = 6; row <= numberOfRows; row++)
                {
                    ProductAddDTO rowProduct = new ProductAddDTO();
                    string? cellValue = worksheet.Cells[row,18].Value.ToString();
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        var PurchaseCell = worksheet.Cells[row,1].Value;
                        if (!decimal.TryParse(PurchaseCell.ToString(), out decimal totalPurchase))
                        {
                            continue;
                        }
                        rowProduct.name = worksheet.GetValue<string>(row, 12).Trim();
                        rowProduct.updatedAt = DateTime.Now;
                        var existedProduct = allProductsNames.Contains(rowProduct.name);
                        if (!existedProduct)
                        {
                            if (productsToAdd.Select(p => p.name).Contains(rowProduct.name))
                            {
                                productsToAdd.FirstOrDefault(p => p.name == rowProduct.name).purchasePrice += totalPurchase;
                                continue;
                            }
                            rowProduct.purchasePrice = totalPurchase;
                            productsToAdd.Add(rowProduct);
                        }
                        else
                        {
                            var changedProduct = allProducts.FirstOrDefault(p => p.name == rowProduct.name);
                            if (!productsToUpdate.Contains(changedProduct))
                            {
                                changedProduct.purchasePrice += totalPurchase;
                                productsToUpdate.Add(changedProduct);
                            }
                            else
                            {
                                productsToUpdate.FirstOrDefault(p => p.name == rowProduct.name).purchasePrice += totalPurchase;
                                continue;
                            }
                        }
                        insertedProducts++;
                    }
                   
                }
                await _productAdder.AddPulkOfProducts(productsToAdd);
                await _productUpdater.UpdatePulkOfProduct(productsToUpdate);
                return insertedProducts;
            }
        }
    }
}
