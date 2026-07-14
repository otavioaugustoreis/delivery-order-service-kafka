using delivery_order_services.Application.Domain;
using delivery_order_services.Application.Shared.Abstractions.Consumer;
using delivery_order_services.Application.Shared.Contants;

namespace delivery_order_services.Notify.Consumer
{
    public class OrderCreatedConsumer : BackgroundService
    {
        private readonly IConsumerAbstractions _consumerAbstractions;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(
            IConsumerAbstractions consumerAbstractions,
            ILogger<OrderCreatedConsumer> logger)
        {
            _consumerAbstractions = consumerAbstractions;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _consumerAbstractions.ExecuteAsync<Order>(
                    Topics.OrderTopic,
                    ConsumerGroups.OrderGroupId,
                    HandleOrderAsync,
                    cancellationToken);

            }catch (Exception ex)
            {
                throw;
            }
    }

        private Task HandleOrderAsync(Order order, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Order received: {@Order}", order);
            return Task.CompletedTask;
        }
    }
}
