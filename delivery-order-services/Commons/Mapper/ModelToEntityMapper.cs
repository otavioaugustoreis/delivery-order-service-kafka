using delivery_order_services.Domain.Entities;
using delivery_order_services.Features.OrderCreatingFeature.Models;
using delivery_order_services.Features.UserFeatureEvent.Model;
using System.Runtime.CompilerServices;

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
                Client = orderModel.Client,
            };
        }
    }
}
