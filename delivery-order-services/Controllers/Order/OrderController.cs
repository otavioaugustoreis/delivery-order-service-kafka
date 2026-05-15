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

        [HttpPost("create")]
        public async Task<ActionResult> PostCreateEventOrderAsync(
                [FromBody] OrderRequestModel orderRequest,
                [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey,
                CancellationToken cancellationToken)
        {
           var order = await _orderEventUseCase.InsertOneAsync(orderRequest, idempotencyKey, cancellationToken);

            return order.IsSuccess
                ? Accepted()
                : BadRequest(order.ErrorMessage);
        }

        [HttpGet("client/{clientId}")]
        public async Task<ActionResult> GetOrdersByClientIdAsync(string clientId,CancellationToken cancellationToken)
        {
            var result = await _orderEventUseCase.FindByClientIdAsync(clientId ,cancellationToken);
            
            if(result.Content is null)
                return NoContent();

            return result.IsSuccess
                ? Ok(result.Content)
                : BadRequest(result.ErrorMessage);
        }
    }
}
