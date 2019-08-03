namespace KeepFitStore.Services.CustomExceptions
{
    using Attributes;

    [MyException(StatusCode = 400)]
    public class InvalidQuantityProvidedException : ServiceException
    {
        public InvalidQuantityProvidedException(string message)
            :base(message)
        {

        }
    }
}