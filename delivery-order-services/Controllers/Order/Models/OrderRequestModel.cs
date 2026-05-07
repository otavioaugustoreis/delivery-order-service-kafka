using delivery_order_services.Application.Entities.Enum;

namespace delivery_order_services.Controllers.Order.Models
{
    public record OrderRequestModel(string ProductName, string Client, OrderStatus OrderStatus);
}
