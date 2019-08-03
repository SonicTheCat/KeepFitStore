namespace KeepFitStore.Services.CustomExceptions
{
    using Attributes;

    [MyException(StatusCode = 404)]
    class OrderNotFoundException : ServiceException
    {
        public OrderNotFoundException(string message)
            : base(message)
        {
        }
    }
}