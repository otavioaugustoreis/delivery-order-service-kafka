using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace delivery_order_services.Application.Shared.Infra.Configuration
{
    public sealed class MongoIndexesHostedService : IHostedService
    {
        private readonly IMongoCollection<Domain.Order> _ordersCollection;
        private readonly ILogger<MongoIndexesHostedService> _logger;

        public MongoIndexesHostedService(
            IMongoCollection<Domain.Order> ordersCollection,
            ILogger<MongoIndexesHostedService> logger)
        {
            _ordersCollection = ordersCollection;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var indexKeys = Builders<Domain.Order>.IndexKeys.Ascending(x => x.IdempotencyKey);

            var indexModel = new CreateIndexModel<Domain.Order>(
                indexKeys,
                new CreateIndexOptions
                {
                    Unique = true,
                    Name = "ux_orders_idempotencyKey"
                });

            try
            {
                await _ordersCollection.Indexes.CreateOneAsync(indexModel, cancellationToken: cancellationToken);

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
