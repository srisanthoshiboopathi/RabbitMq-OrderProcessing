using RabbitMQ.Client;
using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.OrderServices.Publisher.Models;

namespace RabbitMQ.OrderServices.Publisher.Services
{
    public class RabbitMqService
    {
        private readonly string _hostname;

        public RabbitMqService(string hostname)
        {
            _hostname = hostname;
        }

        public void PublishOrder(Order order)
        {
            var factory = new ConnectionFactory() { HostName = _hostname };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "order_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var orderJson = JsonConvert.SerializeObject(order);
                var body = Encoding.UTF8.GetBytes(orderJson);

                channel.BasicPublish(exchange: "",
                                     routingKey: "order_queue",
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine($"Order {order.OrderId} sent to RabbitMQ.");
            }
        }
    }
}
