namespace KeepFitStore.WEB.Middlewares
{
    using Microsoft.AspNetCore.Builder;

    public static class SeedRolesMiddlewareExtensions
    {
        public static IApplicationBuilder UseSeedRolesMiddleware(
           this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SeedRoleMiddleware>();
        }
    }
}