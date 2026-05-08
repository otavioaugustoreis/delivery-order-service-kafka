using delivery_order_services.Application.Shared.Abstractions.Producer;

namespace delivery_order_services.Producer
{
    public class OrderProducer : IOrderProducer
    {
        private readonly ILogger<OrderProducer> _logger;
        private readonly IProducerAbstractions _producer;

        public OrderProducer(
            ILogger<OrderProducer> logger,
            IProducerAbstractions producer)
        {
            _logger = logger;
            _producer = producer;
        }

        public async Task HandleAsync(OrderEnvelope envelope, CancellationToken cancellationToken)
        {
            try
            {
                await _producer.ProduceAsync(envelope, cancellationToken);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,
                    "An error occurred in Kafka message production.  Method: {MethodName} Input:{@input}",
                    nameof(HandleAsync),
                    new
                    {
                        EnvelopeValue = envelope.Value,
                    });

                throw new(ex.Message);
            }
        }
    }
}
