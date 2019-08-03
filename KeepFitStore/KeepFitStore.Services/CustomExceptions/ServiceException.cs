namespace KeepFitStore.Services.CustomExceptions
{
    using System;

    using Attributes;

    [MyException(StatusCode = 400)]
    public class ServiceException : Exception
    {
        public ServiceException()
        {
        }

        public ServiceException(string message)
            : base(message)
        {
        }

        public ServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}