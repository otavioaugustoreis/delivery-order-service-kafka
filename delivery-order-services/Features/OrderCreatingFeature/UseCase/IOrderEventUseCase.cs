using delivery_order_services.Commons.ResultPattern;
using delivery_order_services.Domain.Entities;
using delivery_order_services.Features.OrderCreatingFeature.Models;

namespace delivery_order_services.Features.OrderCreatingFeature.UseCase
{
    public interface IOrderEventUseCase
    {
        Task<Result<bool>> ExecuteAsync(OrderEntity orderRequestModel);
    }
}
