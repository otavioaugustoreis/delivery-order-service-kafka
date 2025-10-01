using delivery_order_services.Features.OrderCreatingFeature.Models;
using delivery_order_services.ResultPattern;

namespace delivery_order_services.Features.OrderFeatureEvent.Contracts
{
    public interface IOrderEventUseCase
    {
        Task ExecuteAsync(OrderRequestModel orderRequestModel);
    }
}
