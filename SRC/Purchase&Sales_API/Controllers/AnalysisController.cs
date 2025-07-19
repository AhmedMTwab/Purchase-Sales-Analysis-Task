using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Purchase_Sales_Core.DTOs.ProductDTO;
using Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions;
using Purchase_Sales_Domain.Models;

namespace Purchase_Sales_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController(IGetTopProductsSales _getTopProductsSales,IGetDeadstockProducts _getDeadstockProducts,IGetProductProfit _getProductProfit) : ControllerBase
    {
        [HttpGet("/GetTopSales/{NumberOfProducts:int}")]
        public async Task<ActionResult<List<string>>> GetTopSold(int NumberOfProducts)
        {
            var orderdProducts=await _getTopProductsSales.GetTopSoldProducts(NumberOfProducts);
            if(orderdProducts!=null)
                return Ok(orderdProducts);
            return BadRequest("There is no Products");
        }
        [HttpGet("/GetDeadstock")]
        public async Task<ActionResult<List<string>>> GetDeadstock()
        {
            var deadstockProducts=await _getDeadstockProducts.GetDeadstockProductsAsync();
            if (deadstockProducts != null)
                return Ok(deadstockProducts);
            return BadRequest("There is no Deadstock Products");
        }
        [HttpGet("/GetProfit/{productName}")]
        public async Task<ActionResult<ProfitResponseDTO>> GetProductProfit(string productName)
        {
            var productProfit = await _getProductProfit.GetProductProfitAsync(productName);
            if (productProfit != null)
                return Ok(productProfit);
            return BadRequest("There is no Product Data");
        }

    }
}
