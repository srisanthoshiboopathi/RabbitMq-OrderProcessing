namespace RabbitMQ.OrderServices.Publisher.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }
    }
}
