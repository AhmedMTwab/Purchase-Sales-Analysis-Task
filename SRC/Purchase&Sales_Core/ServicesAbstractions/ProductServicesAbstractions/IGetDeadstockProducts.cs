using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions
{
    public interface IGetDeadstockProducts
    {
        public Task<List<string>> GetDeadstockProductsAsync();
    }
}
