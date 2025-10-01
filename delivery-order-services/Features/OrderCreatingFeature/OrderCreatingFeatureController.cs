using delivery_order_services.Features.OrderCreatingFeature.Models;
using delivery_order_services.Features.OrderFeatureEvent.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace delivery_order_services.Features.OrderCreatingFeature
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrderCreatingFeatureController : ControllerBase
    {
        private readonly IOrderEventUseCase _orderEventUseCase;

        public OrderCreatingFeatureController(IOrderEventUseCase orderEventUseCase)
        {
            _orderEventUseCase = orderEventUseCase;
        }

        [HttpPost]
        public ActionResult PostCreateEventOrder(
                [FromBody] OrderRequestModel orderRequest
            )
        {
            var orderCreated = _orderEventUseCase.ExecuteAsync(orderRequest);

            return Created();
        }
    }
}
