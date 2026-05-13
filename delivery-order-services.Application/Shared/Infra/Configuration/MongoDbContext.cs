using MongoDB.Driver;


namespace delivery_order_services.Application.Shared.Infra.Configuration
{
    public class MongoDbContext<T> where T : class
    {
        private readonly IMongoClient _client;

        public MongoDbContext(IMongoClient mongoClient)
        {
            _client = mongoClient;
        }

        public IMongoCollection<T> GetCollection<T>(
            string databaseName, 
            string colletionName,
            MongoDatabaseSettings settings = null!,
            MongoCollectionSettings colletionsSettings = null!)
            => _client.GetDatabase(databaseName, settings).GetCollection<T>(colletionName, colletionsSettings);

    }
}
