using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace delivery_order_services.Application.Shared.Abstractions.Producer
{
    public class ProducerAbstractions : IProducerAbstractions 
    {

        private readonly ILogger<ProducerAbstractions> _logger;
        private readonly ProducerConfiguration _producerConfiguration;

        public ProducerAbstractions(
            ILogger<ProducerAbstractions> logger, 
            IOptions<ProducerConfiguration> producerConfiguration)
        {
            _logger = logger;
            _producerConfiguration = producerConfiguration.Value;
        }

        public async Task ProduceAsync<T>(TEnvelope<T> envelope, CancellationToken cancellationToken)
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
                    BootstrapServers = _producerConfiguration.BootstrapServers
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
                           "Kafka message produced. Input{@input}",
                           new
                           {
                               Topic = deliveryReport.Topic,
                               Partition = deliveryReport.Partition.Value,
                               Offset = deliveryReport.Offset.Value
                           });
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
                //Qual a melhor solucação a se fazer aqui? Devo lançar a exceção ou apenas logar o erro?
            }
        }
    }
}
