
using Microsoft.AspNetCore.Mvc;

namespace delivery_order_services.ServicesCollectionExtensions
{
    public static class MvcInstaller
    {
        public static IServiceCollection InstallServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvcCore(setupAction => 
            {
                setupAction.Filters.Add<ApiGlobalExceptionFilterAttribute>();
                setupAction.RespectBrowserAcceptHeader = true;                  
            })
            .AddApiExplorer();
            
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddControllers();

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddAllExtensions(configuration);

            return services;
        }
    }
}
