namespace delivery_order_services.Controllers.User.Model
{
    public record UserRequestModel(string Name, string Email);

    public static class UserResquestModelExtensions
    {
        public static Application.Domain.User ToUserEntity(this UserRequestModel userEntity)
        {
            return new Application.Domain.User
            {
                Name = userEntity.Name,
                Email = userEntity.Email,
            };
        }
    }
}
