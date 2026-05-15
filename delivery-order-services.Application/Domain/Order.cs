using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using delivery_order_services.Application.Domain.Enum;

namespace delivery_order_services.Application.Domain
{
    public class Order
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

        [BsonElement("IdempotencyKey")]
        public string IdempotencyKey { get; set; } = string.Empty;

        public string GetOrderStatus(OrderStatus orderStatus) => orderStatus.ToString();

        public void SetCreated()
        {
            OrderStatus = Enum.OrderStatus.CREATED.ToString();
        }

        public void SetDelivered()
        {
            OrderStatus = Enum.OrderStatus.DELIVERED.ToString();
        }

        public void SetIdempotencyKey(string idempotencyKey)
        {
            IdempotencyKey = idempotencyKey;
        }
    }
}
