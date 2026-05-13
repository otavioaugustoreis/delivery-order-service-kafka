using MongoDB.Driver;


namespace delivery_order_services.Application.Shared.Infra.Repositories.User
{
    public sealed class UserRepository : IUserRepository    
    {
        private readonly IMongoCollection<Domain.User> _collection;

        public UserRepository(IMongoCollection<Domain.User> collection)
        {
            _collection = collection;
        }

        public async Task<List<Domain.User>> GetAllAsync()
            => await _collection.Find(_ => true).ToListAsync();

        public async Task<Domain.User?> GetByIdAsync(string id, CancellationToken cancellationToken)
            => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

        public async Task CreateAsync(Domain.User? userEntity, CancellationToken cancellationToken)
            => await _collection!.InsertOneAsync(userEntity, cancellationToken);
    }
}
