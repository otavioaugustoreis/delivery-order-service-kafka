using delivery_order_services.Application.Entities;

namespace delivery_order_services.Application.Repositories.Contracts
{
    public interface IOrderRepository
    {
        Task<List<OrderEntity>> GetAllAsync();

        Task<OrderEntity?> GetByIdAsync(string id);

        Task CreateAsync(OrderEntity orderEntity);
    }
}
