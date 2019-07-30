namespace KeepFitStore.Services
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Domain;
    using KeepFitStore.Models.InputModels.Address;
    using KeepFitStore.Services.Contracts;

    public class AddressService : IAddressService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<KeepFitUser> userManager;

        public AddressService(KeepFitDbContext context, IMapper mapper, UserManager<KeepFitUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<TViewModel> AddAddressToUserAsync<TViewModel>(
            CreateAddressInputModel model, 
            ClaimsPrincipal principal)
        {
            var city = this.context
                .Cities
                .SingleOrDefault(x => x.Name == model.CityName && 
                x.PostCode == model.PostCode);

            if (city == null)
            {
                city = this.mapper.Map<City>(model);
            }

            var address = this.context
                .Addresses
                .Include(x => x.City)
                .SingleOrDefault(x =>
                x.StreetName == model.StreetName &&
                x.StreetNumber == model.StreetNumber &&
                x.City.Name == model.CityName &&
                x.City.PostCode == model.PostCode);

            if (address != null)
            {
                return this.mapper.Map<TViewModel>(address);
            }

            address = this.mapper.Map<Address>(model);
            address.City = city;
            this.context.Addresses.Add(address);
            var user = await this.userManager.GetUserAsync(principal);
            user.Address = address; 
            await this.context.SaveChangesAsync();

            return this.mapper.Map<TViewModel>(address);
        }

        public async Task<TViewModel> GetAddressFromUser<TViewModel>(ClaimsPrincipal principal)
        {
            var userFromDb = await this.userManager.GetUserAsync(principal);

            var user = await this.userManager
                .Users
                .Include(x => x.Address)
                .SingleOrDefaultAsync(x => x.Id == userFromDb.Id);

            var address = this.mapper.Map<TViewModel>(user.Address);
            return address; 
        }
    }
}