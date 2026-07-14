using delivery_order_services.HealthChecks;
using delivery_order_services.Helpers;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
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
            services.AddHealthChecks()
                .AddCheck<MongoDbHealthCheck>("mongodb", tags: new[] { "ready" });
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
                    endpoints.MapHealthChecks("/health/live");
                    endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
                    {
                        Predicate = healthCheck => healthCheck.Tags.Contains("ready"),
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });
                });
        }
    }
}
