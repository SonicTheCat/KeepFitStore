namespace KeepFitStore.WEB.Middlewares
{
    using Microsoft.AspNetCore.Builder;

    public static class CreateRolesMiddlewareExtensions
    {
        public static IApplicationBuilder UseCreateRolesMiddleware(
           this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CreateRoleMiddleware>();
        }
    }
}