namespace delivery_order_services.Application.Shared.Abstractions.Producer
{
    public interface IProducerAbstractions
    {
        Task ProduceAsync<T>(TEnvelope<T> envelope, CancellationToken cancellationToken);
    }
}
