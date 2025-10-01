using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using delivery_order_services.Domain.Entities.Enum;

namespace delivery_order_services.Domain.Entities
{
    public  class OrderEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("ProductName")]
        public string ProductName { get; set; } = string.Empty;

        [BsonElement("OrderStatus")]
        public string OrderStatus { get; set; } = string.Empty;

        [BsonElement("Client")]
        public string  Client{ get; set; } = string.Empty;

        public string GetOrderStatus(OrderStatus orderStatus) => orderStatus.ToString();
    }
}
