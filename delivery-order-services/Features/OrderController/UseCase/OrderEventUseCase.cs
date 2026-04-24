using delivery_order_services.Commons.ResultPattern;
using delivery_order_services.Domain.Entities;
using delivery_order_services.Domain.Repositories.Contracts;
using delivery_order_services.Features.OrderCreatingFeature.Models;
using System.Reflection;

namespace delivery_order_services.Features.OrderCreatingFeature.UseCase
{
    public class OrderEventUseCase(
        IOrderRepository orderRepository,
        ILogger<OrderEventUseCase> logger) : IOrderEventUseCase
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ILogger<OrderEventUseCase> _logger = logger;

        public async Task<Result<bool>>ExecuteAsync(OrderEntity orderRequestModel)
        {
            try
            {
                return Result<bool>.Success(true);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred in method {MethodName}. Exception Type: {ExceptionType}", nameof(ExecuteAsync), ex.GetType().Name);

                throw;
            }
        }
    }
}
