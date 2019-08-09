namespace KeepFitStore.Services.CustomExceptions
{
    using Attributes;

    [MyException(StatusCode = 404)]
    public class OrderNotFoundException : ServiceException
    {
        public OrderNotFoundException(string message)
            : base(message)
        {
        }
    }
}