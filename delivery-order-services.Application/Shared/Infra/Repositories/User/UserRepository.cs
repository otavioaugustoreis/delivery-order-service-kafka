using delivery_order_services.Application.Shared.Infra.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace delivery_order_services.Application.Shared.Infra.Repositories.User
{
    public sealed class UserRepository : MongoDbContext<Domain.User>, IUserRepository    
    {
        private readonly IMongoCollection<Domain.User> _collection;

        public UserRepository(
            IMongoClient client, 
            IOptions<MongoDbConfiguration> configuration) : base(client)
        {
            _collection = GetCollection(configuration.Value.DatabaseName, nameof(Domain.User));
        }

        public async Task<List<Domain.User>> FindAsync(CancellationToken cancellationToken)
            => await _collection.Find(_ => true).ToListAsync(cancellationToken);

        public async Task<Domain.User?> FindByIdAsync(string id, CancellationToken cancellationToken)
            => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

        public async Task InsertOneAsync(Domain.User? userEntity, CancellationToken cancellationToken)
            => await _collection!.InsertOneAsync(userEntity, cancellationToken);
    }
}
