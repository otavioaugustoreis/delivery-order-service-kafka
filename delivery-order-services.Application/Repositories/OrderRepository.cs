using delivery_order_services.Application.Domain;
using delivery_order_services.Application.Repositories.Configuration;
using delivery_order_services.Application.Repositories.Contracts;
using MongoDB.Driver;

namespace delivery_order_services.Application.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _collection;

        public OrderRepository(MongoDbContext context)
        {
            _collection = context.GetCollection<Order>("orders");
        }

        public async Task<bool> CreateAsync(Order orderEntity, CancellationToken cancellationToken)
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

        public async Task<List<Order>> GetAllAsync(CancellationToken cancellationToken)
            => await _collection.Find(_ => true).ToListAsync(cancellationToken);

        public async Task<Order?> GetByIdAsync(string id, CancellationToken cancellationToken)
             => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }
}
