using delivery_order_services.Application.Shared.Abstractions.Consumer;
using delivery_order_services.Application.Shared.Contants;
using Microsoft.Extensions.Options;

namespace delivery_order_services.Notify.Features
{
    public class OrderCreatedConsumer : BackgroundService
    {
        private readonly ILogger<OrderCreatedConsumer> _logger;
        private readonly ConsumerConfiguration _consumerConfig;
        private readonly IConsumerAbstractions consumerAbstractions;

        public OrderCreatedConsumer(
            ILogger<OrderCreatedConsumer> logger,
            IOptions<ConsumerConfiguration> consumerConfig)
        {
            _logger = logger;
            _consumerConfig = consumerConfig.Value;
        }

        //Testar consumer
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                await consumerAbstractions.ExecuteAsync(Topics.OrderTopic, ConsumerGroups.OrderGroupId, cancellationToken);

            }catch (Exception ex)
            {
                throw;
            }
    }
}}
