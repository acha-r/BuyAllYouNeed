using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllYouNeed_Models.DTOS.Respoonses
{
    public class OrderRequest
    {
        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;       
    }
    
    public class OrderResponse
    {
        public string OrderId { get; set; }
        public decimal Total { get; set; }
        public string Address { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
