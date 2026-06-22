using delivery_order_services.Application.Shared.Abstractions.Result;
using delivery_order_services.Controllers.User.Model;

namespace delivery_order_services.Controllers.User.UseCase
{
    public interface IUserUseCase
    {
        Task<Result> InsertOneAsync(UserRequestModel input, CancellationToken cancellationToken);
        Task<Result<List<Application.Domain.User>>> FindAsync(CancellationToken cancellationToken);
    }
}
