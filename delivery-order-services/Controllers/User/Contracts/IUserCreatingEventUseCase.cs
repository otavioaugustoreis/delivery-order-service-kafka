using delivery_order_services.Commons.ResultPattern;
using delivery_order_services.Controllers.User.Model;
using delivery_order_services.Application.Entities;

namespace delivery_order_services.Controllers.User.Contracts
{
    public interface IUserCreatingEventUseCase
    {
        Task<Result<UserResponseModel>> ExecuteAsync(UserRequestModel input, CancellationToken cancellationToken);
        Task<Result<List<UserEntity>>> GetAllAsync();
    }
}
