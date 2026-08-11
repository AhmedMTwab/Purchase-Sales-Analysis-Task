using Microsoft.AspNetCore.Http;

namespace Purchase_Sales_Core;

public class SalesFileMetadataDTO
{
    public int headerRow { get; set; }
    public string productNameHeader { get; set; }
    public string quantityHeader { get; set; }
    public string priceHeader { get; set; }
    public IFormFile salesFile { get; set; }

}
