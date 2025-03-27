using MongoDB.Driver;
using OrderServices;
using OrderServices.Models;

namespace OrderServices.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            _database = client.GetDatabase("order_management");  // Name of the database
        }

        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");
    }
}
