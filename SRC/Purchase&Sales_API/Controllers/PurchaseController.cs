using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions;

namespace Purchase_Sales_API.Controllers
{
    [Route("api/upload/[controller]")]
    [ApiController]
    public class PurchaseController(IUploadPurchaseAnalysisFromExcel _uploadPurchaseAnalysisFromExcel,IUploadPurchaseAnalysisFromCsv _uploadPurchaseAnalysisFromCsv) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadPurchases(IFormFile purchaseFile)
        {
            if (purchaseFile == null || purchaseFile.Length == 0)
            {
                return BadRequest("Purchase file is Missed or Empty");
            }
            int addedProducts = 0;
            
            if(Path.GetExtension(purchaseFile.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                 addedProducts = await _uploadPurchaseAnalysisFromCsv.UploadPurchaseData(purchaseFile);

            else if(Path.GetExtension(purchaseFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                 addedProducts = await _uploadPurchaseAnalysisFromExcel.UploadPurchaseData(purchaseFile);
            else
                return BadRequest("Wrong File Format ,File Must be in CSV or xlsx Format");

            return Ok($"{addedProducts} Products added");
        }
    }
}
