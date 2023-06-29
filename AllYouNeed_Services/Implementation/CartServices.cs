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
        private readonly IMongoCollection<Cart> _cart;
        private IMongoDatabase _database;
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContext;



        public CartServices(IHttpContextAccessor httpContext, IAllYouNeedRepo dbSetting, IMongoClient mongoClient, IProductService productService)
        {
            _database = mongoClient.GetDatabase(dbSetting.DatabaseName);
            _cart = _database.GetCollection<Cart>("Cart");
            _productService = productService;
            _httpContext = httpContext;
        }

        public async Task<string> CreateCart(CartDTO cart)
        {
            var user = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Name);

            var existingCart = await _cart.Find(x => x.Shopper == user).FirstOrDefaultAsync();
            if (existingCart != null) throw new Exception("You have an existing cart. Add to cart");

            decimal totalAmt = 0;

            if (cart.Products is null) throw new Exception("No item added");

            Cart newCart = new();

            foreach (var item in cart.Products)
            {
                var product = await _productService.GetProductById(item.Key);

                if (item.Value <= 1) throw new Exception("Please pick at least one item");

                if (item.Value > product.Quantity) throw new Exception($"Only {product.Quantity} {product.Name} in stock");

                newCart.Products.Add(item.Key, item.Value);

                totalAmt += product.Price * item.Value;
            }

            newCart.Shopper = user;
            newCart.SubTotal = totalAmt;

            await _cart.InsertOneAsync(newCart);

            return "Cart created";
        }

        public async Task<CartResponse> GetCartSummary(string cartId)
        {
            var cart = await _cart.Find(x => x.Id.ToString() == cartId).FirstOrDefaultAsync() ?? throw new Exception("No cart found");
            CartResponse response = new();

            foreach (var item in cart.Products)
            {
                var product = await _productService.GetProductById(item.Key);

                response.Products.Add($"{product.Name} {item.Key}\t\t{item.Value}\nNGN {product.Price * item.Value}");
            }

            response.Shopper = cart.Shopper;
            response.Total = cart.SubTotal;
            response.Response = "You can add more items to cart OR proceed to checkout";
            return response;
        }

        public async Task<string> RemoveFromCart(string cartId, CartDTO cartUpdate)
        {
            var cart = await _cart.Find(x => x.Id.ToString() == cartId).FirstOrDefaultAsync() ?? throw new Exception("No cart found");

            foreach (var item in cartUpdate.Products)
            {
                var value = await CheckCartItems(cart, item.Key, item.Value);

                cart.Products.Remove(value.Item1);
            }

            await _cart.ReplaceOneAsync(x => x.Id.ToString() == cartId, cart);

            return "Cart updated";
        }


        public async Task<string> AddToCart(string cartId, CartDTO cartUpdate)
        {
            var cart = await _cart.Find(x => x.Id.ToString() == cartId).FirstOrDefaultAsync() ?? throw new Exception("No cart found");

            foreach (var item in cartUpdate.Products)
            {
                var value = await CheckCartItems(cart, item.Key, item.Value, "add");

                cart.Products.Add(value.Item1, value.Item2);
            }

            await _cart.ReplaceOneAsync(x => x.Id.ToString() == cartId, cart);

            return "Cart updated";
        }

        public async Task DeleteCart(string cartId)
            => await _cart.DeleteOneAsync(cartId);

        private async Task<(string, int)> CheckCartItems(Cart cart, string productId, int num, string addOrRemo = "remove")
        {
            (string, int) value;

            var product = await _database.GetCollection<Product>("Products").Find(x => x.Id.ToString() == productId)
                    .FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Product does not exist");
            if (num! >= 1) throw new Exception("Invalid number");
            if (num > product.Quantity) throw new Exception($"Only {product.Quantity} {product.Name} in stock");

            value = (productId, num);

            switch (addOrRemo)
            {
                case "add":
                    if (cart.Products.ContainsKey(productId)) throw new KeyNotFoundException($"{product.Name} ({productId}): " +
                $"Item already in cart");
                    break;
                default:
                    if (!cart.Products.ContainsKey(productId)) throw new KeyNotFoundException($"{product.Name} ({productId}): " +
                $"You do not have this item in your cart");
                    break;
            }

            return value;
        }
    }
}
