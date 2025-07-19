using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions;
using Purchase_Sales_Core.ServicesAbstractions.SaleServicesAbstractions;

namespace Purchase_Sales_API.Controllers
{
    [Route("api/upload/[controller]")]
    [ApiController]
    public class SaleController(IUploadSaleAnalysisFromExcel _uploadSaleAnalysisFromExcel,IUploadSaleAnalysisFromCsv _uploadSaleAnalysisFromCsv) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadSales(IFormFile salesFile)
        {
            if (salesFile == null || salesFile.Length == 0)
            {
                return BadRequest("Sales file is Missed or Empty");
            }
            int addedSales = 0;
           
            if(Path.GetExtension(salesFile.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                 addedSales = await _uploadSaleAnalysisFromCsv.UploadSaleData(salesFile);
            else if(Path.GetExtension(salesFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                 addedSales = await _uploadSaleAnalysisFromExcel.UploadSaleData(salesFile);
            else
                return BadRequest("Wrong File Format ,File Must be in CSV or xlsx Format");


            return Ok($"{addedSales} Sales added");
        }
    }
}
