using AllYouNeed_Models.Interface;
using AllYouNeed_Models.Models;
using AllYouNeed_Services.Interface;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AllYouNeed_Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _products;
        private IMongoDatabase _database;

        public ProductService(IAllYouNeedRepo dbSetting, IMongoClient mongoClient)
        {
            _database = mongoClient.GetDatabase(dbSetting.DatabaseName);
            _products = _database.GetCollection<Product>("Products");
        }
        public async Task DeleteProduct(string id)
            => await _products.DeleteOneAsync(x => x.Id.ToString() == id);

        public async Task<Product> GetProductById(string id)
           => await _products.Find(x => x.Id.ToString() == id).FirstOrDefaultAsync();

        public async Task<Product> RegisterProduct(Product product)
        {
            await _products.InsertOneAsync(product);
            return product;
        }

        public async Task<List<Product>> GetProductBySearch(string keyword)
        {
            var keys = Builders<Product>.IndexKeys.Text("name");
            var indexOptions = new CreateIndexOptions { DefaultLanguage = "english" };
            var model = new CreateIndexModel<Product>(keys, indexOptions);
            _products.Indexes.CreateOne(model);

            var filter = Builders<Product>.Filter.Regex("name", new BsonRegularExpression($".*{keyword}.*", "i"));
            var searchResults = await _products.Find(filter).ToListAsync();

            return searchResults;
        }

        public async Task UpdateInStockStatus(string id)
        {
            var product = await _products.Find(x => x.Id.ToString() == id).FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Product does not exist");

            if (product.Quantity == 0)
            {
                product.InStock = false;

                return;
            }
        }

        public async Task UpdateProductInfo(string id, Product product)
        => await _products.ReplaceOneAsync(x => x.Id.ToString() == id, product);

    }
}
