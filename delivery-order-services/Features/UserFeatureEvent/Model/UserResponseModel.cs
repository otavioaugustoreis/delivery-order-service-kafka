using delivery_order_services.Domain.Entities.Enum;

namespace delivery_order_services.Features.UserFeatureEvent.Model
{
    public record UserResponseModel(string Name, string Email, UserType UserType);
}
