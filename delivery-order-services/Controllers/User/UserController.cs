using delivery_order_services.Controllers.User.Model;
using delivery_order_services.Controllers.User.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace delivery_order_services.Controllers.User
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserUseCase _usecase;

        public UserController(IUserUseCase userIUserServiceUseCase)
        {
            _usecase = userIUserServiceUseCase;
        }

        [HttpPost]
        public async Task<ActionResult> PostCreatingUserAsync(
                [FromBody] UserRequestModel userRequest,
                CancellationToken cancellationToken
            )
        {
            var result = await _usecase.ExecuteAsync(userRequest, cancellationToken);

            return result.IsSuccess 
                ? NoContent() 
                : BadRequest(result.ErrorMessage!);
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
