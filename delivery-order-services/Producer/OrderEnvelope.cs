using delivery_order_services.Application.Entities;
using delivery_order_services.Application.Shared;

namespace delivery_order_services.Producer
{
    public class OrderEnvelope
    {
        public string Key { get; set; } = default!;
        public OrderEntity Value { get; set; }

        public string Topic = Topics.OrderTopic;
    }
}
