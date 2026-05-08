using delivery_order_services.Application.Shared.Abstractions;

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
                _logger.LogInformation("Starting message production. Input:{@input}",
                    new
                    {
                        EnvelopeValue = envelope.Value
                    });

                await _producer.ProduceAsync(envelope, cancellationToken);

                _logger.LogInformation("Kafka message produced. Input:{@input}",
                    new
                    {
                        EnvelopeValue = envelope.Value
                    });

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
