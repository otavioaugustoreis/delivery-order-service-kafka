using delivery_order_services.Commons.ResultPattern;
using delivery_order_services.Controllers.User.Model;
using delivery_order_services.Application.Entities;
using delivery_order_services.Application.Repositories.Contracts;


namespace delivery_order_services.Controllers.User.UseCase
{
    public class UserUseCase(
        IUserRepository _userRepository,
        ILogger<UserUseCase> _logger
        ) : IUserUseCase
    {

        private readonly ILogger<UserUseCase> logger = _logger;
        private readonly IUserRepository userRepository = _userRepository;


        public async Task<Result<UserResponseModel>> ExecuteAsync(UserRequestModel userRequest, CancellationToken cancellationToken)
        {
            try
            {
                var userEntity = new UserEntity()
                {
                    Name = userRequest.Name,
                    Email = userRequest.Email,
                    UserType = UserRequestModel.GetUserType(userRequest.UserType)
                };

				await userRepository.CreateAsync(userEntity,cancellationToken);

                var userModel = new UserResponseModel(
                    userRequest.Name,
                    userRequest.Email,
                    userRequest.UserType);

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

        public async Task<Result<List<UserEntity>>> GetAllAsync()
        {
            try
            {
                var result = await userRepository.GetAllAsync();

                return Result<List<UserEntity>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{Type}] An error occurred. Input: {@Input}",
                    nameof(UserUseCase),
                    new
                    {
                        Method = nameof(GetAllAsync)

                    });

                return Result<List<UserEntity>>.Failed($"An error occurred in the class {nameof(UserUseCase)}");
            }   
        }
    }
}