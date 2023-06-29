using AllYouNeed_Models.DTOS.Requests;
using AllYouNeed_Models.DTOS.Respoonses;
using AllYouNeed_Models.Interface;
using AllYouNeed_Models.Models;
using AllYouNeed_Services.Interface;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System.Security.Claims;

namespace AllYouNeed_Services.Implementation
{
    public class CartServices : ICartServices
    {
        private readonly IMongoCollection<ShoppingCart> _cart;
        private IMongoDatabase _database;
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContext;


        public CartServices(IHttpContextAccessor httpContext, IAllYouNeedRepo dbSetting, IMongoClient mongoClient, IProductService productService)
        {
            _database = mongoClient.GetDatabase(dbSetting.DatabaseName);
            _cart = _database.GetCollection<ShoppingCart>("Cart");
            _productService = productService;
            _httpContext = httpContext;
        }

        public async Task<string> AddToCart(CartDTO cart)
        {
            var user = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Name);

            if (cart.Products is null) throw new Exception("No item added");

            ShoppingCart cartt = new();

            foreach (var item in cart.Products)
            {
                var product = await _productService.GetProductById(item.Key);
                if (item.Value <= 1) throw new Exception("Please pick at least one item");
                if (item.Value > product.Quantity) throw new Exception($"Only {product.Quantity} {product.Name} available");

                cartt.Products.Add($"{product.Name} ({item.Key})", item.Value);
            }

            cartt.Shopper = user;

            await _cart.InsertOneAsync(cartt);

            return "Added to cart";
        }

        public async Task<CartResponse> GetCartSummary()
        {
            var user = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Name);

            //log in to confirm if this works
            if (user == null) throw new Exception("Please log in to see summary");

            var cart = await _cart.Find(x => x.Shopper == user).FirstOrDefaultAsync() ?? throw new Exception("No cart found");
            CartResponse response = new();

            foreach (var item in cart.Products)
            {
                response.Products.Append(($"{item.Key}\t\t+ {item.Value}"));
            }
            return response;
        }
 
        public async Task<string> RemoveFromCart(string cartId, CartDTO cartUpdate)
        {
            var cart = await _cart.Find(x => x.Id.ToString() == cartId).FirstOrDefaultAsync() ?? throw new Exception("No cart found");

            foreach (var item in cartUpdate.Products)
            {
                var product = await _database.GetCollection<Product>("Products").Find(x => x.Id.ToString() == item.Key)
                    .FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Product does not exist");

                if (!cart.Products.ContainsKey(item.Key)) throw new KeyNotFoundException($"{product.Name} ({item.Key}): " +
                    $"You do not have this item in your cart");

                if (item.Value! >= 1) throw new Exception("Invalid number");

                cart.Products.Remove($"{product.Name} ({item.Key})");
            }

            await _cart.ReplaceOneAsync(x => x.Id.ToString() == cartId, cart);

            return "Cart updated";
        }

        public async Task DeleteCart(string cartId)
            => await _cart.DeleteOneAsync(cartId);

    }
}
