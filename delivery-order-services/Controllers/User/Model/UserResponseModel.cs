using delivery_order_services.Application.Entities.Enum;

namespace delivery_order_services.Controllers.User.Model
{
    public record UserResponseModel(string Name, string Email, UserType UserType);
}
