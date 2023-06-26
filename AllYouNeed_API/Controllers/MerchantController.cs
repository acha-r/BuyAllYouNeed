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

        [HttpGet]
        public async Task<ActionResult<List<Merchant>>> Get()
            => await merchantServices.GetMerchants();

        [HttpGet("{id}")]
        public async Task<ActionResult<Merchant>> Get(string id)
            => await merchantServices.GetMerchant(id);

        [HttpPost]
        public async Task<ActionResult<Merchant>> Post([FromBody] Merchant merchant)
        {
            await merchantServices.RegisterMerchant(merchant);
            return CreatedAtAction(nameof(Get), new { id = merchant.Id, merchant });
        }

        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Merchant merchant)
            => await merchantServices.UpdateMerchantInfo(id, merchant);


        // DELETE api/<MerchantController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        => await merchantServices.DeleteMerchant(id);
    }
}
