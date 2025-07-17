using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Purchase_Sales_Core.Services.ProductServices;
using Purchase_Sales_Core.Services.SaleServices;
using Purchase_Sales_Core.ServicesAbstractions.ProductServicesAbstractions;
using Purchase_Sales_Core.ServicesAbstractions.SaleServicesAbstractions;

namespace Purchase_Sales_Core.CoreDIContainer
{
    public static class CoreDIContainer
    {
        public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IGetExistingProductById, GetExistingProductById>();
            services.AddScoped<IGetExistingProductByName, GetExistingProductByName>();
            services.AddScoped<IGetAllProducts, GetAllProducts>();
            services.AddScoped<IProductAdder, ProductAdder>();
            services.AddScoped<IProductUpdater, ProductUpdater>();
            services.AddScoped<IUploadPurchaseAnalysisFromExcel, UploadPurchaseAnalysisFromXLSX>();
            services.AddScoped<IUploadSaleAnalysisFromExcel, UploadSaleAnalysisFromExcel>();
            services.AddScoped<ISaleAdder, SaleAdder>();
        }

    }
}
