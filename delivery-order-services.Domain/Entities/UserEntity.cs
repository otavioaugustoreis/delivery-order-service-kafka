using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


namespace delivery_order_services.Domain.Entities
{
    public class UserEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("Name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("Email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("UserType")]
        public string UserType { get; set; } = string.Empty;
    }
}
