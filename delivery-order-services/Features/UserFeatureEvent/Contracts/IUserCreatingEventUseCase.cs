using delivery_order_services.Features.UserFeatureEvent.Model;
using delivery_order_services.ResultPattern;

namespace delivery_order_services.Features.UserFeatureEvent.Contracts
{
    public interface IUserCreatingEventUseCase
    {
        Task<Result<UserResponseModel>> ExecuteAsync(UserRequestModel userRequest);
    }
}
