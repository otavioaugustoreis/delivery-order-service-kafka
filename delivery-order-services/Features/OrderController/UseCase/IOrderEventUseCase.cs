using delivery_order_services.Commons.ResultPattern;
using delivery_order_services.Domain.Entities;

namespace delivery_order_services.Features.OrderController.UseCase
{
    public interface IOrderEventUseCase
    {
        Task<Result> ExecuteAsync(OrderEntity orderRequestModel);
    }
}
