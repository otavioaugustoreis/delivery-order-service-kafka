namespace delivery_order_services.Application.Shared.Abstractions.Consumer
{
    public interface IConsumerAbstractions
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
