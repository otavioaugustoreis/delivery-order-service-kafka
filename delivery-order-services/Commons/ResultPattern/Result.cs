namespace delivery_order_services.Commons.ResultPattern
{

    public class Result 
    {
        public string? ErrorMessage { get; private set; }
        public bool IsSuccess { get; private set; }


        public Result(string? errorMessage, bool isSuccess)
        {
            ErrorMessage = errorMessage;
            IsSuccess = isSuccess;
        }

        public static Result Success() => new(null, true);
        public static Result Failed(string? errorMessage) => new(errorMessage!, false);

    }


    public class Result<T> : Result
    {
        public T? Content { get; private set; }
        
        public Result(string? errorMesssage,T content ,bool isSuccess) : base(errorMesssage,isSuccess)
        {
            Content = content;
        }

        public Result(string? errorMesssage, bool isSuccess) : base(errorMesssage,isSuccess)
        {
        }

        public static Result<T> Success(T result) => new(null,result,true);
        public static Result<T> Failed(string? errorMessage) => new(errorMessage!,false);
    }

}
