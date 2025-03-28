using Microsoft.AspNetCore.Mvc;
using RabbitMQ.OrderServices.Publisher.Models;
using RabbitMQ.OrderServices.Publisher.Services;

namespace RabbitMQ.OrderServices.Publisher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly RabbitMqService _rabbitMqService;

        public OrderController(RabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Order data is missing.");
            }

            try
            {
                _rabbitMqService.PublishOrder(order);
                return Ok($"Order {order.OrderId} sent to RabbitMQ.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Exception occurred while processing your order {order.OrderId}");
            }

        }
    }
}
