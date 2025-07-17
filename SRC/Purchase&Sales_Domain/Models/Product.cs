using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase_Sales_Domain.Models
{
    public class Product
    {      
        public string name { get; set; }
        public decimal purchasePrice { get; set; }
        public DateTime updatedAt { get; set; }
        public virtual ICollection<Sale>? sales { get; set; }
    }
}
