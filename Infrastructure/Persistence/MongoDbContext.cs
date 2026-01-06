using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace testing.Infrastructure.Persistence
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoConnection") 
                ?? "mongodb://admin:Your_password123@localhost:27017";
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("UsersDb");
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
