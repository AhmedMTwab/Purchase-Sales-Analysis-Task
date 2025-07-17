using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Purchase_Sales_Core.DTOs.ProductDTO;
using Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions;
using Purchase_Sales_Domain.Models;

namespace Purchase_Sales_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController(IGetTopProductsSales _getTopProductsSales) : ControllerBase
    {
        [HttpGet("/GetTop/{NumberOfProducts:int}")]
        public async Task<ActionResult<List<string>>> GetTopSold(int NumberOfProducts)
        {
            var orderdProducts=await _getTopProductsSales.GetTopSoldProducts(NumberOfProducts);
            if(orderdProducts!=null)
                return Ok(orderdProducts);
            return BadRequest("There is no Products");
        }
    }
}
