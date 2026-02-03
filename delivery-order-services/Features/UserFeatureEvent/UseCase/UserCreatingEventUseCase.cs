using delivery_order_services.Commons.ResultPattern;
using delivery_order_services.Domain.Entities;
using delivery_order_services.Domain.Repositories.Contracts;
using delivery_order_services.Features.UserFeatureEvent.Contracts;
using delivery_order_services.Features.UserFeatureEvent.Model;
using System.Collections.Generic;

namespace delivery_order_services.Features.UserFeatureEvent.UseCase
{
    public class UserCreatingEventUseCase(
        IUserRepository _userRepository,
        ILogger<UserCreatingEventUseCase> _logger
        ) : IUserCreatingEventUseCase
    {

        private readonly ILogger<UserCreatingEventUseCase> logger = _logger;
        private readonly IUserRepository userRepository = _userRepository;


        public async Task<Result<UserResponseModel>> ExecuteAsync(UserRequestModel userRequest)
        {
            try
            {
                var userReponse = userRepository.CreateAsync
                    (
                      new()
                      {
                          Name = userRequest.Name,
                          Email = userRequest.Email,
                          UserType = UserRequestModel.GetUserType(userRequest.UserType)
                      }
                    );

                if (userReponse.IsFaulted)
                {
                    return Result<UserResponseModel>.Failed(
                        $"An error occurred in the class {nameof(UserCreatingEventUseCase)}");
                }

                var userModel = new UserResponseModel(
                    userRequest.Name,
                    userRequest.Email,
                    userRequest.UserType);

                return Result<UserResponseModel>.Success(userModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"An error occurred in the  class{nameof(UserCreatingEventUseCase)}");

                return Result<UserResponseModel>.Failed(
                    $"An error occurred in the class {nameof(UserCreatingEventUseCase)}," +
                    $" message: {ex.Message}");
            }
        }

        public async Task<Result<List<UserEntity>>> GetAll()
        {
            var result = await userRepository.GetAllAsync();
            return Result<List<UserEntity>>.Success(result);
        }
    }
}
