using delivery_order_services.Domain.Entities;

namespace delivery_order_services.Producer
{
    public interface IOrderProducer
    {
        Task HandleAsync(OrderEnvelope envelope);
    }
}