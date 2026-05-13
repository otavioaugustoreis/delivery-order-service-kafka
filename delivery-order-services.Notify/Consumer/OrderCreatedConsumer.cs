
using Confluent.Kafka;
using delivery_order_services.Application.Domain;
using delivery_order_services.Application.Shared;
using System.Text.Json;

namespace delivery_order_services.Notify.Features
{
    public class OrderCreatedConsumer : BackgroundService
    {
        private readonly ILogger<OrderCreatedConsumer> _logger;
        
        public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Order notifier running at: {time}", DateTimeOffset.Now);

                var config = new ConsumerConfig
                {
                    BootstrapServers = "localhost:9092",
                    GroupId = "notifier-consumer-group",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumer.Subscribe(Topics.OrderTopic);

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

                                var order = JsonSerializer.Deserialize<Order>(consumeResult.Message.Value);

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
