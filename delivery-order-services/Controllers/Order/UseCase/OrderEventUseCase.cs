using delivery_order_services.Application.Repositories.Contracts;
using delivery_order_services.Commons.ResultPattern;
using delivery_order_services.Producer;
using delivery_order_services.Application.Domain;

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

        public async Task<Result> ExecuteAsync(Application.Domain.Order entity, CancellationToken cancellationToken)
        {
            try
            {
                
                var orderEnvelope = new OrderEnvelope
                {
                    Value = entity
                };

                entity.SetCreated();

                await _orderRepository.CreateAsync(entity, cancellationToken);

                _logger.LogInformation("[{Type}] Order created successfully. Input:{@input}",
                    nameof(OrderEventUseCase),
                    new 
                    {
                        Product = entity.ProductName
                    });

                await _orderProducer.HandleAsync(orderEnvelope, cancellationToken);

                _logger.LogInformation("[{Type}] Order processed successfully.", nameof(OrderEventUseCase));

                return Result.Success();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex,
                    "[{Type}] An error occurred. Input:{@input}", 
                    nameof(ExecuteAsync),
                    new
                    {
                        entity.ProductName
                    });

                return Result.Failed($"An error occurred in method: {nameof(ExecuteAsync)}");
            }
        }
		public async Task<Result<List<Application.Domain.Order>>> GetAllAsync(CancellationToken cancellationToken)
		{
			try
			{
				var result = await _orderRepository.GetAllAsync(cancellationToken);
				return Result<List<Application.Domain.Order>>.Success(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "[{Type}] An error occurred.",
					nameof(OrderEventUseCase));

				return Result<List<Application.Domain.Order>>.Failed($"An error occurred in the class {nameof(OrderEventUseCase)}");
			}
		}
	}
}
