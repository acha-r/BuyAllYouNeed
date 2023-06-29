using AllYouNeed_Models.DTOS.Respoonses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllYouNeed_Services.Interface
{
    public interface IOrderServices
    {
        Task<OrderResponse> CheckOut(string cartId, OrderRequest request);
    }
}
