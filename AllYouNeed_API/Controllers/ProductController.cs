﻿using AllYouNeed_Models.DTOS.Requests;
using AllYouNeed_Models.Models;
using AllYouNeed_Services.Implementation;
using AllYouNeed_Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace AllYouNeed_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpPost("pre-seed")]
        public async Task PreSeed()
        {
            await _productService.PreSeedProducts();
        }

        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<List<ProductRegistration>> GetBySearch(string keyword) 
            => await _productService.GetProductBySearch(keyword);

        [Authorize(Roles = "admin")]
        [HttpGet("get-by-id")]
        public async Task<ProductRegistration> GetById(string id)
            => await _productService.GetProductById(id);


        [Authorize(Roles = "admin")]
        [HttpPost("register-product")]
        public async Task<IActionResult> Post([FromBody] ProductRegistration product)
            => Ok(await _productService.RegisterProduct(product));


        [Authorize(Roles = "admin")]
        [HttpPut("update-product")]
        public async Task Put(string id, [FromBody] ProductRegistration product)
            => await _productService.UpdateProductInfo(id, product);


        [Authorize(Roles = "admin")]
        [HttpDelete("delete")]
        public async Task Delete(string id)
            => await _productService.DeleteProduct(id);


        [Authorize(Roles = "admin")]
        [HttpPut("check-in-stock")]
        public async Task<IActionResult> CheckStatus(string id)
           => Ok(await _productService.UpdateInStockStatus(id));
        
    }
}
