using AllYouNeed_Models.Interface;
using AllYouNeed_Models.Models;
using AllYouNeed_Services.Interface;
using MongoDB.Driver;

namespace AllYouNeed_Services.Implementation
{
    public class MerchantService : IMerchantServices
    {
        private readonly IMongoCollection<Merchant> _merchants;

        public MerchantService(IAllYouNeedRepo dbSetting, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(dbSetting.DatabaseName);
            _merchants = database.GetCollection<Merchant>("Merchants");
        }
        public async Task<bool> ToggleActiveStatus(string id)
        {
            var merchant = await _merchants.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (merchant is not null)
            {
                merchant.IsActive = !merchant.IsActive;
                return true;
            }
            return false;
        }

        public async Task DeleteMerchant(string id)
            => await _merchants.DeleteOneAsync(x => x.Id == id);

        public async Task<Merchant> GetMerchant(string id)
            => await _merchants.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Merchant>> GetMerchants()
            => await _merchants.Find(x => true).ToListAsync();

        public async Task<Merchant> RegisterMerchant(Merchant merchant)
        {
            await _merchants.InsertOneAsync(merchant);
            return merchant;
        }

        public async Task UpdateMerchantInfo(string id, Merchant merchant)
            => await _merchants.ReplaceOneAsync(x => x.Id == id, merchant);
    }
}
