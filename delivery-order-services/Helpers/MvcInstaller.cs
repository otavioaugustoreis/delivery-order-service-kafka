using Microsoft.AspNetCore.Mvc;

namespace delivery_order_services.Helpers
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

            services.AddAllExtensions(configuration);

            return services;
        }
    }
}
