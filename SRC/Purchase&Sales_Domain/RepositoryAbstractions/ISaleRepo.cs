using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Purchase_Sales_Domain.Models;

namespace Purchase_Sales_Domain.RepositoryAbstractions
{
    public interface ISaleRepo
    {
        public Task<bool> AddSale(Sale newSale);
    }
}
