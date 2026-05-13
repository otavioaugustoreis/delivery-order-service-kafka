using delivery_order_services.Application.Domain;

namespace delivery_order_services.Application.Repositories.Contracts
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync(CancellationToken cancellationToken);

        Task<Order?> GetByIdAsync(string id, CancellationToken cancellationToken);

        Task<bool> CreateAsync(Order orderEntity, CancellationToken cancellationToken);
    }
}
