using delivery_order_services.ServicesCollectionExtensions;
using Microsoft.AspNetCore.Http.Timeouts;

namespace delivery_order_services
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; set; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen();
            services.InstallServices(Configuration);
            services.AddRequestTimeouts(options =>
            {
                options.DefaultPolicy = new RequestTimeoutPolicy
                {
                    Timeout = TimeSpan.FromSeconds(3)
                };
            });
        }

        public void Configure(IApplicationBuilder app)
        {  
            app
                .UseDefaultLocalization()
                .UseSwagger()
                .UseSwaggerUI()
                .UseHttpsRedirection()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}
