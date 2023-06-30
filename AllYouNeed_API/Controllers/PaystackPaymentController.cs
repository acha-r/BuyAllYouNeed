using AllYouNeed_Services.Implementation;
using AllYouNeed_Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayStack.Net;

namespace AllYouNeed_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaystackPaymentController : ControllerBase
    {
        private readonly IPaystackPaymentService _paymentService;

        public PaystackPaymentController(IPaystackPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> InitializePayement(string cartId)
        {
            var response = await _paymentService.PayViaPaystack(cartId);
            
            return Ok(response.Data.AuthorizationUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Verfiy(string reference, string cartId)
        {
            return Ok(await _paymentService.VerfiyPaystackPayment(reference, cartId));
        }
    }

}
