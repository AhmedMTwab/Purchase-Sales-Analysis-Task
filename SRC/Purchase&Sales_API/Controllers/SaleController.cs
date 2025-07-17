using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions;
using Purchase_Sales_Core.ServicesAbstractions.SaleServicesAbstractions;

namespace Purchase_Sales_API.Controllers
{
    [Route("api/upload/[controller]")]
    [ApiController]
    public class SaleController(IUploadSaleAnalysisFromExcel _uploadSaleAnalysisFromExcel) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadSales(IFormFile salesFile)
        {
            if (salesFile == null || salesFile.Length == 0)
            {
                return BadRequest("Sales file is Missed or Empty");
            }
            if (!Path.GetExtension(salesFile.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase)
                && !Path.GetExtension(salesFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Wrong File Format ,File Must be in CSV or xlsx Format");
            }

            await _uploadSaleAnalysisFromExcel.UploadSaleData(salesFile);

            return Created();
        }
    }
}
