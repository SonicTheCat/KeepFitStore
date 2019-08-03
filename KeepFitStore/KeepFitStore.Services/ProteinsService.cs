namespace KeepFitStore.Services
{
    using System.Threading.Tasks;
    using System.Linq;
    using System.Collections.Generic;
    using System;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Domain.Enums;
    using KeepFitStore.Services.CustomExceptions;
    using KeepFitStore.Domain.Products;
    using KeepFitStore.Services.CustomExceptions.Messsages;

    public class ProteinsService : IProteinsService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public ProteinsService(KeepFitDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TViewModel>> GetAllByTypeAsync<TViewModel>(string type)
        {
            var isValidType = Enum.TryParse(typeof(ProteinType), type, true, out _);

            if (!isValidType)
            {
                throw new ServiceException(string.Format(
                    ExceptionMessages.InvalidProductType, type, nameof(Protein)));
            }

            var proteins = await this.context
               .Proteins
               .Where(x => x.Type.ToString() == type)
               .AsNoTracking()
               .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<TViewModel>>(proteins);
            return viewModel;
        }

        public async Task<TViewModel> GetByIdAsync<TViewModel>(int id)
        {
            var protein = await this.context
                .Proteins
                .Include(x => x.Reviews)
                .ThenInclude(x => x.KeepFitUser)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (protein == null)
            {
                throw new ProductNotFoundException(string.Format(ExceptionMessages.ProductNotFound, id));
            }

            var viewModel = this.mapper.Map<TViewModel>(protein);
            return viewModel;
        }
    }
}