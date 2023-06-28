﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllYouNeed_Models.DTOS.Requests
{
    public class UpdateMerchantRequest
    {
        [Required]
        public string FullName { get; set; }
    }

    public class DepositRequest
    {
        [Required]
        public decimal Balance { get; set; }
    }

}