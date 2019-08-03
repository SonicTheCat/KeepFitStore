namespace KeepFitStore.Services.CustomExceptions
{
    using Attributes;

    [MyException(StatusCode = 400)]
    public class InvalidProductTypeException : ServiceException
    {
        public InvalidProductTypeException(string message)
            : base(message)
        {

        }
    }
}