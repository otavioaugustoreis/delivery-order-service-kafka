using delivery_order_services.Commons.ResultPattern;
using delivery_order_services.Domain.Entities;
using delivery_order_services.Features.UserFeatureEvent.Model;

namespace delivery_order_services.Features.UserFeatureEvent.Contracts
{
    public interface IUserCreatingEventUseCase
    {
        Task<Result<UserResponseModel>> ExecuteAsync(UserRequestModel input, CancellationToken cancellationToken);
        Task<Result<List<UserEntity>>> GetAllAsync();
    }
}
