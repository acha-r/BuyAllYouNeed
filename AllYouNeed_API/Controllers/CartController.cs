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

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromBody] CartDTO cart)
            => Ok(await _cartServices.CreateCart(cart));
        

        [HttpGet("get-cart-summary")]
        public async Task<IActionResult> GetById()
            => Ok(await _cartServices.GetCartSummary());


        [HttpPut("remove-from-cart")]
        public async Task<IActionResult> Put(string cartId, [FromBody] CartDTO request)
            => Ok(await _cartServices.RemoveFromCart(cartId, request));

        [HttpDelete("delete-cart")]
        public async Task Delete(string id)
            => await _cartServices.DeleteCart(id);

        
    }
}
