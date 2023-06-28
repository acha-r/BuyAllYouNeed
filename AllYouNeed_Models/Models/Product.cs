using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AllYouNeed_Models.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] //converts Mongo datatype to a .net datatype
        public ObjectId Id { get; set; }

        [BsonElement("name")] 
        public string Name { get; set; } = string.Empty;

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("quantity")]
        public long Quantity { get; set; }

        [BsonElement("in_stock")]
        public bool InStock { get; set; } = true;

    }

}
