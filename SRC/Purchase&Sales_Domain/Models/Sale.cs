﻿using System;
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
        public string productName { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        [ForeignKey("productName")]
        public virtual Product Product { get; set; }
    }
}
