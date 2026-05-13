using delivery_order_services.Application.Domain;
using delivery_order_services.Controllers.Order.Models;

namespace delivery_order_services.Commons.Mapper
{
    public static class ModelToEntityMapper
    {
        public static Order ToOrderEntity(this OrderRequestModel orderModel, string idempotencyKey)
        {
            return new Order
            {
                ProductName = orderModel.ProductName,
                ClientId = orderModel.Client,
                IdempotencyKey = idempotencyKey
            };
        }
    }
}
