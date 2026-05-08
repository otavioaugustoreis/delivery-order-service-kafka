using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using delivery_order_services.Application.Entities.Enum;

namespace delivery_order_services.Application.Entities
{
    public class OrderEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("ProductName")]
        public string ProductName { get; set; } = string.Empty;

        [BsonElement("OrderStatus")]
        public string OrderStatus { get; set; } = default!;

        [BsonElement("ClientId")]
        public string ClientId{ get; set; } = string.Empty;

        public string GetOrderStatus(OrderStatus orderStatus) => orderStatus.ToString();

        public void OrderCreated()
        {
            OrderStatus = delivery_order_services.Application.Entities.Enum.OrderStatus.CREATED.ToString();
        }

        public void OrderDelivered()
        {
            OrderStatus = delivery_order_services.Application.Entities.Enum.OrderStatus.DELIVERED.ToString();
        }
    }
}
