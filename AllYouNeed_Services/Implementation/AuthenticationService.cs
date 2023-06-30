using AllYouNeed_Models.DTOS.Requests;
using AllYouNeed_Models.DTOS.Respoonses;
using AllYouNeed_Models.Models;
using AllYouNeed_Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;

namespace AllYouNeed_Services.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public AuthenticationService(RoleManager<ApplicationRole> rolemanager, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _roleManager = rolemanager;
        }
        public async Task<LogInResponse> Login(LogInRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new LogInResponse
                {
                    Success = false,
                    Message = "Invalid log in details"
                };
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));
            claims.AddRange(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asdf;lkj12qw09po"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(30);
            var token = new JwtSecurityToken(
                issuer: "https://localhost:7061",
                audience: "https://localhost:7061",
                claims : claims,
                expires : expires,
                signingCredentials: credentials
                );

            return new LogInResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Message = "Login successful",
                Email = user.Email,
                Success = true,
                UserId = user.Id.ToString(),
            };
        }

        public async Task<RegistrationResponse> Register(RegisterRequest request)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email);
            if (userExists is not null)
            {
                return new RegistrationResponse
                {
                    Success = false,
                    Message = "User already exists"
                };
            }
            userExists = new ApplicationUser
            {
                FullName = request.FullName,
                Email = request.Email,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                UserName = request.Email
            };

            var createUser = await _userManager.CreateAsync(userExists, request.Password);

            if (!createUser.Succeeded)
                return new RegistrationResponse
                {
                    Success = false,
                    Message = $"An error occurred : {createUser.Errors.First().Description}"
                };

            await _userManager.AddToRoleAsync(userExists, "USER");

            return new RegistrationResponse
            {
                Success = true,
                Message = "User registered successfully"
            };
        }

        public async Task<RoleResponse> CreateRole(RoleRequest request)
        {
            ApplicationRole roleExists = await _roleManager.FindByNameAsync(request.Role.Trim().ToLower());
            if (roleExists is not null)
            {
                throw new InvalidOperationException($"Role with name {request.Role} already exist");
            }

            var appRole = new ApplicationRole
            {
                Name = request.Role
            };
             await _roleManager.CreateAsync(appRole);

             return new RoleResponse { Message = "Role created successfully" };
        }
        public async Task<RoleResponse> AddUserToRole(string userId, string roleName)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId) ?? throw new InvalidOperationException($"User '{userId}' does not Exist!");           

            ApplicationRole role = await _roleManager.FindByNameAsync(roleName) ?? throw new InvalidOperationException($"Role '{roleName}' does not Exist!");
           
            await _userManager.AddToRoleAsync(user, role.Name);

            return new RoleResponse()
            {
                Message = $"{userId} has been assigned a {roleName} role",
            };
        }
    }
}
