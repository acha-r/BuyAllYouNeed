﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllYouNeed_Models.DTOS.Requests
{
    public class RegisterRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; }
        
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        
        [Required, DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Passwords do not match")] 
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

