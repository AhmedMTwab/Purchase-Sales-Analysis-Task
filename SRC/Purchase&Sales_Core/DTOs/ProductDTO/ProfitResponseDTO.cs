using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase_Sales_Core.DTOs.ProductDTO
{
    public class ProfitResponseDTO
    {
        public string productName {  get; set; }
        public decimal purchasePrice { get; set; }
        public decimal sellPrice { get; set; }
        public decimal profit {  get; set; }
    }
}
