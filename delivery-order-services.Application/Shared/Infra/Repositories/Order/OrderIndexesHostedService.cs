using delivery_order_services.Application.Shared.Infra.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace delivery_order_services.Application.Shared.Infra.Repositories.Order
{
    public sealed class OrderIndexesHostedService : MongoDbContext<Domain.Order>, IHostedService
    {
        private readonly IMongoCollection<Domain.Order> _ordersCollection;
        private readonly ILogger<OrderIndexesHostedService> _logger;
        private const string IndexName = "ux_orders_idempotencyKey";

        public OrderIndexesHostedService(
            IMongoClient client,
            ILogger<OrderIndexesHostedService> logger,
            IOptions<MongoDbConfiguration> configuration) : base(client)
        {
            _ordersCollection =  GetCollection(configuration.Value.DatabaseName, nameof(Domain.Order));
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var cursor = await _ordersCollection.Indexes.ListAsync(cancellationToken);

            var indexes = await cursor.ToListAsync(cancellationToken);

            var exists = indexes.Any(d =>
                d.TryGetValue("name", out var name) &&
                name.AsString == IndexName);

            if(exists)
                return;

            var indexKeys = Builders<Domain.Order>.IndexKeys.Ascending(x => x.IdempotencyKey);

            var indexModel = new CreateIndexModel<Domain.Order>(
                indexKeys,
                new CreateIndexOptions
                {
                    Unique = true,
                    Sparse = true,
                    Name = IndexName
                });

            try
            {
                await _ordersCollection.Indexes.CreateOneAsync(indexModel, cancellationToken: cancellationToken);

                _logger.LogInformation("[MongoIndexes] Índice criado/garantido: {IndexName}", IndexName);
            }
            catch (MongoCommandException ex) when (ex.CodeName is "IndexOptionsConflict" or "IndexKeySpecsConflict")
            {
                _logger.LogWarning(ex, "[MongoIndexes] Conflito ao criar índice {IndexName}. Verifique o índice existente.", IndexName);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
