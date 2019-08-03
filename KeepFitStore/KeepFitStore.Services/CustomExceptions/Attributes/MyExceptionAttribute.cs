namespace KeepFitStore.Services.CustomExceptions.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class MyExceptionAttribute : Attribute
    {
        public int StatusCode { get; set; }
    }
}