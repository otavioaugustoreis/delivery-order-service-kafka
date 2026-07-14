namespace delivery_order_services.Application.Shared.Abstractions.Consumer
{
    public interface IConsumerAbstractions
    {
       Task ExecuteAsync<T>(
           string topicName,
           string consumerGroup,
           Func<T, CancellationToken, Task> messageHandler,
           CancellationToken cancellationToken);
    }
}
