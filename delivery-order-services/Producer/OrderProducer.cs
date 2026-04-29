

using Confluent.Kafka;
using DnsClient.Internal;
using System.Text.Json;

namespace delivery_order_services.Producer
{
    public class OrderProducer : IOrderProducer
    {
        private readonly string _kafkaBootstrapServers;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrderProducer> _logger;

        public OrderProducer(ILogger<OrderProducer> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(OrderEnvelope envelope)
        {
            try
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = "localhost:9092"
                };


                string orderConvertedToJson = JsonSerializer.Serialize(envelope.Value);

                using (var producer = new ProducerBuilder<Null, string>(config).Build())
                {
                    var deliveryReport = await producer.ProduceAsync(
                        envelope.Topic,
                        new Message<Null, string> 
                        { 
                            Value = orderConvertedToJson
                        });

                    _logger.LogInformation("An error occurred in method {MethodName}. Input:{@input}",
                    nameof(HandleAsync),
                    new
                    {
                        envelope.Value
                    });
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
