using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace delivery_order_services.Application.Shared.Abstractions.Consumer
{
    public class ConsumerAbstractions : IConsumerAbstractions
    {

        private readonly ILogger<ConsumerAbstractions> _logger;
        private readonly ConsumerConfiguration _consumerConfig;

        private const int maxRetries = 3;
        private const int baseDelayMs = 200;
        public ConsumerAbstractions(ILogger<ConsumerAbstractions> logger, IOptions<ConsumerConfiguration> consumerConfig)
        {
            _logger = logger;
            _consumerConfig = consumerConfig.Value;
        }

        public async Task ExecuteAsync<T>(
            string topicName, 
            string consumerGroup, 
            Func<T, CancellationToken, Task> messageHandler,
            CancellationToken cancellationToken)
        {
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
                        var message = JsonSerializer.Deserialize<T>(
                            consumeResult.Message.Value,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                        if (message is null)
                            throw new InvalidOperationException("Invalid message: could not deserialize the Order.");

                        await ProcessMessageWithRetryAsync(message, messageHandler, cancellationToken);

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
        private async Task ProcessMessageWithRetryAsync<T>(
            T message,
            Func<T, CancellationToken, Task> messageHandler, 
            CancellationToken cancellationToken)
        {
            for (var attempt = 0; attempt <= maxRetries; attempt++)
            {
                try
                {
                    await messageHandler(message, cancellationToken);

                    _logger.LogInformation(
                        "[{Type}] Message processed successfully",nameof(ProcessMessageWithRetryAsync));

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


                    var delay = TimeSpan.FromMilliseconds(baseDelayMs * Math.Pow(2, attempt));
                    try
                    {
                        await Task.Delay(delay, cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                }
            }

            throw new InvalidOperationException("Retry loop exited unexpectedly.");
        }
        private Task SendToDLQAsync(string message, Exception ex, CancellationToken cancellationToken)
        {
            _logger.LogError(ex, "Sending message to DLQ: {Message}", message);
            return Task.CompletedTask;
        }
    }
}
