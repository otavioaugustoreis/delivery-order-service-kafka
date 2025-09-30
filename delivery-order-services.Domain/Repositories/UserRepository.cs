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
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserEntity> _collection;

        public UserRepository(MongoDbContext context)
        {
            _collection = context.GetCollection<UserEntity>("users");
        }

        public async Task<List<UserEntity>> GetAllAsync()
            => await _collection.Find(_ => true).ToListAsync();

        public async Task<UserEntity?> GetByIdAsync(string id)
            => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(UserEntity userEntity)
            => await _collection.InsertOneAsync(userEntity);
    }
}
