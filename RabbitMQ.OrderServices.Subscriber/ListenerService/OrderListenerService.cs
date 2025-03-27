using Microsoft.Extensions.Hosting;
using RabbitMQ.OrderServices.Subscriber.Services;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.OrderServices.Subscriber.ListenerService
{
    public class OrderListenerService : BackgroundService
    {
        private readonly OrderService _orderService;

        public OrderListenerService(OrderService orderService)
        {
            _orderService = orderService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _orderService.StartListening();  // Start receiving messages from RabbitMQ
            return Task.CompletedTask;
        }
    }
}
