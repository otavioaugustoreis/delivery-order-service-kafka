using Confluent.Kafka;
using delivery_order_services.Notify.Features;
using System.Runtime.CompilerServices;

namespace delivery_order_services.Notify.ServicesCollectionExtensions
{
    public static class ServicesCollectionExtensions
    {
        
        public static IServiceCollection AddWorkerConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConsumerConfig>(
                configuration.GetSection(nameof(ConsumerConfig)));

            return services;
        }
    }
}
