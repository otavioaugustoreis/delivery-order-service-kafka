using delivery_order_services.Application.Shared.Infra.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace delivery_order_services.Application.Shared.Infra.Repositories.Order
{
    public class OrderRepository : MongoDbContext<Domain.Order>, IOrderRepository
    {
        private readonly IMongoCollection<Domain.Order> _collection;

        public OrderRepository(
            IMongoClient client, 
            IOptions<MongoDbConfiguration> configuration) : base(client)
        {
            _collection = GetCollection(configuration.Value.DatabaseName, nameof(Domain.Order));
        }

        public async Task<bool> CreateAsync(Domain.Order orderEntity, CancellationToken cancellationToken)
        {
            try
            {
                await _collection.InsertOneAsync(orderEntity, cancellationToken: cancellationToken);
                return true;
            }
            catch (MongoWriteException ex) when (ex.WriteError?.Category == ServerErrorCategory.DuplicateKey)
            {
                return false;
            }
        }

        public async Task<List<Domain.Order>> GetAllAsync(CancellationToken cancellationToken)
            => await _collection.Find(_ => true).ToListAsync(cancellationToken);

        public async Task<Domain.Order?> GetByIdAsync(string id, CancellationToken cancellationToken)
             => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }
}
