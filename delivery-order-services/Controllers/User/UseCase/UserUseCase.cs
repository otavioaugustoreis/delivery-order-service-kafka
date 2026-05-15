using delivery_order_services.Commons.ResultPattern;
using delivery_order_services.Controllers.User.Model;
using delivery_order_services.Application.Shared.Infra.Repositories.User;

namespace delivery_order_services.Controllers.User.UseCase
{
    public class UserUseCase(
        IUserRepository userRepository,
        ILogger<UserUseCase> logger
        ) : IUserUseCase
    {

        private readonly ILogger<UserUseCase> _logger = logger;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result> InsertOneAsync(UserRequestModel userRequest, CancellationToken cancellationToken)
        {
            try
            {
                var userEntity = userRequest.ToUserEntity();

                userEntity.SetClient();

				await _userRepository.InsertOneAsync(userEntity,cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"[{Type}] An error occurred. Input: {@Input}",
                    nameof(UserUseCase),
                    new
                    {
                        Method = nameof(InsertOneAsync)
                    });

                return Result.Failed($"An error occurred in the class {nameof(UserUseCase)}");
            }
        }

        public async Task<Result<List<Application.Domain.User>>> FindAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userRepository.FindAsync(cancellationToken);

                return Result<List<Application.Domain.User>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{Type}] An error occurred. Input: {@Input}",
                    nameof(UserUseCase),
                    new
                    {
                        Method = nameof(FindAsync)
                    });

                return Result<List<Application.Domain.User>>.Failed($"An error occurred in the class {nameof(UserUseCase)}");
            }   
        }
    }
}