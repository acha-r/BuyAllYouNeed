using AllYouNeed_Models.DTOS.Requests;
using AllYouNeed_Models.Models;
using AllYouNeed_Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AllYouNeed_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController : ControllerBase
    {
        private readonly IMerchantServices merchantServices;

        public MerchantController(IMerchantServices merchantServices)
        {
            this.merchantServices = merchantServices;
        }

        [HttpGet("get-all-merchants")]
        public async Task<ActionResult<List<Merchant>>> Get()
            => await merchantServices.GetMerchants();

        [HttpGet("get-merchant")]
        public async Task<ActionResult<Merchant>> Get(string email)
            => await merchantServices.GetMerchant(email);

        [HttpPost("register-merchant")]
        public async Task<IActionResult> Post(string email, [FromBody] DepositRequest request)
        {
            return Ok(await merchantServices.RegisterMerchant(email, request));
        }

        [HttpPut("update-merchant")]
        public async Task<IActionResult> Put(string email, [FromBody] UpdateMerchantRequest merchant)
            => Ok(await merchantServices.UpdateMerchantInfo(email, merchant));


        [HttpPut("update-balance")]
        public async Task UpdateBalance(string email, [FromBody]DepositRequest request)
        => await merchantServices.UpdateWallet(email, request);

        [HttpPut("change-active-status")]
        public async Task IsActive(string email)
        {
            await merchantServices.ToggleActiveStatus(email);
        }
    }
}
