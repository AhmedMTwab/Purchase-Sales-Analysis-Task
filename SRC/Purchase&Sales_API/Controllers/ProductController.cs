using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Purchase_Sales_Core.DTOs.ProductDTO;
using Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions;

namespace Purchase_Sales_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(ISearchForProductByName _searchForProductByName) : ControllerBase
    {
        [HttpGet("/GetProduct/{productName}")]
        public async Task<ActionResult<List<ProductAddDTO>>> GetProductsByName(string productName)
        {
            var matchedProducts=await _searchForProductByName.SearchForPorductsByName(productName.Trim());
            var products = matchedProducts.Select(p => p.ProductTODTO()).ToList();
            if(products!=null)
                return Ok(products);
            else return BadRequest("No Products with that Name");

        }
    }
}
