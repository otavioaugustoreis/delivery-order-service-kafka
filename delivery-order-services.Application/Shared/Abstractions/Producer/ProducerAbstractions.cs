using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace delivery_order_services.Application.Shared.Abstractions.Producer
{
    public class ProducerAbstractions : IProducerAbstractions
    {

        private readonly ILogger<ProducerAbstractions> _logger;

        public ProducerAbstractions(ILogger<ProducerAbstractions> logger)
        {
            _logger = logger;
        }

        public async Task ProduceAsync(TEnvelope envelope, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Starting message production. Input:{@input}",
                   new
                   {
                       EnvelopeValue = envelope.Value
                   });

            try
            {
                var producerConfig = new ProducerConfig
                {
                    BootstrapServers = "localhost:9092"
                };

                string orderConvertedToJson = JsonSerializer.Serialize(envelope.Value);

                using (var producer = new ProducerBuilder<Null, string>(producerConfig).Build())
                {
                    var deliveryReport = await producer.ProduceAsync(
                        envelope.Topic,
                        new Message<Null, string>
                        {
                            Value = orderConvertedToJson
                        }, cancellationToken);

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
                    "An error occurred in Kafka message production. Input:{@input}",
                    new
                    {
                        EnvelopeValue = envelope.Value,
                    });

                throw new ();
            }
        }
    }
}
