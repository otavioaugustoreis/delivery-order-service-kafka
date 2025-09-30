using delivery_order_services.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delivery_order_services.Domain.Repositories.Contracts
{
    public interface IUserRepository
    {
         Task<List<UserEntity>> GetAllAsync();

         Task<UserEntity?> GetByIdAsync(string id);

         Task CreateAsync(UserEntity userEntity); 
    }
}
