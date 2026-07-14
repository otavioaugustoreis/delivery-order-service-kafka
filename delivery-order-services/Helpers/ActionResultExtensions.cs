using delivery_order_services.Application.Shared.Abstractions.Result;
using Microsoft.AspNetCore.Mvc;

namespace delivery_order_services.Helpers
{
    public static  class ActionResultExtensions
    {
        public static ActionResult ToActionResult(this Result result) 
        {
            if (result.IsSuccess)
            {
                if (result.GetContent() is null)
                    return new NoContentResult();

                return new OkObjectResult(result.GetContent());
            }
                
            return result.Error!.Code switch
            {

                ErrorCode.Conflict => new ConflictResult(),
                ErrorCode.ValidationError => new BadRequestObjectResult(result.Error.ErrorMessage),
                ErrorCode.NotFound => new NotFoundResult(),
                ErrorCode.UnexpectedError => new BadRequestObjectResult(result.Error.ErrorMessage),
                ErrorCode.InternalServerError => new InternalServerErrorResult(),
                ErrorCode.RequestTimeout => new ObjectResult(result)
                {
                    StatusCode = StatusCodes.Status408RequestTimeout
                },
                _ => new InternalServerErrorResult()
            };
        }
    }
    public class InternalServerErrorResult : StatusCodeResult
    {
        public InternalServerErrorResult() : base(500)
        {
            
        }
    }
}
