namespace KeepFitStore.WEB.Middlewares
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;

    using KeepFitStore.WEB.Common;
    using KeepFitStore.Domain;

    public class SeedPowerUserMiddleware
    {
        private readonly RequestDelegate next;

        public SeedPowerUserMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration, UserManager<KeepFitUser> userManager)
        {
            var user = await userManager.FindByEmailAsync(configuration.GetSection(GlobalConstants.UserSettingsString)[GlobalConstants.UserEmailString]);

            if (user == null)
            {
                var powerUser = new KeepFitUser
                {
                    UserName = configuration.GetSection(GlobalConstants.UserSettingsString)[GlobalConstants.UsernameString],
                    Email = configuration.GetSection(GlobalConstants.UserSettingsString)[GlobalConstants.UserEmailString],
                    FullName = configuration.GetSection(GlobalConstants.UserSettingsString)[GlobalConstants.FullNameString],
                    Basket = new Basket()
                };

                string userPassword = configuration.GetSection(GlobalConstants.UserSettingsString)[GlobalConstants.UserPasswordString];

                powerUser.EmailConfirmed = true;

                var createPowerUser = await userManager.CreateAsync(powerUser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the "Administrator" role 
                    await userManager.AddToRoleAsync(powerUser, GlobalConstants.AdministratorRoleName);
                }
            }

            await this.next(context);
        }
    }
}