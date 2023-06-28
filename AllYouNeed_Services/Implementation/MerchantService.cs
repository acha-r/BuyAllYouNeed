using AllYouNeed_Models.DTOS.Requests;
using AllYouNeed_Models.Interface;
using AllYouNeed_Models.Models;
using AllYouNeed_Services.Interface;
using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace AllYouNeed_Services.Implementation
{
    public class MerchantService : IMerchantServices
    {
        private readonly IMongoCollection<Merchant> _merchants;
        private readonly UserManager<ApplicationUser> _userManager;

        public MerchantService(IAllYouNeedRepo dbSetting, IMongoClient mongoClient,UserManager<ApplicationUser> userManager)
        {
            var database = mongoClient.GetDatabase(dbSetting.DatabaseName);
            _merchants = database.GetCollection<Merchant>("Merchants");
            _userManager = userManager;
        }

        public async Task<string> RegisterMerchant(string email, DepositRequest request)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email) ?? throw new KeyNotFoundException("User does not exist");

            await _merchants.InsertOneAsync(new Merchant
            {
                FullName = user.FullName,
                Email = user.Email,
                Balance = request.Balance
            });
            return "Registration complete";
        }

        public async Task<string> UpdateMerchantInfo(string email, UpdateMerchantRequest request)
        {
            //update application user
            ApplicationUser user = await _userManager.FindByEmailAsync(email) ?? throw new KeyNotFoundException("User does not exist");
            user.FullName = request.FullName;

            await _userManager.UpdateAsync(user);

            //update merchant
            var merchantFilter = Builders<Merchant>.Filter.Eq("email", user.Email);
            var update = Builders<Merchant>.Update.Set("full_name", user.FullName);

            await _merchants.UpdateOneAsync(merchantFilter, update);

            return "Updated";
        }

        public async Task<string> UpdateWallet(string email, DepositRequest request)
        {
            try
            {
                if (request.Balance <= 1000)
                    return "Amount cannot be less than NGN 1000";

                var merchantFilter = Builders<Merchant>.Filter.Eq("email", email);
                var update = Builders<Merchant>.Update.Set("balance", request.Balance);

                await _merchants.UpdateOneAsync(merchantFilter, update);
                return "Deposit complete";
            }
            catch (KeyNotFoundException ex)
            {
                return ($"{ex.Message}\nUser does not exist");
            }

        }

        public async Task ToggleActiveStatus(string email)
        {
            var check = await _merchants.Find(x => x.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync() ?? throw new KeyNotFoundException("User does not exist");
            
            check.IsActive = !check.IsActive;
            var merchantFilter = Builders<Merchant>.Filter.Eq("email", email);
            var update = Builders<Merchant>.Update.Set("is_active", check.IsActive);

            await _merchants.UpdateOneAsync(merchantFilter, update);
        }

        public async Task<Merchant> GetMerchant(string email)
            => await _merchants.Find(x => x.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();

        public async Task<List<Merchant>> GetMerchants()
            => await _merchants.Find(x => true).ToListAsync();     
    }
}
