using delivery_order_services.Commons.Mapper;
using delivery_order_services.Features.UserFeatureEvent.Contracts;
using delivery_order_services.Features.UserFeatureEvent.Model;
using Microsoft.AspNetCore.Mvc;

namespace delivery_order_services.Features.UserFeatureEvent
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserCreatingEventUseCase _usecase;

        public UserController(IUserCreatingEventUseCase userIUserServiceUseCase)
        {
            _usecase = userIUserServiceUseCase;
        }

        [HttpPost]
        public async Task<ActionResult> PostCreatingUserAsync(
                [FromBody] UserRequestModel userRequest,
                CancellationToken cancellationToken
            )
        {
            var userCreated = await _usecase.ExecuteAsync(userRequest, cancellationToken);

            return Ok(MessageCommons.USUARIO_CADASTRADO);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsersAsync()
        {
            var result = await _usecase.GetAllAsync();

            return result.IsSuccess 
                ? Ok(result.Content) 
                : BadRequest(result.ErrorMessage!);
        }
    }
}
