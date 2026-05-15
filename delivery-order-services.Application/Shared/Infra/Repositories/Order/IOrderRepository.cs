using delivery_order_services.Application.Domain;

namespace delivery_order_services.Application.Shared.Infra.Repositories.Order
{
    public interface IOrderRepository
    {
        Task<List<Domain.Order?>?> FindByClientIdAsync(string id, CancellationToken cancellationToken);

        Task<bool> InsertOneAsync(Domain.Order orderEntity, CancellationToken cancellationToken);
    }
}