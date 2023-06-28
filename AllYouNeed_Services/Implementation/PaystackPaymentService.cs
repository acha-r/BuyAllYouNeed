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
        private IMerchantServices _merchantService;
        private IMongoDatabase _database;
        private readonly IMongoCollection<Product> _products;


        public PaystackPaymentService(IAllYouNeedRepo dbSetting, IMongoClient mongoClient, IConfiguration configuration, IProductService product, IMerchantServices merchant)
        {
            _productService = product;
            _merchantService = merchant;
            _configuration = configuration;
            _token = _configuration["Paystack:SecretKey"];
            _paystackApi = new PayStackApi(_token);
            _database = mongoClient.GetDatabase(dbSetting.DatabaseName);
            _products = _database.GetCollection<Product>("Products");
        }

        public async Task<TransactionInitializeResponse> InitializeTransaction(string productId, string email, int numOfItems = 1)
        {
            var product = await _productService.GetProductById(productId) ?? throw new KeyNotFoundException("Product does not exist");
            var merchant = await _merchantService.GetMerchant(email);
            
            var request = new TransactionInitializeRequest()
            {
                AmountInKobo = (int)(product.Price * numOfItems) * 100 ,
                Email = merchant.Email,
                Reference = GenerateRef().ToString(),
                Currency = "NGN",
                CallbackUrl = "https://localhost:7061/swagger/index.html"
            };
            
            TransactionInitializeResponse response = _paystackApi.Transactions.Initialize(request);
            if (response.Status)
            {
                product.Quantity -= numOfItems;
                //await _products.ReplaceOneAsync(productId., product);
            }
            return response;


        }
        public TransactionVerifyResponse Verfiy(string reference)
        {
            TransactionVerifyResponse response = _paystackApi.Transactions.Verify(reference);
            return response;
        }

        private static int GenerateRef()
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            return random.Next(100000000, 999999999);
        }
    }



}


