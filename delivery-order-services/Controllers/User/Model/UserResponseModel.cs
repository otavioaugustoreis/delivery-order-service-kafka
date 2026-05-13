using delivery_order_services.Application.Domain.Enum;

namespace delivery_order_services.Controllers.User.Model
{
    public record UserResponseModel(string Name, string Email, UserType UserType);
    
    public static class UserResponseModelExtensions
    {
        public static UserResponseModel ToUserResponseModel(this Application.Domain.User userEntity)
        {
            return new UserResponseModel(
                userEntity.Name,
                userEntity.Email,
                Enum.Parse<UserType>(userEntity.UserType)
            );
        }
    }
}
