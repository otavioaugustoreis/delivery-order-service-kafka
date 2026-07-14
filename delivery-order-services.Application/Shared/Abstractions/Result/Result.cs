
namespace delivery_order_services.Application.Shared.Abstractions.Result
{

    public class Result 
    {
        public Error? Error { get; private set; }
        public bool IsSuccess { get; private set; }


        public Result(Error? errorMessage, bool isSuccess)
        {
            Error = errorMessage;
            IsSuccess = isSuccess;
        }

        public static Result Success() => new(null, true);
        public static Result Failed(Error? errorMessage) => new(errorMessage!, false);

    }

    public class Result<T> : Result
    {
        public T? Content { get; private set; }
        
        public Result(Error? errorMesssage,T content ,bool isSuccess) : base(errorMesssage,isSuccess)
        {
            Content = content;
        }

        public Result(Error? errorMesssage, bool isSuccess) : base(errorMesssage,isSuccess)
        {
        }

        public static Result<T> Success(T result) => new(null,result,true);
        public static Result<T> Failed(Error? errorMessage) => new(errorMessage!,false);
    }

    public class Error 
    {
        public ErrorCode Code { get; private set; }
        public string? ErrorMessage { get; private set; }
        public Error(ErrorCode code, string? errorMessage)
        {
            ErrorMessage = errorMessage;
            code = Code;
        }
    }

    public enum ErrorCode
    {
        RequestTimeout,
        ValidationError,
        NotFound,
        Conflict,
        BadRequest,
        InternalServerError,
        UnexpectedError
    }
}