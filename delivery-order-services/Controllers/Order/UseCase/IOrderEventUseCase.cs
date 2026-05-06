using delivery_order_services.Commons.ResultPattern;
using delivery_order_services.Domain.Entities;

namespace delivery_order_services.Controllers.Order.UseCase
{
    public interface IOrderEventUseCase
    {
        Task<Result> ExecuteAsync(OrderEntity orderRequestModel);
    }
}
