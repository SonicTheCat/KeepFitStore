namespace KeepFitStore.Services.CustomExceptions
{
    using Attributes;

    [MyException(StatusCode = 403)]
    public class UserNotAuthorizedException : ServiceException
    {
        public UserNotAuthorizedException(string message)
            : base(message)
        {
        }
    }
}