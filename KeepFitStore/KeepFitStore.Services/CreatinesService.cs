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

    public class CreatinesService : ICreatinesService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public CreatinesService(KeepFitDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TViewModel>> GetAllByTypeAsync<TViewModel>(string type)
        {
            var isValidType = Enum.TryParse(typeof(CreatineType), type, true, out _);

            if (!isValidType)
            {
                throw new InvalidProductTypeException(string.Format(ExceptionMessages.InvalidProductType, type, nameof(Creatine)));
            }

            var creatines = await this.context
               .Creatines
               .Where(x => x.Type.ToString() == type)
               .AsNoTracking()
               .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<TViewModel>>(creatines);
            return viewModel;
        }

        public async Task<TViewModel> GetByIdAsync<TViewModel>(int id)
        {
            var creatine = await this.context
                .Creatines
                .Include(x => x.Reviews)
                .ThenInclude(x => x.KeepFitUser)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (creatine == null)
            {
                throw new ProductNotFoundException(string.Format(ExceptionMessages.InvalidCreatine, id));
            }

            var viewModel = this.mapper.Map<TViewModel>(creatine);
            return viewModel;
        }
    }
}