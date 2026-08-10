using Microsoft.AspNetCore.Http;

namespace Purchase_Sales_Core;

public class PurchaseFileMetadataDTO
{
    public string productNameHeader { get; set; }
    public string priceHeader { get; set; }
    public int headerRow { get; set; }
    public IFormFile purchaseFile { get; set; }
}
