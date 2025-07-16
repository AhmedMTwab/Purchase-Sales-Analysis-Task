using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions
{
    public interface IUploadPurchaseAnalysisFromExcel
    {
        public Task<int> UploadPurchaseData(IFormFile purchaseFile); 
    }
}
