using delivery_order_services.Application.Shared.Infra.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace delivery_order_services.Application.Shared.Infra.Repositories.Order
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrderCollection(this IServiceCollection services)
        {
            services.AddScoped(services =>
            {
                var mongoDbConfiguration = services.GetRequiredService<MongoDbConfiguration>();

                var mongoClient = services.GetRequiredService<IMongoClient>();

                var collection = mongoClient.GetDatabase(mongoDbConfiguration.DatabaseName)
                    .GetCollection<Domain.Order>("Order");

                return collection;
            });

            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
