using delivery_order_services.Application.Domain;

namespace delivery_order_services.Application.Shared.Infra.Repositories.Order
{
    public interface IOrderRepository
    {
        Task<List<Domain.Order>> GetAllAsync(CancellationToken cancellationToken);

        Task<Domain.Order?> GetByIdAsync(string id, CancellationToken cancellationToken);

        Task<bool> CreateAsync(Domain.Order orderEntity, CancellationToken cancellationToken);
    }
}