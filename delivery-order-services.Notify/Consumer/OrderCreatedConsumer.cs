
using Confluent.Kafka;
using delivery_order_services.Application.Domain;
using delivery_order_services.Application.Shared.Abstractions.Consumer;
using delivery_order_services.Application.Shared.Contants;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace delivery_order_services.Notify.Features
{
    public class OrderCreatedConsumer : BackgroundService
    {
        private readonly ILogger<OrderCreatedConsumer> _logger;
        private readonly ConsumerConfiguration _consumerConfig;

        public OrderCreatedConsumer(
            ILogger<OrderCreatedConsumer> logger,
            IOptions<ConsumerConfiguration> consumerConfig)
        {
            _logger = logger;
            _consumerConfig = consumerConfig.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {

            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Order notifier running at: {time}", DateTimeOffset.Now);

                var config = new ConsumerConfig
                {
                    BootstrapServers = _consumerConfig.BootstrapServers,
                    GroupId = ConsumerGroups.OrderGroupId,
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
