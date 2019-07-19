namespace KeepFitStore.WEB.Middlewares
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    using KeepFitStore.WEB.Common;

    public class CreateRoleMiddleware
    {
        private readonly RequestDelegate next;

        public CreateRoleMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager, GlobalConstants.AdministratorRoleName).Wait();
            SeedRoles(roleManager, GlobalConstants.UserRoleName).Wait();

            await this.next(context);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager, string role)
        {
            var roleExists = await roleManager.RoleExistsAsync(role);

            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}