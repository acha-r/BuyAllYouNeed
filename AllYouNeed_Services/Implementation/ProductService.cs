using AllYouNeed_Models.DTOS.Requests;
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


        public async Task<ProductRegistration> RegisterProduct(ProductRegistration product)
        {
            if (product.Price <= 0 || product.Quantity <= 0) throw new Exception("Value must be greater than zero");

            await _products.InsertOneAsync(new Product
            {
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity
            });
            return product;
        }

        public async Task<List<ProductRegistration>> GetProductBySearch(string keyword)
        {
            var keys = Builders<Product>.IndexKeys.Text("name");
            var indexOptions = new CreateIndexOptions { DefaultLanguage = "english" };
            var model = new CreateIndexModel<Product>(keys, indexOptions);
            _products.Indexes.CreateOne(model);

            var filter = Builders<Product>.Filter.Regex("name", new BsonRegularExpression($".*{keyword}.*", "i"));
            var searchResult = await _products.Find(filter).ToListAsync();

            searchResult.RemoveAll(x => x.InStock == false);
            

            var product = new List<ProductRegistration>();

            foreach (var item in searchResult)
            {
                product.Add(new ProductRegistration
                {
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity
                });
            }
            return product;
        }

        public async Task<bool> UpdateInStockStatus(string id)
        {
            var product = await _products.Find(x => x.Id.ToString() == id).FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Product does not exist");

            if (product.Quantity <= 0)
            {
                product.InStock = false;

                return false;
            }
            return true;
        }

        public async Task UpdateProductInfo(string id, ProductRegistration product)
        {
            var prod = await _products.Find(x => x.Id.ToString() == id).FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Product does not exist");
            prod.Name = product.Name;
            prod.Price = product.Price;
            prod.Quantity = product.Quantity;
            prod.InStock = await UpdateInStockStatus(id);

            await _products.ReplaceOneAsync(x => x.Id.ToString() == id, prod);
        }

        public async Task DeleteProduct(string id)
                => await _products.DeleteOneAsync(x => x.Id.ToString() == id);

        public async Task<ProductRegistration> GetProductById(string id)
        {
            var prod = await _products.Find(x => x.Id.ToString() == id).FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Product does not exist");
            if (!prod.InStock) throw new Exception($"{prod.Name} is out of stock");

            return new ProductRegistration
            {
                Name = prod.Name,
                Price = prod.Price,
                Quantity = prod.Quantity
            };
        }
    }
}
