using AllYouNeed_Models.DTOS.Requests;
using AllYouNeed_Models.DTOS.Respoonses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AllYouNeed_Services.Interface
{
    public interface IAuthenticationService
    {
        Task<RoleResponse> CreateRole(RoleRequest request);
        Task<LogInResponse> Login(LogInRequest request);
        Task<RegistrationResponse> Register(RegisterRequest request);
        Task<RoleResponse> AddUserToRole(string userId, string roleName);
    }
}
