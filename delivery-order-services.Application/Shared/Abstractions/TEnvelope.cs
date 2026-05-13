using delivery_order_services.Application.Domain;

namespace delivery_order_services.Application.Shared.Abstractions
{
    public interface TEnvelope
    {
        string Key { get; set; }
        Order Value { get; set; }
        string Topic { get; }
    }
}
