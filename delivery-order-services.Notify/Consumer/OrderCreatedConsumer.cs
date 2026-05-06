namespace delivery_order_services.Notify.Features
{
    public class OrderCreatedConsumer
    {
        private readonly ILogger<OrderCreatedConsumer> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _orderTopicName;
        private readonly string _kafkaBootstrapServers;
        private readonly string _notifierConsumeGroupName; 
    }
}
