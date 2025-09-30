using delivery_order_services.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delivery_order_services.Domain.Repositories.Contracts
{
    public interface IOrderRepository
    {
        Task<List<OrderEntity>> GetAllAsync();

        Task<OrderEntity?> GetByIdAsync(string id);

        Task CreateAsync(OrderEntity orderEntity);
    }
}
