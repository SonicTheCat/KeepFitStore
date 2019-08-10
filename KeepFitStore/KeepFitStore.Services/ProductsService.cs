namespace KeepFitStore.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Linq.Dynamic.Core;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using KeepFitStore.Helpers;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Data;
    using KeepFitStore.Domain.Products;
    using KeepFitStore.Services.PhotoKeeper;
    using KeepFitStore.Services.CustomExceptions;
    using KeepFitStore.Services.CustomExceptions.Messsages;

    public class ProductsService : IProductsService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;
        private readonly IMyCloudinary cloudinary;

        public ProductsService(KeepFitDbContext context, IMapper mapper, IMyCloudinary cloudinary)
        {
            this.context = context;
            this.mapper = mapper;
            this.cloudinary = cloudinary;
        }

        public async Task<int> CreateProductAsync<TEntityType, TSourceType>(TSourceType sourceType, IFormFile image)
            where TSourceType : class
            where TEntityType : Product
        {
            var product = this.mapper.Map<TEntityType>(sourceType);

            product.ImageUrl = this.cloudinary.UploadImage(image);

            this.context.Add(product);
            var rowsAdded = await this.context.SaveChangesAsync();

            return rowsAdded; 
        }

        public async Task<IEnumerable<TViewModel>> GetTopRatedProducts<TViewModel>(int countOfProducts)
        {
            var products = await this.context
                .Products
                .Include(x => x.Reviews)
                .AsNoTracking()
                .ToListAsync();

            var orderedProducts = products
             .OrderByDescending(x => x.Rating)
             .Take(countOfProducts);

            var viewModel = this.mapper.Map<IEnumerable<TViewModel>>(orderedProducts);
            return viewModel;
        }

        public async Task<IEnumerable<TViewModel>> GetNewestProductsAsync<TViewModel>(int countOfProducts)
        {
            var products = await this.context
                .Products
                .Include(x => x.Reviews)
                .OrderByDescending(x => x.CreatedOn)
                .Take(countOfProducts)
                .AsNoTracking()
                .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<TViewModel>>(products);
            return viewModel;
        }

        public async Task<PaginatedList<TViewModel>> SearchProductsAsync<TViewModel>(int pageNumber, int pageSize, string sortBy)
        {
            var products = this.context
               .Products
               .Include(x => x.Reviews)
               .AsQueryable();

            var paginatedList = await PaginatedList<Product>.CreateAsync(products, pageNumber, pageSize, sortBy);

            var paginatedListViewModel = this.mapper.Map<PaginatedList<TViewModel>>(paginatedList);

            PaginatedList<TViewModel>.SwapValues(paginatedListViewModel, paginatedList);

            return paginatedListViewModel;
        }

        public async Task<IEnumerable<TViewModel>> GetAll<TViewModel>()
        {
            var products = await this.context
               .Products
               .AsNoTracking()
               .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<TViewModel>>(products);
            return viewModel;
        }

        public async Task<TViewModel> GetProductByIdAsync<TViewModel>(int id)
        {
            var product = await this.context
                .Products
                .Include(x => x.Reviews)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                throw new ProductNotFoundException(string.Format(ExceptionMessages.ProductNotFound, id));
            }

            var viewModel = this.mapper.Map<TViewModel>(product);
            return viewModel;
        }

        public async Task<int> DeleteProductByIdAsync(int id)
        {
            var product = await this.context.Products.SingleOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                throw new ProductNotFoundException(string.Format(ExceptionMessages.ProductNotFound, id));
            }

            this.context.Remove(product);
            var countOfDeletedItems = await this.context.SaveChangesAsync();

            return countOfDeletedItems;
        }

        public async Task<int> EditProductAsync<TDestination, TSourceType>(
            TSourceType model,
            IFormFile newImage,
            int productId)
            where TSourceType : class
            where TDestination : Product
        {
            if (!this.context.Products.Any(e => e.Id == productId))
            {
                throw new ProductNotFoundException(string.Format(ExceptionMessages.ProductNotFound, productId));
            }

            string imageUrl = this.context
                .Products
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == productId)
                .GetAwaiter()
                .GetResult()
                .ImageUrl;

            if (newImage != null)
            {
                imageUrl = this.cloudinary.UploadImage(newImage);
            }

            var entity = this.mapper.Map<TDestination>(model);
            entity.ImageUrl = imageUrl;

            this.context.Update(entity);
            var countOfEditedRows = await this.context.SaveChangesAsync();

            return countOfEditedRows;
        }

        public async Task<IEnumerable<TViewModel>> GetMostOrderedProducts<TViewModel>(int countOfProducts)
        {
            var products = await this.context
               .Products
               .Include(x => x.Reviews)
               .OrderByDescending(x => x.Orders.Count)
               .Take(countOfProducts)
               .AsNoTracking()
               .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<TViewModel>>(products);
            return viewModel;
        }

        //public async Task<TDestination> FindProductForEditAsync<TDestination>(int id)
        //{
        //    var product = await this.context
        //        .Products
        //        .AsNoTracking()
        //        .SingleOrDefaultAsync(x => x.Id == id);

        //    if (product == null)
        //    {
        //        throw new ProductNotFoundException(string.Format(ExceptionMessages.ProductNotFound, id));
        //    }

        //    var model = this.mapper.Map<TDestination>(product);

        //    return model;
        //}

    }
}