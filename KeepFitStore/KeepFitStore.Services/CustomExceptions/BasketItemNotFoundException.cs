namespace KeepFitStore.Services.CustomExceptions
{
    using Attributes; 

    [MyException(StatusCode = 404)]
    public class BasketItemNotFoundException : ServiceException
    {
        public BasketItemNotFoundException(string message)
            :base(message)
        {

        }
    }
}