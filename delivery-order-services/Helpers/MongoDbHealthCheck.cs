using delivery_order_services.Application.Shared.Infra.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace delivery_order_services.Helpers
{
    public sealed class MongoDbHealthCheck : IHealthCheck
    {
        private readonly IMongoClient _mongoClient;
        private readonly MongoDbConfiguration _configuration;

        public MongoDbHealthCheck(IMongoClient mongoClient, IOptions<MongoDbConfiguration> configuration)
        {
            _mongoClient = mongoClient;
            _configuration = configuration.Value;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var database = _mongoClient.GetDatabase(_configuration.DatabaseName);
            await database.RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1), cancellationToken: cancellationToken);

            return HealthCheckResult.Healthy("MongoDB is available.");
        }
    }
}