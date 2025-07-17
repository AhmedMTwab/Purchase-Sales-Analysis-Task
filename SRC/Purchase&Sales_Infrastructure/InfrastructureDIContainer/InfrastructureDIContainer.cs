using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Purchase_Sales_Domain.RepositoryAbstractions;
using Purchase_Sales_Infrastructure.Context;
using Purchase_Sales_Infrastructure.Repositories;

namespace Purchase_Sales_Infrastructure.InfrastructureDIContainer
{
    public static class InfrastructureDIContainer
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(configuration.GetConnectionString("ApplicationDb"));
                options.EnableSensitiveDataLogging();
            });
            //Register Infrastructure services
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<ISaleRepo,SaleRepo>();
        }

    }
}
