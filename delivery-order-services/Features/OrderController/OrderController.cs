using delivery_order_services.Commons.Mapper;
using delivery_order_services.Features.OrderController.Models;
using delivery_order_services.Features.OrderController.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace delivery_order_services.Features.OrderController
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
        public async Task<ActionResult> PostCreateEventOrder(
                [FromBody] OrderRequestModel orderRequest
            )
        {
           var order = await _orderEventUseCase.ExecuteAsync(orderRequest.ToOrderEntity());

            return order.IsSuccess
                ? Ok()
                : BadRequest();
        }
    }
}
