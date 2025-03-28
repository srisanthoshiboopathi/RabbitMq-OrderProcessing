using RabbitMQ.Client;
using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.OrderServices.Publisher.Models;
using System.Net.Security;
using RabbitMQ.Client.Events;

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
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _hostname, // Use correct server hostname or IP
                    //Port = 5672, // Default AMQP port
                    UserName = "guest",
                    Password = "guest"
                };



                using (var connection = factory.CreateConnection()) // ✅ Works in v7.x with ClientExtensions
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

                    Console.WriteLine($"✅ Order {order.OrderId} sent to RabbitMQ.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to publish order: {ex.Message}");
            }
        }
        public void ReceiveMessage(string queueName, Func<string, Task> onMessageReceived)
        {
            var factory = new ConnectionFactory() { HostName = _hostname, UserName = "guest", Password = "guest" };

            // Create connection and channel
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Declare the queue (ensure the queue exists)
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                // Create an event-based consumer
                var consumer = new EventingBasicConsumer(channel);

                // Handle the message when it's received
                consumer.Received += async (sender, e) =>
                {
                    var body = e.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    // Process the message (call the provided callback)
                    await onMessageReceived(message);
                };

                // Start consuming messages from the queue
                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                // Keep the application running to continue listening
                Console.WriteLine("Listening for messages. Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
