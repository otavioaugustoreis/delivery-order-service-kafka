namespace delivery_order_services.Application.Shared.Abstractions.Producer
{
    public interface IProducerAbstractions
    {
        Task ProduceAsync(TEnvelope envelope, CancellationToken cancellationToken);
    }
}
