using delivery_order_services.Application.Entities;
using delivery_order_services.Application.Repositories.Contracts;
using delivery_order_services.Commons.ResultPattern;
using delivery_order_services.Producer;

namespace delivery_order_services.Controllers.Order.UseCase
{
    public class OrderEventUseCase : IOrderEventUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderEventUseCase> _logger;
        private readonly IOrderProducer _orderProducer;

        public OrderEventUseCase(
            IOrderRepository orderRepository,
            ILogger<OrderEventUseCase> logger,
            IOrderProducer orderProducer)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _orderProducer = orderProducer;
        }

        public async Task<Result> ExecuteAsync(OrderEntity entity, CancellationToken cancellationToken)
        {
            try
            {
                var orderEnvelope = new OrderEnvelope
                {
                    Value = entity
                };

                await _orderRepository.CreateAsync(entity, cancellationToken);

                _logger.LogInformation("");

                await _orderProducer.HandleAsync(orderEnvelope, cancellationToken);

                _logger.LogInformation("");

                return Result.Success();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, 
                    "An error occurred in method {MethodName}. Input:{@input}", 
                    nameof(ExecuteAsync), new
                    {
                        entity.ProductName
                    });

                return Result.Failed($"An error occurred in method: {nameof(ExecuteAsync)}");
            }
        }
		public async Task<Result<List<OrderEntity>>> GetAllAsync(CancellationToken cancellationToken)
		{
			try
			{
				var result = await _orderRepository.GetAllAsync(cancellationToken);
				return Result<List<OrderEntity>>.Success(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "[{Type}] An error occurred.",
					nameof(OrderEventUseCase));

				return Result<List<OrderEntity>>.Failed($"An error occurred in the class {nameof(OrderEventUseCase)}");
			}
		}
	}
}
