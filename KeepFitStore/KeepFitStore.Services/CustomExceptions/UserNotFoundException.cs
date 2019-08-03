namespace KeepFitStore.Services.CustomExceptions
{
    using Attributes;

    [MyException(StatusCode = 404)]
    public class UserNotFoundException : ServiceException
    {
        public UserNotFoundException(string message)
            : base(message)
        {
        }
    }
}