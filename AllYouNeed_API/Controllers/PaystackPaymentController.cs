using AllYouNeed_Services.Implementation;
using AllYouNeed_Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayStack.Net;

namespace AllYouNeed_API.Controllers
{
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
        public async Task<IActionResult> InitializePayement(string productId, string buyerId, int numOfItems)
        {
            var response = await _paymentService.InitializeTransaction(productId, buyerId, numOfItems);
            
           Redirect (response.Data.AuthorizationUrl);
            return Ok(response);
        }

        [HttpGet]
        public IActionResult Verfiy(string reference)
        {
            var response = _paymentService.Verfiy(reference);
            return Ok(response);
        }
    }

}
