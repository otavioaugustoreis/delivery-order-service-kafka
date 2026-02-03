

using Confluent.Kafka;
using delivery_order_services.Domain.Entities;

namespace delivery_order_services.Domain.Producer
{
    public class OrderProducer : IOrderProducer
    {
        public async Task HandleAsync(OrderEnvelope entity)
        {
            try
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = "localhost:9092"
                };


            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
