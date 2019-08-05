namespace KeepFitStore.Services
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Domain;
    using KeepFitStore.Models.InputModels.Address;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Services.CustomExceptions;
    using KeepFitStore.Services.CustomExceptions.Messsages;

    public class AddressService : IAddressService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public AddressService(KeepFitDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<TViewModel> AddAddressToUserAsync<TViewModel>(
            CreateAddressInputModel model,
           string username)
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
                .Include(x => x.KeepFitUsers)
                .SingleOrDefault(x =>
                x.StreetName == model.StreetName &&
                x.StreetNumber == model.StreetNumber &&
                x.City.Name == model.CityName &&
                x.City.PostCode == model.PostCode);

            var thisUserLiveHere = address?
                .KeepFitUsers
                .Any(x => x.UserName == username);

            if (address != null && thisUserLiveHere.HasValue)
            {
                return this.mapper.Map<TViewModel>(address);
            }

            if (address == null)
            {
                address = this.mapper.Map<Address>(model);
                address.City = city;
                this.context.Addresses.Add(address);
            }
            
            var user = await this.context
                .Users
                .SingleOrDefaultAsync(x => x.UserName == username);

            user.Address = address;
            await this.context.SaveChangesAsync();

            return this.mapper.Map<TViewModel>(address);
        }

        public async Task<TViewModel> GetAddressFromUser<TViewModel>(string id)
        {
            KeepFitUser user = null;

            try
            {
                user = await this.context
                .Users
                .Include(x => x.Address)
                .ThenInclude(x => x.City)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            }
            catch (InvalidOperationException ex)
            {
                throw new ServiceException(string.Format(ExceptionMessages.UserLookupFailed, id), ex);
            }

            if (user == null)
            {
                throw new UserNotFoundException(string.Format(ExceptionMessages.UserLookupFailed, id));
            }

            var address = this.mapper.Map<TViewModel>(user.Address);
            return address;
        }
    }
}