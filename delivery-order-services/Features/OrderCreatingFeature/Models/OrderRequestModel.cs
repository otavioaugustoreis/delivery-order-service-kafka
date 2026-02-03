using delivery_order_services.Domain.Entities;
using delivery_order_services.Domain.Entities.Enum;

namespace delivery_order_services.Features.OrderCreatingFeature.Models
{
    public record OrderRequestModel(string ProductName, string Client, OrderStatus OrderStatus);
}
