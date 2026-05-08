using delivery_order_services.Application.Entities;

namespace delivery_order_services.Application.Repositories.Contracts
{
    public interface IOrderRepository
    {
        Task<List<OrderEntity>> GetAllAsync(CancellationToken cancellationToken);

        Task<OrderEntity?> GetByIdAsync(string id, CancellationToken cancellationToken);

        Task CreateAsync(OrderEntity orderEntity, CancellationToken cancellationToken);
    }
}
