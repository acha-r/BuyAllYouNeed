using AllYouNeed_Models.Models;
using AllYouNeed_Services.Implementation;
using AllYouNeed_Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace AllYouNeed_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<List<Product>> GetBySearch(string keyword)
        {
            return await _productService.GetProductBySearch(keyword);
        }
        [HttpGet("{id}")]
        public async Task<Product> GetById(string id)
        {
            return await _productService.GetProductById(id);
        }

        [Authorize(Roles = "USER")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            await _productService.RegisterProduct(product);
            return CreatedAtAction(nameof(GetBySearch), new {id  = product.Id, product});
        }

        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Product product)
            => await _productService.UpdateProductInfo(id, product);

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        => await _productService.DeleteProduct(id);
    }
}
