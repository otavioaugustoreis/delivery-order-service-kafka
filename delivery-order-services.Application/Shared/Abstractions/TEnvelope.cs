using delivery_order_services.Application.Entities;

namespace delivery_order_services.Application.Shared.Abstractions
{
    public interface TEnvelope
    {
        string Key { get; set; }
        OrderEntity Value { get; set; }
        string Topic { get; }
    }
}
