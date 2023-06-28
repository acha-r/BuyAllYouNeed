using AllYouNeed_Models.DTOS.Requests;
using AllYouNeed_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllYouNeed_Services.Interface
{
    public interface IMerchantServices
    {
        Task<List<Merchant>> GetMerchants();
        Task<Merchant> GetMerchant(string email);
        Task<string> RegisterMerchant(string email, DepositRequest request);
        Task<string> UpdateMerchantInfo(string email, UpdateMerchantRequest merchant);
        Task<string> UpdateWallet(string email, DepositRequest request);
        Task ToggleActiveStatus(string email);
    }
}
