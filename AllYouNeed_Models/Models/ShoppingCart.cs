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
        public Dictionary<string, int>? Products { get; set; }

        [BsonElement("total_amt")]
        public decimal TotalAmt { get; set; }

        [BsonElement("email")]
        [EmailAddress]
        public string BuyerEmail { get; set; } = string.Empty;

        [BsonElement("deliver_address")]
        public string DeliveryAddress { get; set; } = string.Empty;

        [BsonElement("has_paid")]
        public bool HasPaid { get; set; } = false;
    }
}
