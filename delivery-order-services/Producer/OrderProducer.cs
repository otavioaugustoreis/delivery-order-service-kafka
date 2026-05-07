

using Confluent.Kafka;
using System.Text.Json;

namespace delivery_order_services.Producer
{
    public class OrderProducer : IOrderProducer
    {
        private readonly ILogger<OrderProducer> _logger;

        private readonly ProducerConfig _producerConfig;

        public OrderProducer(ILogger<OrderProducer> logger, IConfiguration configuration)
        {
            _logger = logger;
            _producerConfig = configuration.GetSection("ProducerConfig").Get<ProducerConfig>()
                ?? throw new InvalidOperationException("Missing ProducerConfig configuration.");
        }

        public async Task HandleAsync(OrderEnvelope envelope)
        {
            try
            {


                string orderConvertedToJson = JsonSerializer.Serialize(envelope.Value);

                using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
                {
                    var deliveryReport = await producer.ProduceAsync(
                        envelope.Topic,
                        new Message<Null, string> 
                        { 
                            Value = orderConvertedToJson
                        });

                    _logger.LogInformation(
                        "Kafka message produced. Topic: {Topic}; Partition: {Partition}; Offset: {Offset}",
                        deliveryReport.Topic,
                        deliveryReport.Partition.Value,
                        deliveryReport.Offset.Value);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "An error occurred in method {MethodName}. Input:{@input}",
                    nameof(HandleAsync),
                    new
                    {
                        envelope.Value
                    });

                throw;
            }
        }
    }
}
