using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RabbitMQ.OrderServices.Subscriber.Models
{
    public class Order
    {
        [BsonId]
        public ObjectId Id { get; set; }  // MongoDB will generate this automatically
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }
        public string Status { get; set; }
    }
}
