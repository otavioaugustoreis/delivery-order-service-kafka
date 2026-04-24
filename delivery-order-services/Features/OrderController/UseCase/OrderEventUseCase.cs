using Confluent.Kafka;
using delivery_order_services.Commons.ResultPattern;
using delivery_order_services.Domain.Entities;
using delivery_order_services.Domain.Repositories.Contracts;
using delivery_order_services.Producer;


namespace delivery_order_services.Features.OrderController.UseCase
{
    public class OrderEventUseCase(
        IOrderRepository orderRepository,
        ILogger<OrderEventUseCase> logger) : IOrderEventUseCase
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ILogger<OrderEventUseCase> _logger = logger;
        private readonly IOrderProducer _orderProducer;

        public async Task<Result>ExecuteAsync(OrderEntity orderRequestModel)
        {
            try
            {
                return Result.Success();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, 
                    "An error occurred in method {MethodName}. Input:{@input}", 
                    nameof(ExecuteAsync), new
                    {
                        orderRequestModel.ProductName
                    });

                return Result.Failed($"An error occurred in method: {nameof(ExecuteAsync)}");
            }
        }
    }
}
