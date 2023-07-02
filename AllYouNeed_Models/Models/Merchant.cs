using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AllYouNeed_Models.Models
{
    [BsonIgnoreExtraElements]
    public class Merchant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] //converts Mongo datatype to a .net datatype
        public ObjectId Id { get; set; }

        [BsonElement("email")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [BsonElement("full_name")]
        public string FullName { get; set; } = string.Empty;

        [BsonElement("active")]
        public bool IsActive { get; set; } = true;

        [BsonElement("balance")]
        public decimal Balance { get; set; }

    }
}
