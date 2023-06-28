using AllYouNeed_Models.DTOS.Requests;
using AllYouNeed_Models.DTOS.Respoonses;
using AllYouNeed_Models.Interface;
using AllYouNeed_Models.Models;
using AllYouNeed_Services.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllYouNeed_Services.Implementation
{
    public class CartServices : ICartServices
    {
        private readonly IMongoCollection<ShoppingCart> _cart;
        private IMongoDatabase _database;
        //private readonly IProductService _productService;


        public CartServices(IAllYouNeedRepo dbSetting, IMongoClient mongoClient)
        {
            _database = mongoClient.GetDatabase(dbSetting.DatabaseName);
            _cart = _database.GetCollection<ShoppingCart>("Cart");
        }

        public async Task MakeAnOrder(string email, CartDTO cart)
        {
            var customer = await _database.GetCollection<Merchant>("Merchants").Find(x => x.Email == email).FirstOrDefaultAsync() ?? throw new Exception("User not found");
            decimal totalAmt = 0;

            foreach (var item in cart.Products)
            {
                var product = await _database.GetCollection<Product>("Products").Find(x => x.Id.ToString() == item.Key).FirstOrDefaultAsync() ?? throw new Exception("Product not found");
                if (!product.InStock) throw new Exception($"{product.Name} is out of stock");
                if (item.Value <= 1) throw new Exception("Please pick at least one item");
                totalAmt += (product.Price * item.Value);
            }

            await _cart.InsertOneAsync(new ShoppingCart
            {
                Products = cart.Products,
                BuyerEmail = email,
                DeliveryAddress = cart.DeliveryAddress,
                TotalAmt = totalAmt
            });          

        }

        public async Task<CartResponse> UpdateOrder(string cartId, CartDTO cartUpdate)
        {
            var cart = await _cart.Find(x => x.Id.ToString() == cartId).FirstOrDefaultAsync() ?? throw new Exception("Order not found");
            cart.DeliveryAddress = cartUpdate.DeliveryAddress;
            cart.Products = cartUpdate.Products;

            await _cart.ReplaceOneAsync(x => x.Id.ToString() == cartId, cart);
            return new CartResponse
            {
                Products = cart.Products,
                Response = "Updated"
            };
        }
    }
}
