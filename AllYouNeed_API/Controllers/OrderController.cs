using AllYouNeed_Models.DTOS.Respoonses;
using AllYouNeed_Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AllYouNeed_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderServices _orderServices;

        public OrderController(IOrderServices order)
        {
            _orderServices = order;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(string cartId, [FromBody] OrderRequest orderRequest)
        {
            return Ok(await _orderServices.CheckOut(cartId, orderRequest));
        }
    }
}
