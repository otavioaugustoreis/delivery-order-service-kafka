using delivery_order_services.Domain.Repositories;
using delivery_order_services.Domain.Repositories.Configuration;
using delivery_order_services.Domain.Repositories.Contracts;
using delivery_order_services.Features.OrderCreatingFeature.UseCase;
using delivery_order_services.Features.OrderFeatureEvent.Contracts;
using delivery_order_services.Features.UserFeatureEvent.Contracts;
using delivery_order_services.Features.UserFeatureEvent.UseCase;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace delivery_order_services.ServicesCollectionExtensions
{
    public static class ServicesCollectionExtensions
    {

        public static IServiceCollection AddAllExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMongoDbExtensions(configuration)
                .AddRepositoriesExtensions(configuration)
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
            services.TryAddScoped<IOrderRepository, OrderRepository>();
            services.TryAddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection AddUseCasesExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddScoped<IOrderEventUseCase, OrderEventUseCase>();
            services.TryAddScoped<IUserCreatingEventUseCase, UserCreatingEventUseCase>();

            return services;
        }

    }
}
