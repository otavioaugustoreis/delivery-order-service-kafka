namespace delivery_order_services.ResultPattern
{
    public class Result<T> where T: class
    {
        public T Content { get; set; }
        public string ErrorMessage { get; set; }

        public Result(string errorMesssage)
        {
            if(string.IsNullOrEmpty(errorMesssage)) 
                ErrorMessage = "Ocorreu um erro inesperado";
                
            ErrorMessage = errorMesssage;
        }

        public Result()
        {
        }

        public Result<T> Success(T result) => new();
        public Result<T> Failed(string? errorMessage) => new(errorMessage);
    }

}
