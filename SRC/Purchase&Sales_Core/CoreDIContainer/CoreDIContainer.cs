using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Purchase_Sales_Core.Services.ProductServices;
using Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions;

namespace Purchase_Sales_Core.CoreDIContainer
{
    public static class CoreDIContainer
    {
        public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IGetExistingProductById, GetExistingProductById>();
            services.AddScoped<IProductAdder, ProductAdder>();
            services.AddScoped<IProductUpdater, ProductUpdater>();
            services.AddScoped<IUploadPurchaseAnalysisFromExcel, UploadPurchaseAnalysisFromXLSX>();
        }

    }
}
