using delivery_order_services.Commons.ResultPattern;
using delivery_order_services.Controllers.User.Contracts;
using delivery_order_services.Controllers.User.Model;
using delivery_order_services.Application.Entities;
using delivery_order_services.Application.Repositories.Contracts;


namespace delivery_order_services.Controllers.User.UseCase
{
    public class UserCreatingEventUseCase(
        IUserRepository _userRepository,
        ILogger<UserCreatingEventUseCase> _logger
        ) : IUserCreatingEventUseCase
    {

        private readonly ILogger<UserCreatingEventUseCase> logger = _logger;
        private readonly IUserRepository userRepository = _userRepository;


        public async Task<Result<UserResponseModel>> ExecuteAsync(UserRequestModel userRequest, CancellationToken cancellationToken)
        {
            try
            {
                 await userRepository.CreateAsync(
                      new()
                      {
                          Name = userRequest.Name,
                          Email = userRequest.Email,
                          UserType = UserRequestModel.GetUserType(userRequest.UserType)
                      }, cancellationToken
                    );

                var userModel = new UserResponseModel(
                    userRequest.Name,
                    userRequest.Email,
                    userRequest.UserType);

                return Result<UserResponseModel>.Success(userModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"[{Type}] An error occurred. Input: {@Input}",
                    nameof(UserCreatingEventUseCase),
                    new
                    {
                        Method = nameof(ExecuteAsync)
                    });

                return Result<UserResponseModel>.Failed($"An error occurred in the class {nameof(UserCreatingEventUseCase)}");
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
                    nameof(UserCreatingEventUseCase),
                    new
                    {
                        Method = nameof(GetAllAsync)

                    });

                return Result<List<UserEntity>>.Failed($"An error occurred in the class {nameof(UserCreatingEventUseCase)}");
            }   
        }
    }
}
//Feature a criar -> Criar Produção dellll