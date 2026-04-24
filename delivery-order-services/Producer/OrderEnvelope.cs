

using delivery_order_services.Domain.Entities;
using delivery_order_services.Domain.Shared;

namespace delivery_order_services.Domain.Producer
{
    public class OrderEnvelope
    {
        public string Key { get; set; } = default!;

        public OrderEntity Value { get; set; }

        public string Topic = Topics.OrderTopic;
    }
}
