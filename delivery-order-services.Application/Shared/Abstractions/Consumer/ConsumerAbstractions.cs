using Confluent.Kafka;
using delivery_order_services.Application.Domain;
using delivery_order_services.Application.Shared.Contants;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace delivery_order_services.Application.Shared.Abstractions.Consumer
{
    public class ConsumerAbstractions : IConsumerAbstractions
    {

        private readonly ILogger<ConsumerAbstractions> _logger;
        private readonly string _kafkaBootstrapServers;
        private readonly ConsumerConfiguration _consumerConfig;

        private const int maxRetries = 3;

        public ConsumerAbstractions(ILogger<ConsumerAbstractions> logger, IOptions<ConsumerConfiguration> consumerConfig  )
        {
            _logger = logger;
            _consumerConfig = consumerConfig.Value;
        }

        public async Task ExecuteAsync(string topicName, string consumerGroup ,CancellationToken cancellationToken)
        {
           
                _logger.LogInformation("Order notifier running at: {time}", DateTimeOffset.Now);

                var config = new ConsumerConfig
                {
                    BootstrapServers = _consumerConfig.BootstrapServers,
                    GroupId = consumerGroup,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = false
                };

                using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
                consumer.Subscribe(topicName);

                _logger.LogInformation("Consumer started for the topic: {Topic}", topicName);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(cancellationToken);

                    try
                    {
                        await ProcessMessageWithRetryAsync(consumeResult, cancellationToken);
                        consumer.Commit(consumeResult);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex,
                                "[{Type}] Fatal failure processing message. Input:{@input}",
                                nameof(ExecuteAsync),
                                new
                                {
                                    Offset = consumeResult.Offset.Value
                                });

                        await SendToDLQAsync(consumeResult.Message.Value, ex, cancellationToken);

                        consumer.Commit(consumeResult);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Consumer finished due to cancellation.");
                consumer.Close();
            }
        }

        private async Task ProcessMessageWithRetryAsync(ConsumeResult<Ignore, string> consumeResult, CancellationToken cancellationToken)
        {
            for (var attempt = 0; attempt <= maxRetries; attempt++)
            {
                try
                {
                    var order = JsonSerializer.Deserialize<Order>(consumeResult.Message.Value);

                    if (order is null)
                        throw new InvalidOperationException("Invalid message: could not deserialize the Order.");

                    _logger.LogInformation(
                        "[{Type}] Message processed successfully. Input:{@input}",
                        nameof(ProcessMessageWithRetryAsync),
                        new
                        {
                            Topic = consumeResult.Topic,
                            Offset = consumeResult.Offset.Value,
                            OrderId = order.Id
                        });

                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex,
                        "[{Type}] Error processing message. Input:{@input}",
                        nameof(ProcessMessageWithRetryAsync),
                        new
                        {
                         Attempt = attempt,
                         MaxRetries = maxRetries
                        });

                    if (attempt == maxRetries)
                        throw;
                }
            }
        }
        private Task SendToDLQAsync(string message, Exception ex, CancellationToken cancellationToken)
        {
            _logger.LogError(ex, "Sending message to DLQ: {Message}", message);
            return Task.CompletedTask;
        }
    }
}
