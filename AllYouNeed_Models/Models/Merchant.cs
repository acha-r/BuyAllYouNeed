using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AllYouNeed_Models.Models
{
   // [BsonIgnoreExtraElements]
    public class Merchant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] //converts Mongo datatype to a .net datatype
        public string Id { get; set; }

        [BsonElement("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [BsonElement("last_name")]
        public string LastName { get; set; } = string.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("active")]
        public bool IsActive { get; set; } 

        [BsonElement("balance")]
        public decimal Balance { get; set; }

    }
}
