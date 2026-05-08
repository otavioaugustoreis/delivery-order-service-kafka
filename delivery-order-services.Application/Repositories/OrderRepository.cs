using delivery_order_services.Application.Entities;
using delivery_order_services.Application.Repositories.Configuration;
using delivery_order_services.Application.Repositories.Contracts;
using MongoDB.Driver;

namespace delivery_order_services.Application.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<OrderEntity> _collection;

        public OrderRepository(MongoDbContext context)
        {
            _collection = context.GetCollection<OrderEntity>("orders");
        }

        public async Task CreateAsync(OrderEntity orderEntity, CancellationToken cancellationToken)
             => await _collection.InsertOneAsync(orderEntity, cancellationToken: cancellationToken);

        public async Task<List<OrderEntity>> GetAllAsync(CancellationToken cancellationToken)
            => await _collection.Find(_ => true).ToListAsync(cancellationToken);

        public async Task<OrderEntity?> GetByIdAsync(string id, CancellationToken cancellationToken)
             => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }
}
