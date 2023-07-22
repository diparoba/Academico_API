using MongoDB.Driver;

namespace ServiceAuth_API
{
    public class MongoDBRepository
    {
        public MongoClient client;
        public IMongoDatabase database;
        private readonly IConfiguration _config;
        public MongoDBRepository(IConfiguration config)
        {
            _config = config;
            client = new MongoClient(_config.GetConnectionString("Client"));
            database = client.GetDatabase(_config.GetConnectionString("DbName"));
        }
    }
}