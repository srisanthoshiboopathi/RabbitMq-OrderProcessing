using RabbitMQ.Client;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Text;
using OrderServices.Models;

namespace OrderServices
{
    public class OrderServiceRabbitMQ
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderServiceRabbitMQ(IMongoDatabase database)
        {
            _orders = database.GetCollection<Order>("Orders");
        }

        public void PlaceOrder(Order order)
        {
            // Store the order in MongoDB
            _orders.InsertOne(order);
            Console.WriteLine($"Order {order.OrderId} placed.");

            // Publish order to RabbitMQ
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnectionAsync();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "payment_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var message = JsonConvert.SerializeObject(order);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: "payment_queue", basicProperties: null, body: body);
            Console.WriteLine($"[x] Sent order {order.OrderId} for payment processing.");
        }
    }
}