using delivery_order_services.Controllers.Order.UseCase;
using delivery_order_services.Controllers.User.UseCase;
using delivery_order_services.Application.Repositories;
using delivery_order_services.Application.Repositories.Configuration;
using delivery_order_services.Application.Repositories.Contracts;
using delivery_order_services.Producer;
using delivery_order_services.Application.Shared.Abstractions.Producer;

namespace delivery_order_services.ServicesCollectionExtensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddAllExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMongoDbExtensions(configuration)
                .AddRepositoriesExtensions(configuration)
                .AddUseCasesExtensions(configuration)
                .AddProducers(configuration);

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
            services.AddScoped<IUserUseCase, UserUseCase>();

            return services;
        }

        public static IServiceCollection AddProducers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProducerAbstractions, ProducerAbstractions>();
            services.AddScoped<IOrderProducer, OrderProducer>();

            services.Configure<ProducerConfiguration>(
                configuration.GetSection(nameof(ProducerConfiguration)));

            return services;
        }
    }
}
