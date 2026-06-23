namespace delivery_order_services.Application.Shared.Abstractions.Consumer
{
    public interface IConsumerAbstractions
    {
       Task ExecuteAsync(string topicName, string consumerGroup, CancellationToken cancellationToken);
    }
}
