using delivery_order_services.Application.Entities;

namespace delivery_order_services.Application.Repositories.Contracts
{
    public interface IUserRepository
    {
         Task<List<UserEntity>> GetAllAsync();

         Task<UserEntity?> GetByIdAsync(string id, CancellationToken cancellationToken);

         Task CreateAsync(UserEntity userEntity, CancellationToken cancellationToken); 
    }
}
