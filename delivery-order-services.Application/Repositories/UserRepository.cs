using delivery_order_services.Application.Domain;
using delivery_order_services.Application.Repositories.Configuration;
using delivery_order_services.Application.Repositories.Contracts;
using MongoDB.Driver;


namespace delivery_order_services.Application.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _collection;

        public UserRepository(MongoDbContext context)
        {
            _collection = context.GetCollection<User>("users");
        }

        public async Task<List<User>> GetAllAsync()
            => await _collection.Find(_ => true).ToListAsync();

        public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken)
            => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

        public async Task CreateAsync(User? userEntity, CancellationToken cancellationToken)
            => await _collection!.InsertOneAsync(userEntity, cancellationToken);
    }
}
