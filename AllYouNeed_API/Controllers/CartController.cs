using AllYouNeed_Models.DTOS.Requests;
using AllYouNeed_Models.DTOS.Respoonses;
using AllYouNeed_Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AllYouNeed_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICartServices _cartServices;

        public CartController(ICartServices cartServices)
        {
            _cartServices = cartServices;
        }

        [HttpPost("make-an-order")]
        public async Task<IActionResult> MakeOrder(string email, [FromBody] CartDTO cart)
            => Ok(await _cartServices.MakeAnOrder(email, cart));
        

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetById(string cartId)
            => Ok(await _cartServices.GetOrder(cartId));


        [HttpPut("update-order")]
        public async Task<IActionResult> Put(string id, [FromBody] CartDTO request)
            => Ok(await _cartServices.UpdateOrder(id, request));

        [HttpDelete("delete")]
        public async Task Delete(string id)
            => await _cartServices.DeleteOrder(id);

        
    }
}
