using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase_Sales_Domain.Models
{
    public class Sale
    {
        public int id { get; set; }
        public string productId { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public DateTime saleDate { get; set; }
        [ForeignKey("productId")]
        public virtual Product Product { get; set; }
    }
}
