using AllYouNeed_Models.DTOS.Requests;
using AllYouNeed_Models.DTOS.Respoonses;
using AllYouNeed_Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AllYouNeed_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LogInRequest request)
        {
            try
            {
                var result = await _authService.Login(request);
                return result.Success ? Ok(result) : BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new LogInResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var result = await _authService.Register(request);
                return result.Success ? Ok(result) : BadRequest(result.Message);

            }
            catch (Exception ex)
            {
                return BadRequest(new RegistrationResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create-role")]
        public async Task<IActionResult> CreateRole([FromBody] RoleRequest request)
        {
            var result = await _authService.CreateRole(request);
            return Ok(result);
        }
    }
}
