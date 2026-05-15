using delivery_order_services.Application.Domain;
using delivery_order_services.Application.Shared.Abstractions;
using delivery_order_services.Application.Shared.Contants;

namespace delivery_order_services.Producer
{
    public class OrderEnvelope : TEnvelope
	{
        public string Key { get; set; } = default!;
        public Order Value { get; set; }
        public string Topic { get; } = Topics.OrderTopic;
    }
}
