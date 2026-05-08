
using Confluent.Kafka;
using delivery_order_services.Application.Entities;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace delivery_order_services.Application.Shared.Abstractions.Consumer
{
    public class ConsumerAbstractions : IConsumerAbstractions
    {

        private readonly ILogger<ConsumerAbstractions> _logger;
        private readonly string _orderTopicName;
        private readonly string _kafkaBootstrapServers;
        private readonly string _notifierConsumeGroupName;

        public ConsumerAbstractions(ILogger<ConsumerAbstractions> logger)
        {
            _logger = logger;
        }

        public async  Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
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
                                var consumeResult = consumer.Consume(cancellationToken);

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
