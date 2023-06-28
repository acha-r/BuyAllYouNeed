﻿using System.ComponentModel.DataAnnotations;

namespace AllYouNeed_Models.DTOS.Requests
{
    public class LogInRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
