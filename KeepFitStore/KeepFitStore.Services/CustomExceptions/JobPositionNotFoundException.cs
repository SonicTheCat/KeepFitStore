namespace KeepFitStore.Services.CustomExceptions
{
    using Attributes;

    [MyException(StatusCode = 404)]
    public class JobPositionNotFoundException : ServiceException
    {
        public JobPositionNotFoundException(string message)
            :base(message)
        {

        }
    }
}