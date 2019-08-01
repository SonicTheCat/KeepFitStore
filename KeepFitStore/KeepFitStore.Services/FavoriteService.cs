namespace KeepFitStore.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Domain;
    using KeepFitStore.Services.Contracts;

    public class FavoriteService : IFavoriteService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public FavoriteService(KeepFitDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<bool> AddAsync(int productId, string username)
        {
            var user = await this.GetUserAsync(username);

            if (user == null || user.FavoriteProducts.Any(x => x.ProductId == productId))
            {
                return false;
            }

            var doesProductExist = this.context
                .Products
                .Any(x => x.Id == productId);

            if (!doesProductExist)
            {
                return false;
            }

            user.FavoriteProducts.Add(new KeepFitUserFavoriteProducts()
            {
                ProductId = productId
            });

            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<TViewModel>> GetAllByUser<TViewModel>(string username)
        {
            var favProducts = await this
                .context
                .UserFavoriteProducts
                .Where(x => x.KeepFitUser.UserName == username)
                .Select(x => x.Product)
                .Include(x => x.Reviews)
                .AsNoTracking()
                .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<TViewModel>>(favProducts);

            return viewModel;
        }

        public async Task<bool> RemoveAsync(int productId, string username)
        {
            var product = await this.context
                .UserFavoriteProducts
                .SingleOrDefaultAsync(x => x.ProductId == productId && x.KeepFitUser.UserName == username);

            if (product == null)
            {
                return false;
            }

            this.context.UserFavoriteProducts.Remove(product);
            await this.context.SaveChangesAsync();

            return true;
        }

        //TODO remove this method
        private async Task<KeepFitUser> GetUserAsync(string username)
        {
            return await this.context
                .Users
                .Include(x => x.FavoriteProducts)
                .ThenInclude(x => x.Product)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }
    }
}