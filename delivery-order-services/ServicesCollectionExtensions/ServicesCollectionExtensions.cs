using delivery_order_services.Domain.Repositories;
using delivery_order_services.Domain.Repositories.Configuration;
using delivery_order_services.Domain.Repositories.Contracts;
using delivery_order_services.Features.OrderCreatingFeature.UseCase;
using delivery_order_services.Features.UserFeatureEvent.Contracts;
using delivery_order_services.Features.UserFeatureEvent.UseCase;

namespace delivery_order_services.ServicesCollectionExtensions
{
    public static class ServicesCollectionExtensions
    {

        public static IServiceCollection AddAllExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMongoDbExtensions(configuration)
                .AddRepositoriesExtensions(configuration)
                .AddUseCasesExtensions(configuration);

            return services;
        }

        public static IServiceCollection AddMongoDbExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(sp =>
            new MongoDbContext(
               configuration.GetConnectionString("MongoDb")!,
                "MinhaBaseDeDados"
             ));
            
            return services;
        }

        public static IServiceCollection AddRepositoriesExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection AddUseCasesExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOrderEventUseCase, OrderEventUseCase>();
            services.AddScoped<IUserCreatingEventUseCase, UserCreatingEventUseCase>();

            return services;
        }

        public static IServiceCollection AddConsumers(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }

        public static IServiceCollection AddProducers(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
