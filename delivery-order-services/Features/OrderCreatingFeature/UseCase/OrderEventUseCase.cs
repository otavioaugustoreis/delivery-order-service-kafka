using delivery_order_services.Domain.Repositories.Contracts;
using delivery_order_services.Features.OrderCreatingFeature.Models;
using delivery_order_services.Features.OrderFeatureEvent.Contracts;

namespace delivery_order_services.Features.OrderCreatingFeature.UseCase
{
    public class OrderEventUseCase : IOrderEventUseCase
    {
        private readonly IOrderRepository orderRepository;

        public OrderEventUseCase(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public Task ExecuteAsync(OrderRequestModel orderRequestModel)
        {
            throw new NotImplementedException();
        }
    }
}
