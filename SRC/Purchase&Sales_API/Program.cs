
using Microsoft.AspNetCore.Http.Features;
using Purchase_Sales_Core.CoreDIContainer;
using Purchase_Sales_Infrastructure.InfrastructureDIContainer;
namespace Purchase_Sales_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //AddInfrastructureServices
            builder.Services.AddInfrastructureServices(builder.Configuration);
            //AddCoreServices
            builder.Services.AddCoreServices(builder.Configuration);
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 500 * 1024 * 1024; 
            });
            //builder.Services.Configure<FormOptions>(options =>
            //{
            //    options.ValueLengthLimit = int.MaxValue;
            //    options.MultipartBodyLengthLimit = int.MaxValue;
            //});

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
