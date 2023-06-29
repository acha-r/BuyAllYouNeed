using AllYouNeed_Models.Interface;
using AllYouNeed_Models.Models;
using AllYouNeed_Services.Interface;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using PayStack.Net;

namespace AllYouNeed_Services.Implementation
{
    public class PaymentService : IPaystackPaymentService
    {
        private IConfiguration _configuration;
        private readonly string? _token;
        private PayStackApi _paystackApi;
        private IProductService _productService;
        private IMongoDatabase _database;
        private readonly IMongoCollection<Order> _order;
        private readonly IMongoCollection<Cart> _cart;
        private readonly IMongoCollection<Product> _product;


        public PaymentService(IAllYouNeedRepo dbSetting, IMongoClient mongoClient, IConfiguration configuration, IProductService product)
        {
            _productService = product;
            _configuration = configuration;
            _token = _configuration["Paystack:SecretKey"];
            _paystackApi = new PayStackApi(_token);
            _database = mongoClient.GetDatabase(dbSetting.DatabaseName);
            _order = _database.GetCollection<Order>("Orders");
            _cart = _database.GetCollection<Cart>("Cart");
            _product = _database.GetCollection<Product>("Products");
        }

        public async Task<TransactionInitializeResponse> PayViaPaystack(string orderId)
        {
            var order = await _order.Find(x => x.Id.ToString() == orderId).FirstOrDefaultAsync() ?? throw new Exception("Order not found");

            
            var request = new TransactionInitializeRequest()
            {
                AmountInKobo = (int)order.TotalAmount * 100, //total amount
                Email = order.Shopper,
                Reference = GenerateRef().ToString(),
                Currency = "NGN",
                CallbackUrl = "https://localhost:7061/swagger/index.html"
            };
            
            return _paystackApi.Transactions.Initialize(request);            
        }

        public async Task<TransactionVerifyResponse> VerfiyPaystackPayment(string reference, string orderId)
        {

            TransactionVerifyResponse response = _paystackApi.Transactions.Verify(reference);
            
            var order = await _order.Find(x => x.Id.ToString() == orderId).FirstOrDefaultAsync() ?? throw new Exception("Order not found");

            if (response.Data.Status == "success")
            {
                await _order.UpdateOneAsync(Builders<Order>.Filter.Eq("_id", order.Id),
                    Builders<Order>.Update.Set("has_paid", true));

                var cart = await _cart.Find(x => x.Id.ToString() == order.CartId).FirstOrDefaultAsync();

                foreach (var item in cart.Products)
                {
                    var product = await _product.Find(x => x.Id.ToString() == item.Key).FirstOrDefaultAsync();
                    product.Quantity -= item.Value;

                    await _product.UpdateOneAsync(Builders<Product>.Filter.Eq("_id", product.Id),
                        Builders<Product>.Update.Set("quantity", product.Quantity));

                    await _productService.UpdateInStockStatus(item.Key);
                }
            }
            return response;
        }

        private static int GenerateRef()
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            return random.Next(100000000, 999999999);
        }
    }



}


