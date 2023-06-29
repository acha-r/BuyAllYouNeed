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
        Task<CartResponse> GetCartSummary(string cartId);
        Task<string> CreateCart(CartDTO cart);
        Task<string> RemoveFromCart(string cartId, CartDTO cartUpdate); 
        Task<string> AddToCart(string cartId, CartDTO cartUpdate);
        Task DeleteCart(string cartId);

    }
}
