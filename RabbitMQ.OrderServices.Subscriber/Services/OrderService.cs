using RabbitMQ.OrderServices.Subscriber.Data;
using RabbitMQ.OrderServices.Subscriber.Models;
using RabbitMQ.OrderServices.Subscriber.Services;
using Newtonsoft.Json;

namespace RabbitMQ.OrderServices.Subscriber.Services
{
    public class OrderService
    {
        private readonly RabbitMqService _rabbitMqService;
        private readonly MongoDbContext _mongoDbContext;

        public OrderService(RabbitMqService rabbitMqService, MongoDbContext mongoDbContext)
        {
            _rabbitMqService = rabbitMqService;
            _mongoDbContext = mongoDbContext;
        }

        public void StartListening()
        {
            // Listening for messages from the RabbitMQ queue
            _rabbitMqService.ReceiveMessage("order_queue", async (message) =>
            {
                var order = JsonConvert.DeserializeObject<Order>(message);

                // Save the order to MongoDB
                await _mongoDbContext.Orders.InsertOneAsync(order);

                // Simulate processing and logging the order
                Console.WriteLine($"Order {order.OrderId} saved to MongoDB.");
            });
        }
    }
}
