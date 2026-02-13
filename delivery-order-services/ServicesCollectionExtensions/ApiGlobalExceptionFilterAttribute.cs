using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace delivery_order_services.ServicesCollectionExtensions
{
    public class ApiGlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public ApiGlobalExceptionFilterAttribute(ILogger<ApiGlobalExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.Log(
                logLevel: LogLevel.Error,
                eventId: new EventId(1, nameof(Exception)),
                message: "An error occured in '{@Source}'",
                new
                {
                    Source = context.HttpContext.Request.Path
                });

            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            context.ExceptionHandled = true;

            base.OnException(context);
        }
    }
}