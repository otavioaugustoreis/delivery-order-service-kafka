using delivery_order_services.Controllers.Order.Models;
using delivery_order_services.Controllers.Order.UseCase;
using delivery_order_services.ServicesCollectionExtensions;
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
           var result = await _orderEventUseCase.InsertOneAsync(orderRequest, idempotencyKey, cancellationToken);

           return result.ToActionResult();
        }

        [HttpGet("client/{clientId}")]
        public async Task<ActionResult> GetOrdersByClientIdAsync(string clientId, CancellationToken cancellationToken)
        {
            var result = await _orderEventUseCase.FindByClientIdAsync(clientId ,cancellationToken);

            return result.ToActionResult();
        }
    }
}
