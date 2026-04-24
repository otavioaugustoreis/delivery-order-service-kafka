

using Confluent.Kafka;
using delivery_order_services.Commons;
using delivery_order_services.Domain.Repositories.Contracts;
using System.Text.Json;

namespace delivery_order_services.Producer
{
    public class OrderProducer : IOrderProducer
    {
        private readonly string _kafkaBootstrapServers;
        private readonly IConfiguration _configuration;


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
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
