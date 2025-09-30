using delivery_order_services.Domain.Entities;
using delivery_order_services.Domain.Repositories.Configuration;
using delivery_order_services.Domain.Repositories.Contracts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delivery_order_services.Domain.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<OrderEntity> _collection;

        public OrderRepository(MongoDbContext context)
        {
            _collection = context.GetCollection<OrderEntity>("orders");
        }

        public async Task CreateAsync(OrderEntity orderEntity)
             => await _collection.InsertOneAsync(orderEntity);

        public async Task<List<OrderEntity>> GetAllAsync()
            => await _collection.Find(_ => true).ToListAsync();

        public async Task<OrderEntity?> GetByIdAsync(string id)
             => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }
}
