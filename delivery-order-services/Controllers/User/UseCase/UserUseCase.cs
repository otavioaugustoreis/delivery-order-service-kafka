using delivery_order_services.Commons.ResultPattern;
using delivery_order_services.Controllers.User.Model;
using delivery_order_services.Application.Repositories.Contracts;

namespace delivery_order_services.Controllers.User.UseCase
{
    public class UserUseCase(
        IUserRepository userRepository,
        ILogger<UserUseCase> logger
        ) : IUserUseCase
    {

        private readonly ILogger<UserUseCase> _logger = logger;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<UserResponseModel>> ExecuteAsync(UserRequestModel userRequest, CancellationToken cancellationToken)
        {
            try
            {
                var userEntity = new Application.Entities.User()
                {
                    Name = userRequest.Name,
                    Email = userRequest.Email,
                };

                userEntity.SetClient();

				await _userRepository.CreateAsync(userEntity,cancellationToken);

                var userModel = userEntity.ToUserResponseModel();

                return Result<UserResponseModel>.Success(userModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"[{Type}] An error occurred. Input: {@Input}",
                    nameof(UserUseCase),
                    new
                    {
                        Method = nameof(ExecuteAsync)
                    });

                return Result<UserResponseModel>.Failed($"An error occurred in the class {nameof(UserUseCase)}");
            }
        }
        public async Task<Result<List<Application.Domain.User>>> GetAllAsync()
        {
            try
            {
                var result = await _userRepository.GetAllAsync();

                return Result<List<Application.Domain.User>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{Type}] An error occurred. Input: {@Input}",
                    nameof(UserUseCase),
                    new
                    {
                        Method = nameof(GetAllAsync)
                    });

                return Result<List<Application.Domain.User>>.Failed($"An error occurred in the class {nameof(UserUseCase)}");
            }   
        }
    }
}