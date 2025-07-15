using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase_Sales_Domain.Models
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public decimal purchasePrice { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual ICollection<Sale> sales { get; set; }
    }
}
