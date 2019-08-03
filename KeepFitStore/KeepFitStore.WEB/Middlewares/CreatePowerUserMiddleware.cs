namespace KeepFitStore.WEB.Middlewares
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;

    using KeepFitStore.WEB.Common;
    using KeepFitStore.Domain;

    public class CreatePowerUserMiddleware
    {
        private readonly RequestDelegate next;

        public CreatePowerUserMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration, UserManager<KeepFitUser> userManager)
        {
            var user = await userManager.FindByEmailAsync(configuration.GetSection(WebConstants.UserSettingsString)[WebConstants.UserEmailString]);

            if (user == null)
            {
                var powerUser = new KeepFitUser
                {
                    UserName = configuration.GetSection(WebConstants.UserSettingsString)[WebConstants.UsernameString],
                    Email = configuration.GetSection(WebConstants.UserSettingsString)[WebConstants.UserEmailString],
                    FullName = configuration.GetSection(WebConstants.UserSettingsString)[WebConstants.FullNameString],
                    Basket = new Basket()
                };

                string userPassword = configuration.GetSection(WebConstants.UserSettingsString)[WebConstants.UserPasswordString];

                powerUser.EmailConfirmed = true;

                var createPowerUser = await userManager.CreateAsync(powerUser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the "Administrator" role 
                    await userManager.AddToRoleAsync(powerUser, WebConstants.AdministratorRoleName);
                }
            }

            await this.next(context);
        }
    }
}