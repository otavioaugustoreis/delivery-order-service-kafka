using delivery_order_services.Controllers.User.Model;
using delivery_order_services.Controllers.User.UseCase;
using delivery_order_services.ServicesCollectionExtensions;
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
            var result = await _usecase.InsertOneAsync(userRequest, cancellationToken);

            return result.ToActionResult();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            var result = await _usecase.FindAsync(cancellationToken);

            return result.ToActionResult();
        }
    }
}
