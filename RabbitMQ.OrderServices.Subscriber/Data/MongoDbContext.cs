using MongoDB.Driver;
using RabbitMQ.OrderServices.Subscriber.Models;

namespace RabbitMQ.OrderServices.Subscriber.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            _database = client.GetDatabase("order_management");
        }

        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");
    }
}

