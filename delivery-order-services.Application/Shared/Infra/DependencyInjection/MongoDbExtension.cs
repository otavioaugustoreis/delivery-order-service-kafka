using delivery_order_services.Application.Shared.Infra.Configuration;
using delivery_order_services.Application.Shared.Infra.Repositories.Order;
using delivery_order_services.Application.Shared.Infra.Repositories.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace delivery_order_services.Application.Shared.Infra.DependencyInjection
{
    public static class MongoDbExtension
    {
        public static IServiceCollection AddMongoDbConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbConfiguration>(options =>
                configuration.GetSection(nameof(MongoDbConfiguration)).Bind(options));

            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IOptions<MongoDbConfiguration>>().Value;

                var clientSettings = MongoClientSettings.FromUrl(new (configuration.ConnectionString));

                return new MongoClient(clientSettings);
            });

            services.AddHostedService<OrderIndexesHostedService>();

            services.AddUserCollection();
            services.AddOrderCollection();

            return services;
        }
    }
}
