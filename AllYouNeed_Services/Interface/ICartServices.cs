using AllYouNeed_Models.DTOS.Requests;
using AllYouNeed_Models.DTOS.Respoonses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllYouNeed_Services.Interface
{
    public interface ICartServices
    {
        Task MakeAnOrder(string email, CartDTO cart);
        Task<CartResponse> UpdateOrder(string cartId, CartDTO cartUpdate);

    }
}
