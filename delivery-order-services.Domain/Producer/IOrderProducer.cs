using delivery_order_services.Domain.Entities;

namespace delivery_order_services.Domain.Producer
{
    public interface IOrderProducer
    {
        Task HandleAsync(OrderEnvelope entity);
    }
}