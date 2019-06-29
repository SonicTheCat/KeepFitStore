﻿namespace KeepFitStore.WEB.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class RedirectUserIfLoggedInFilterAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.HttpContext.User != null)
            {
                context.Result = new RedirectResult("/"); 
            }
        }
    }
}