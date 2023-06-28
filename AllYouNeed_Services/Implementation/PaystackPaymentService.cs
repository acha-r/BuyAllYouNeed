using AllYouNeed_Models.Interface;
using AllYouNeed_Models.Models;
using AllYouNeed_Services.Interface;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using PayStack.Net;

namespace AllYouNeed_Services.Implementation
{
    public class PaystackPaymentService : IPaystackPaymentService
    {
        private IConfiguration _configuration;
        private readonly string? _token;
        private PayStackApi _paystackApi;
        private IProductService _productService;
        private IMongoDatabase _database;
        private readonly IMongoCollection<Product> _products;


        public PaystackPaymentService(IAllYouNeedRepo dbSetting, IMongoClient mongoClient, IConfiguration configuration, IProductService product)
        {
            _productService = product;
            _configuration = configuration;
            _token = _configuration["Paystack:SecretKey"];
            _paystackApi = new PayStackApi(_token);
            _database = mongoClient.GetDatabase(dbSetting.DatabaseName);
            _products = _database.GetCollection<Product>("Products");
        }

        public async Task<TransactionInitializeResponse> InitializeTransaction(string cartId)
        {
            var cart = await _database.GetCollection<ShoppingCart>("Cart").Find(x => x.Id.ToString() == cartId).FirstOrDefaultAsync() ?? throw new Exception("Order not found");

            
            var request = new TransactionInitializeRequest()
            {
                AmountInKobo = (int)cart.TotalAmt * 100 ,
                Email = cart.BuyerEmail,
                Reference = GenerateRef().ToString(),
                Currency = "NGN",
                CallbackUrl = "https://localhost:7061/swagger/index.html"
            };
            
            return _paystackApi.Transactions.Initialize(request);            
        }

        public async Task<TransactionVerifyResponse> Verfiy(string reference, string cartId)
        {

            TransactionVerifyResponse response = _paystackApi.Transactions.Verify(reference);
            
            var cart = await _database.GetCollection<ShoppingCart>("Cart").Find(x => x.Id.ToString() == cartId).FirstOrDefaultAsync() ?? throw new Exception("Order not found");

            if (response.Data.Status == "success")
            {
                ObjectId id = new(cartId);
                await _database.GetCollection<ShoppingCart>("Cart")
                    .UpdateOneAsync(Builders<ShoppingCart>.Filter.Eq("_id", id),
                    Builders<ShoppingCart>.Update.Set("has_paid", true));

                foreach (var item in cart.Products)
                {
                    var product = _products.Find(x => x.Id.ToString() == item.Key).FirstOrDefault();
                    product.Quantity -= item.Value;

                    await _products.UpdateOneAsync(Builders<Product>.Filter.Eq("_id", product.Id),
                        Builders<Product>.Update.Set("quantity", product.Quantity));

                    await _productService.CheckInStockStatus(item.Key);
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


