using MongoDB.Driver;
using OrderServices.Models;
using OrderServices.Data;

namespace OrderServices.Services
{
    public class OrderService
    {
        private readonly MongoDbContext _context;

        public OrderService(MongoDbContext context)
        {
            _context = context;
        }

        // Create Order
        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _context.Orders.InsertOneAsync(order);
            return order;
        }

        // Get Order by OrderId
        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await _context.Orders.Find(o => o.OrderId == orderId).FirstOrDefaultAsync();
        }

        // Update Order Status
        public async Task<Order> UpdateOrderStatusAsync(string orderId, string status)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.OrderId, orderId);
            var update = Builders<Order>.Update.Set(o => o.Status, status);
            var updatedOrder = await _context.Orders.FindOneAndUpdateAsync(filter, update);
            return updatedOrder;
        }
    }
}
