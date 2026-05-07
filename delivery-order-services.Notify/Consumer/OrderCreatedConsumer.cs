
using Confluent.Kafka;
using delivery_order_services.Application.Entities;
using System.Text.Json;

namespace delivery_order_services.Notify.Features
{
    public class OrderCreatedConsumer : BackgroundService
    {
        private readonly ILogger<OrderCreatedConsumer> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _orderTopicName;
        private readonly string _kafkaBootstrapServers;
        private readonly string _notifierConsumeGroupName; 

        public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _orderTopicName =  "order-created-topic";
            _kafkaBootstrapServers =  "localhost:9092";
            _notifierConsumeGroupName =  "notifier-consumer-group";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Order notifier running at: {time}", DateTimeOffset.Now);

                var config = new ConsumerConfig
                {
                    BootstrapServers = _kafkaBootstrapServers,
                    GroupId = _notifierConsumeGroupName,
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumer.Subscribe(_orderTopicName);

                    CancellationTokenSource cts = new CancellationTokenSource();
                    Console.CancelKeyPress += (_, e) => {
                        e.Cancel = true;
                        cts.Cancel();
                    };

                    try
                    {
                        while (true)
                        {
                            try
                            {
                                var consumeResult = consumer.Consume(cts.Token);

                                var order = JsonSerializer.Deserialize<OrderEntity>(consumeResult.Message.Value);

                                Console.WriteLine($"Mensagem recebida: {consumeResult.Message.Value}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Erro ao consumir a mensagem: {ex.Message}");
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        consumer.Close();
                    }
                }
            }
        }
    }
}
