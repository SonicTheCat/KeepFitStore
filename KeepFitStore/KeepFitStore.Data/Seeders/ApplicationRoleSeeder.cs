namespace KeepFitStore.Data.Seeders
{
    using System;
    using System.Threading.Tasks;

    using KeepFitStore.Common;
    using Common;
    using KeepFitStore.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;


    public class ApplicationRoleSeeder
    {
        public static void Seed(IServiceProvider provider)
        {
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

            SeedRoles(roleManager, GlobalConstants.AdministratorRoleName).Wait();
            SeedRoles(roleManager, GlobalConstants.UserRoleName).Wait();

            var configuration = provider.GetRequiredService<IConfiguration>();
            var userManager = provider.GetRequiredService<UserManager<KeepFitUser>>();

            CreatePowerUser(configuration, userManager).Wait();
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager, string role)
        {
            if (roleManager == null)
            {
                throw new ArgumentNullException(nameof(roleManager));
            }

            var roleExists = await roleManager.RoleExistsAsync(role);

            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private static async Task CreatePowerUser(IConfiguration configuration, UserManager<KeepFitUser> userManager)
        {
            var user = await userManager.FindByEmailAsync(configuration.GetSection(DataConstants.UserSettingsString)[DataConstants.UserEmailString]);

            if (user == null)
            {
                var powerUser = new KeepFitUser
                {
                    UserName = configuration.GetSection(DataConstants.UserSettingsString)[DataConstants.UsernameString],
                    Email = configuration.GetSection(DataConstants.UserSettingsString)[DataConstants.UserEmailString],
                    FullName = configuration.GetSection(DataConstants.UserSettingsString)[DataConstants.FullNameString],
                    Basket = new Basket() 
                };

                string userPassword = configuration.GetSection(DataConstants.UserSettingsString)[DataConstants.UserPasswordString];

                powerUser.EmailConfirmed = true;

                var createPowerUser = await userManager.CreateAsync(powerUser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the "Administrator" role 
                    await userManager.AddToRoleAsync(powerUser, GlobalConstants.AdministratorRoleName);
                }
            }
        }
    }
}