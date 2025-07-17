using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Purchase_Sales_Core.ServicesAbstractions.SaleServicesAbstractions
{
    public interface IUploadSaleAnalysisFromExcel
    {
        public Task<int> UploadSaleData(IFormFile saleFile);

    }
}
