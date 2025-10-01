using delivery_order_services.Domain.Entities.Enum;

namespace delivery_order_services.Features.UserFeatureEvent.Model
{
    public record UserRequestModel(string Name, string Email ,UserType UserType)
    {
        public static string GetUserType(UserType userType) => userType.ToString();
    };
}
