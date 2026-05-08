namespace delivery_order_services.Application.Shared.Abstractions
{
    public interface IProducerAbstractions
    {
        Task ProduceAsync(TEnvelope envelope, CancellationToken cancellationToken);
    }
}
