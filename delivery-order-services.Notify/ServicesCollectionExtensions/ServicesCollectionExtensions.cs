using delivery_order_services.Application.Shared.Abstractions.Consumer;

namespace delivery_order_services.Notify.ServicesCollectionExtensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddWorkerConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConsumerConfiguration>(
                configuration.GetSection(nameof(ConsumerConfiguration)));

            return services;
        }
    }
}
