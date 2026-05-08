using delivery_order_services.Commons.ResultPattern;
using delivery_order_services.Application.Entities;

namespace delivery_order_services.Controllers.Order.UseCase
{
    public interface IOrderEventUseCase
    {
        Task<Result> ExecuteAsync(OrderEntity entity, CancellationToken cancellationToken);
        Task<Result<List<OrderEntity>>> GetAllAsync(CancellationToken cancellationToken);
    }
}
