using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllYouNeed_Models.Models
{
    
    [CollectionName("Cart")]
    public class ShoppingCart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("products")]
        public Dictionary<string, int>? Products { get; set; } = new Dictionary<string, int>();

        [BsonElement("email")]
        [EmailAddress]
        public string Shopper { get; set; } = string.Empty;
    }
}
