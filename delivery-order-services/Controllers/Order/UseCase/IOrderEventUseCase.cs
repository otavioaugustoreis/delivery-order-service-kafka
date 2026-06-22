using delivery_order_services.Application.Shared.Abstractions.Result;
using delivery_order_services.Controllers.Order.Models;

namespace delivery_order_services.Controllers.Order.UseCase
{
    public interface IOrderEventUseCase
    {
        Task<Result> InsertOneAsync(OrderRequestModel input, string? idempotencyKey,CancellationToken cancellationToken);
        Task<Result<List<Application.Domain.Order>>> FindByClientIdAsync(string clientId, CancellationToken cancellationToken);
    }
}