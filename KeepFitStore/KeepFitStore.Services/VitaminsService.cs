namespace KeepFitStore.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;
    using System;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Domain.Enums;
    using KeepFitStore.Services.CustomExceptions;
    using KeepFitStore.Domain.Products;
    using KeepFitStore.Services.CustomExceptions.Messsages;

    public class VitaminsService : IVitaminsService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public VitaminsService(KeepFitDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TViewModel>> GetAllByTypeAsync<TViewModel>(string type)
        {
            var isValidType = Enum.TryParse(typeof(VitaminType), type, true, out _);

            if (!isValidType)
            {
                throw new ServiceException(string.Format(
                    ExceptionMessages.InvalidProductType, type, nameof(Vitamin)));
            }

            var vitamins = await this.context
              .Vitamins
              .Where(x => x.Type.ToString() == type)
              .AsNoTracking()
              .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<TViewModel>>(vitamins);
            return viewModel;
        }

        public async Task<TViewModel> GetByIdAsync<TViewModel>(int id)
        {
            var vitamin = await this.context
                .Vitamins
                .Include(x => x.Reviews)
                .ThenInclude(x => x.KeepFitUser)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (vitamin == null)
            {
                throw new ProductNotFoundException(string.Format(ExceptionMessages.ProductNotFound, id));
            }

            var viewModel = this.mapper.Map<TViewModel>(vitamin);
            return viewModel;
        }
    }
}