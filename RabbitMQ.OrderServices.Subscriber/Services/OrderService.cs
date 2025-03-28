using RabbitMQ.OrderServices.Subscriber.Data;
using RabbitMQ.OrderServices.Subscriber.Models;
using RabbitMQ.OrderServices.Subscriber.Services;
using Newtonsoft.Json;
using RabbitMQ.OrderServices.Publisher.Services;

namespace RabbitMQ.OrderServices.Subscriber.Services
{
    public class OrderService
    {
        private readonly RabbitMQReceivingServices _rabbitMQReceivingServices;
        private readonly MongoDbContext _mongoDbContext;

        public OrderService(RabbitMQReceivingServices rabbitMQReceivingServices, MongoDbContext mongoDbContext)
        {
            _rabbitMQReceivingServices = rabbitMQReceivingServices;
            _mongoDbContext = mongoDbContext;
        }

        public void StartListening()
        {
            // Listening for messages from the RabbitMQ queue
            _rabbitMQReceivingServices.ReceiveMessage("order_queue", async (message) =>
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
