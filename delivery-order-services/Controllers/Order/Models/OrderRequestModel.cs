namespace delivery_order_services.Controllers.Order.Models
{
    public record OrderRequestModel(string ProductName, string Client);

    public static class OrderRequestModelExtensions
    {
        public static Application.Domain.Order ToOrderEntity(this OrderRequestModel orderModel, string idempotencyKey)
        {
            return new Application.Domain.Order
            {
                ProductName = orderModel.ProductName,
                ClientId = orderModel.Client,
                IdempotencyKey = idempotencyKey
            };
        }
    }
}