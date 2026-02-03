using delivery_order_services.Commons.Mapper;
using delivery_order_services.Features.OrderCreatingFeature.Models;
using delivery_order_services.Features.OrderCreatingFeature.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace delivery_order_services.Features.OrderCreatingFeature
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderEventUseCase _orderEventUseCase;

        public OrderController(IOrderEventUseCase orderEventUseCase)
        {
            _orderEventUseCase = orderEventUseCase;
        }

        [HttpPost("create-order")]
        public async Task<ActionResult> PostCreateEventOrder(
                [FromBody] OrderRequestModel orderRequest
            )
        {
           var order = await _orderEventUseCase.ExecuteAsync(orderRequest.ToOrderEntity());

            return order.IsSuccess
                ? Ok(MessageCommons.PEDIDO_REGISTRADO)
                : BadRequest();
        }
    }
}
