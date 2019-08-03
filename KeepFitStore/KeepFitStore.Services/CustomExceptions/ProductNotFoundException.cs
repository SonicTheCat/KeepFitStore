namespace KeepFitStore.Services.CustomExceptions
{
    using Attributes;

    [MyException(StatusCode = 404)]
    public class ProductNotFoundException : ServiceException
    {
        public ProductNotFoundException(string message)
            : base(message)
        {
        }
    }
}