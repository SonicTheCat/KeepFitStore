namespace KeepFitStore.WEB.Filters
{
    using System.Net;
    using System.Reflection;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using KeepFitStore.Services.CustomExceptions;
    using KeepFitStore.Services.CustomExceptions.Attributes;

    public class KeepFitExceptionFilter : IExceptionFilter
    {
        private const string ServerError = "Server error occured";

        private readonly bool isDevelopment;

        public KeepFitExceptionFilter(bool isDevelopment)
        {
            this.isDevelopment = isDevelopment;
        }

        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            var message = ServerError;

            var exception = context.Exception;
            var stackTrace = this.isDevelopment ? exception.StackTrace : string.Empty;

            if (exception is ServiceException)
            {
                var exAttr = exception
                    .GetType()
                    .GetCustomAttribute(typeof(MyExceptionAttribute))
                    as MyExceptionAttribute;

                message = exception.Message;
                status = (HttpStatusCode)exAttr.StatusCode;
            }

            context.ExceptionHandled = true;

            context.Result = new ObjectResult(new
            {
                StatusCode = status,
                Message = message,
                StackTrace = stackTrace
            });
        }
    }
}