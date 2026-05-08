using delivery_order_services.Commons.Mapper;
using delivery_order_services.Controllers.Order.Models;
using delivery_order_services.Controllers.Order.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace delivery_order_services.Controllers.Order
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderEventUseCase _orderEventUseCase;

        public OrderController(IOrderEventUseCase orderEventUseCase)
        {
            _orderEventUseCase = orderEventUseCase;
        }

        [HttpPost("create-order")]
        public async Task<ActionResult> PostCreateEventOrderAsync(
                [FromBody] OrderRequestModel orderRequest, CancellationToken cancellationToken)
        {
           var order = await _orderEventUseCase.ExecuteAsync(orderRequest.ToOrderEntity(), cancellationToken);

            return order.IsSuccess
                ? NoContent()
                : BadRequest(order.ErrorMessage);
        }

        [HttpGet("get-all-orders")]
        public async Task<ActionResult> GetAllOrders(CancellationToken cancellationToken)
        {
                        
            var result = await _orderEventUseCase.GetAllAsync(cancellationToken);
            
            return result.IsSuccess
                ? Ok(result.Content)
                : BadRequest(result.ErrorMessage);
        }
    }
}
