namespace KeepFitStore.WEB.Filters
{
    using System.Linq;
    using System.Reflection;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ValidateModelStateFilterAttribute : ActionFilterAttribute
    {
        private readonly string actionName;

        public ValidateModelStateFilterAttribute(string actionName)
        {
            this.actionName = actionName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var action = context.Controller
                    .GetType()
                    .GetMethods()
                    .Where(x => x.Name == actionName)
                    .FirstOrDefault(x => !x.GetCustomAttributes<HttpPostAttribute>().Any());

                
                if (action != null)
                {
                    var actionResult = (IActionResult)action.Invoke(context.Controller, null);
                    context.Result = actionResult;
                }
            }
        }
    }
}