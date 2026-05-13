using delivery_order_services.Application.Shared.Infra.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace delivery_order_services.Application.Shared.Infra.Repositories.User
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserCollection(this IServiceCollection services)
        {
            services.AddScoped(services =>
            {
                var mongoDbConfiguration = services.GetRequiredService<MongoDbConfiguration>();

                var mongoClient = services.GetRequiredService<IMongoClient>();

                var collection = mongoClient.GetDatabase(mongoDbConfiguration.DatabaseName)
                    .GetCollection<Domain.User>("User");

                return collection;
            });

            services.AddScoped<IUserRepository, UserRepository>();
    
            return services;
        }
    }
}
