using delivery_order_services.Application.Shared.Abstractions.Consumer;
using delivery_order_services.Application.Shared.Contants;

namespace delivery_order_services.Notify.Features
{
    public class OrderCreatedConsumer : BackgroundService
    {
        private readonly IConsumerAbstractions _consumerAbstractions;

        public OrderCreatedConsumer(
            IConsumerAbstractions consumerAbstractions)
        {
            _consumerAbstractions = consumerAbstractions;
        }   

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _consumerAbstractions.ExecuteAsync(Topics.OrderTopic, ConsumerGroups.OrderGroupId, cancellationToken);

            }catch (Exception ex)
            {
                throw;
            }
    }
}}
