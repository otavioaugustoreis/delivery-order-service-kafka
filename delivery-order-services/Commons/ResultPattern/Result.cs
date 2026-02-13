namespace delivery_order_services.Commons.ResultPattern
{
    public class Result<T>
    {
        public T Content { get; private set; }
        public string? ErrorMessage { get; private set; }
        public bool IsSuccess { get; private set; } 

        public Result(string? errorMesssage,T content ,bool isSuccess)
        {
            if (string.IsNullOrEmpty(errorMesssage))
                ErrorMessage = "Ocorreu um erro inesperado";

            ErrorMessage = errorMesssage;
            IsSuccess = isSuccess;
            Content = content;
        }

        public Result(string? errorMesssage, bool isSuccess)
        {
            if (string.IsNullOrEmpty(errorMesssage))
                ErrorMessage = "Ocorreu um erro inesperado";

            ErrorMessage = errorMesssage;
            IsSuccess = isSuccess;
        }

        public Result()
        {
        }

        public static Result<T> Success(T result) => new(null,result,true);
        public static Result<T> Failed(string? errorMessage) => new(errorMessage!,false);
    }

}
