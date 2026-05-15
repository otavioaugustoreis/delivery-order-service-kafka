using delivery_order_services.Application.Shared.Infra.Repositories.Order;
using delivery_order_services.Commons.ResultPattern;
using delivery_order_services.Controllers.Order.Models;
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

        public async Task<Result> InsertOneAsync(OrderRequestModel input, string idempotencyKey, CancellationToken cancellationToken)
        {
            try
            {
                var orderEntity = input.ToOrderEntity(idempotencyKey);

                var orderEnvelope = new OrderEnvelope
                {
                    Value = orderEntity
                };

                orderEntity.SetCreated();

                await _orderRepository.InsertOneAsync(orderEntity, cancellationToken);

                _logger.LogInformation("[{Type}] Order created successfully. Input:{@input}",
                    nameof(OrderEventUseCase),
                    new 
                    {
                        Product = orderEntity.ProductName
                    });

                await _orderProducer.HandleAsync(orderEnvelope, cancellationToken);

                _logger.LogInformation("[{Type}] Order processed successfully.", nameof(OrderEventUseCase));

                return Result.Success();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex,
                    "[{Type}] An error occurred. Input:{@input}", 
                    nameof(InsertOneAsync),
                    new
                    {
                        input.ProductName
                    });

                return Result.Failed($"An error occurred in method: {nameof(InsertOneAsync)}");
            }
        }
		public async Task<Result<List<Application.Domain.Order?>?>> FindByClientIdAsync(string clientId, CancellationToken cancellationToken)
		{
			try
			{
				var result = await _orderRepository.FindByClientIdAsync(clientId,cancellationToken);

				return Result<List<Application.Domain.Order?>?>.Success(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "[{Type}] An error occurred.",
					nameof(OrderEventUseCase));

				return Result<List<Application.Domain.Order?>?>.Failed($"An error occurred in the class {nameof(OrderEventUseCase)}");
			}
		}
	}
}
