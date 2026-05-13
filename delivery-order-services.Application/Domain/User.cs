using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace delivery_order_services.Application.Domain
{
    public class User
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

        public void SetClient() 
        {  
            UserType =  Enum.UserType.CLIENT.ToString();
        }
    }
}
