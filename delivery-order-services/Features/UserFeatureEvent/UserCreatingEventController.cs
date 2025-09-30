using delivery_order_services.Features.UserFeatureEvent.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace delivery_order_services.Features.UserFeatureEvent
{
    [ApiController]
    [Route("[ApiController]")]
    public class UserController : ControllerBase
    {
        private readonly Contracts.IUserCreatingEventUseCase userIUserServiceUseCase;

        public UserController(Contracts.IUserCreatingEventUseCase userIUserServiceUseCase)
        {
            this.userIUserServiceUseCase = userIUserServiceUseCase;
        }
    }
}
