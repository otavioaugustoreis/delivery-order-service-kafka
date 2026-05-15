using delivery_order_services.Application.Domain;

namespace delivery_order_services.Application.Shared.Infra.Repositories.User
{
    public interface IUserRepository
    {
         Task<List<Domain.User>> FindAsync(CancellationToken cancellationToken);

         Task<Domain.User?> FindByIdAsync(string id, CancellationToken cancellationToken);

         Task InsertOneAsync(Domain.User userEntity, CancellationToken cancellationToken); 
    }
}