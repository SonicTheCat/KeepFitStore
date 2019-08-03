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

    public class AminosService : IAminosService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public AminosService(KeepFitDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TViewModel>> GetAllByTypeAsync<TViewModel>(string type)
        {
            var isValidType = Enum.TryParse(typeof(AminoAcidType), type, true, out _);

            if (!isValidType)
            {
                throw new InvalidProductTypeException(string.Format(ExceptionMessages.InvalidProductType, type, nameof(AminoAcid)));
            }

            var aminos = await this.context
              .Aminos
              .Where(x => x.Type.ToString() == type)
              .AsNoTracking()
              .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<TViewModel>>(aminos);
            return viewModel;
        }

        public async Task<TViewModel> GetByIdAsync<TViewModel>(int id)
        {
            var amino = await this.context
                .Aminos
                .Include(x => x.Reviews)
                .ThenInclude(x => x.KeepFitUser)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (amino == null)
            {
                throw new ProductNotFoundException(string.Format(ExceptionMessages.InvalidAminoId, id));
            }

            var viewModel = this.mapper.Map<TViewModel>(amino);
            return viewModel;
        }
    }
}