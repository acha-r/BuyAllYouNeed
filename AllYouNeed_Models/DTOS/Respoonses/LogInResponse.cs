using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllYouNeed_Models.DTOS.Respoonses
{
    public class LogInResponse
    {
        public bool Success { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class RegistrationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
