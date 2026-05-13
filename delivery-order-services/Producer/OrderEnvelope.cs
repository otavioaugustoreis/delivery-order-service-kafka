using delivery_order_services.Application.Domain;
using delivery_order_services.Application.Shared;
using delivery_order_services.Application.Shared.Abstractions;

namespace delivery_order_services.Producer
{
    public class OrderEnvelope : TEnvelope
	{
        public string Key { get; set; } = default!;
        public Order Value { get; set; }
        public string Topic { get; } = Topics.OrderTopic;
    }
}
