namespace KeepFitStore.WEB.Middlewares
{
    using Microsoft.AspNetCore.Builder;

    public static class SeedPowerUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseSeedPowerUserMiddleware(
           this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SeedPowerUserMiddleware>();
        }
    }
}