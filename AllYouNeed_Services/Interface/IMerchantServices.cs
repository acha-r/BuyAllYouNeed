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
        Task<Merchant> GetMerchant(string id);
        Task<Merchant> RegisterMerchant(Merchant merchant);
        Task UpdateMerchantInfo(string id, Merchant merchant);
        Task<bool> ToggleActiveStatus(string id);
        Task DeleteMerchant(string id);

    }
}
