using delivery_order_services.ServicesCollectionExtensions;

using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace delivery_order_services
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; set; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen();
            services.InstallServices(Configuration);
        }

        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {  
            app.UseDefaultLocalization();
            
            app.UseSwagger();
            
            app.UseSwaggerUI();
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
