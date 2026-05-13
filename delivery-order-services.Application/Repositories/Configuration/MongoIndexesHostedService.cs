using delivery_order_services.Application.Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace delivery_order_services.Application.Repositories.Configuration
{
    public sealed class MongoIndexesHostedService : IHostedService
    {
        private readonly MongoDbContext _mongoDbContext;
        private readonly ILogger<MongoIndexesHostedService> _logger;

        public MongoIndexesHostedService(
            MongoDbContext mongoDbContext,
            ILogger<MongoIndexesHostedService> logger)
        {
            _mongoDbContext = mongoDbContext;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var orders = _mongoDbContext.GetCollection<Order>("orders");

            var indexKeys = Builders<Order>.IndexKeys.Ascending(x => x.IdempotencyKey);

            var indexModel = new CreateIndexModel<Order>(
                indexKeys,
                new CreateIndexOptions
                {
                    Unique = true,
                    Name = "ux_orders_idempotencyKey"
                });

            try
            {
                await orders.Indexes.CreateOneAsync(indexModel, cancellationToken: cancellationToken);

                _logger.LogInformation("[MongoIndexes] Índice criado/garantido: {IndexName}", "ux_orders_idempotencyKey");
            }
            catch (MongoCommandException ex) when (ex.CodeName is "IndexOptionsConflict" or "IndexKeySpecsConflict")
            {
                _logger.LogWarning(ex, "[MongoIndexes] Conflito ao criar índice {IndexName}. Verifique o índice existente.", "ux_orders_idempotencyKey");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
