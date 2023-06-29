using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllYouNeed_Models.Models
{
    [CollectionName("Orders")]
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("total_amount")]
        public string CartId { get; set; } = string.Empty;

        [BsonElement("total_amount")]
        public decimal TotalAmount { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        [BsonElement("state")]
        public string State { get; set; }

        [BsonElement("shopper")]
        public string Shopper { get; set; } = string.Empty;

        [BsonElement("has_paid")]
        public bool HasPaid { get; set; } = false;

    }
}
