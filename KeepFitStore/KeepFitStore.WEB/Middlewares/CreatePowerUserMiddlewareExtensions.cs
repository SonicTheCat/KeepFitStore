namespace KeepFitStore.WEB.Middlewares
{
    using Microsoft.AspNetCore.Builder;

    public static class CreatePowerUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseCreatePowerUserMiddleware(
           this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CreatePowerUserMiddleware>();
        }
    }
}