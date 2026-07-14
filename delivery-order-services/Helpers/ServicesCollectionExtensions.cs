using delivery_order_services.Application.Shared.Abstractions.Producer;
using delivery_order_services.Application.Shared.Infra.Configuration;
using delivery_order_services.Controllers.Order.UseCase;
using delivery_order_services.Controllers.User.UseCase;
using delivery_order_services.Producer;

namespace delivery_order_services.Helpers
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddAllExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMongoDbConfiguration(configuration)
                .AddUseCasesExtensions(configuration)
                .AddProducers(configuration);

            return services;
        }

        public static IServiceCollection InitializeApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMongoDbConfiguration(configuration);

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
