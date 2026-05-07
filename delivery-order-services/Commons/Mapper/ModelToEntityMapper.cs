using delivery_order_services.Application.Entities;
using delivery_order_services.Controllers.Order.Models;
using delivery_order_services.Controllers.User.Model;


namespace delivery_order_services.Commons.Mapper
{
    public static class ModelToEntityMapper
    {
        public static UserEntity ToUserEntity(this UserRequestModel userModel)
        {
            return new UserEntity
            {
                Name = userModel.Name,
                Email = userModel.Email,
                UserType = userModel.UserType.ToString(),
            };
        }

        public static OrderEntity ToOrderEntity(this OrderRequestModel orderModel)
        {
            return new OrderEntity
            {
                ProductName = orderModel.ProductName,
                OrderStatus = orderModel.OrderStatus.ToString(),
                ClientId = orderModel.Client,
            };
        }
    }
}
