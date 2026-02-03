using delivery_order_services.Commons.Mapper;
using delivery_order_services.Features.OrderCreatingFeature.Models;
using delivery_order_services.Features.UserFeatureEvent.Contracts;
using delivery_order_services.Features.UserFeatureEvent.Model;
using Microsoft.AspNetCore.Mvc;

namespace delivery_order_services.Features.UserFeatureEvent
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserCreatingEventUseCase _userIUserServiceUseCase;

        public UserController(IUserCreatingEventUseCase userIUserServiceUseCase)
        {
            _userIUserServiceUseCase = userIUserServiceUseCase;
        }

        [HttpPost]
        public ActionResult PostCreateEventUser(
                [FromBody] UserRequestModel userRequest
            )
        {
            var userCreated = _userIUserServiceUseCase.ExecuteAsync(userRequest);

            return Ok(MessageCommons.USUARIO_CADASTRADO);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            var result = await _userIUserServiceUseCase.GetAll();

            return Ok(result.Content);
        }
    }
}
