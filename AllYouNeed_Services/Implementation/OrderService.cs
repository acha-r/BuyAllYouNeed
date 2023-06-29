using AllYouNeed_Models.DTOS.Respoonses;
using AllYouNeed_Models.Models;
using AllYouNeed_Services.Interface;
using MongoDB.Driver;

namespace AllYouNeed_Services.Implementation
{
    public class OrderService : IOrderServices
    {
        private IMongoCollection<Order> _order;
        private readonly ICartServices _cartServices;
        private IMongoDatabase _database;
        private IMongoCollection<Cart> _cart;


        public OrderService(ICartServices cartServices, IMongoCollection<Order> order, IMongoDatabase database, IMongoCollection<Cart> cart)
        {
            _cartServices = cartServices;
            _order = order;
            _database = database;
            _cart = cart;
        }

        public async Task<OrderResponse> CheckOut(string cartId, OrderRequest details)
        {
            var cart = await _cart.Find(x => x.Id.ToString() == cartId).FirstOrDefaultAsync();

            if (cart == null) throw new KeyNotFoundException("You don't have any items in your cart");

            Order order = new()
            {
                CartId = cartId,
                TotalAmount = cart.SubTotal + 3000, //delivery charge
                Address = details.Address,
                State = details.State,
                Shopper = cart.Shopper,
            };

            return new OrderResponse
            {
                OrderId = order.Id.ToString(),
                Address = details.Address,
                State = details.State,
                Total = order.TotalAmount,
                Status = "Pending"
            };

        }
    }
}
