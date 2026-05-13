using delivery_order_services.Application.Domain;

namespace delivery_order_services.Application.Repositories.Contracts
{
    public interface IUserRepository
    {
         Task<List<User>> GetAllAsync();

         Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken);

         Task CreateAsync(User userEntity, CancellationToken cancellationToken); 
    }
}
