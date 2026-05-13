using delivery_order_services.Application.Domain;

namespace delivery_order_services.Application.Shared.Infra.Repositories.User
{
    public interface IUserRepository
    {
         Task<List<Domain.User>> GetAllAsync();

         Task<Domain.User?> GetByIdAsync(string id, CancellationToken cancellationToken);

         Task CreateAsync(Domain.User userEntity, CancellationToken cancellationToken); 
    }
}
