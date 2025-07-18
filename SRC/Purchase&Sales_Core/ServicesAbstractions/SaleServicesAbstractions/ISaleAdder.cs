using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Purchase_Sales_Core.DTOs.ProductDTO;
using Purchase_Sales_Core.DTOs.SaleDTO;

namespace Purchase_Sales_Core.ServicesAbstractions.SaleServicesAbstractions
{
    public interface ISaleAdder
    {
        public Task<bool> AddSale(SaleAddDTO newSale);
        public Task<bool> AddPulkOfSales(List<SaleAddDTO> newSales);


    }
}
