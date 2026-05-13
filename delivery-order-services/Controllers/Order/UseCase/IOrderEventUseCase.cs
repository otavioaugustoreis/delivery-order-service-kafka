using delivery_order_services.Commons.ResultPattern;

namespace delivery_order_services.Controllers.Order.UseCase
{
    public interface IOrderEventUseCase
    {
        Task<Result> ExecuteAsync(Application.Domain.Order entity, CancellationToken cancellationToken);
        Task<Result<List<Application.Domain.Order>>> GetAllAsync(CancellationToken cancellationToken);
    }
}
