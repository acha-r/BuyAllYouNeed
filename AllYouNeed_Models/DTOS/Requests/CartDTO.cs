using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllYouNeed_Models.DTOS.Requests
{
    public class CartDTO
    {
        [Required]
        public Dictionary<string, int>? Products { get; set; } = new Dictionary<string, int>();

        
    }

    
}
