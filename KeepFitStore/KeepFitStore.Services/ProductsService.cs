namespace KeepFitStore.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Options;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;

    using KeepFitStore.Helpers;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Data;
    using KeepFitStore.Domain.Products;
    using KeepFitStore.Models.ViewModels.Products;
    using System;

    public class ProductsService : IProductsService
    {
        private const string CloudinarySettingCrop = "fill";

        private const string CloudinarySettingGravity = "face";

        private const int CloudinarySettingWidth = 500;

        private const int CloudinarySettingHeight = 500;

        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;
        private readonly IOptions<CloudinarySettings> cloudinaryConfig;
        private Cloudinary cloudinary;

        public ProductsService(KeepFitDbContext context, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this.context = context;
            this.mapper = mapper;
            this.cloudinaryConfig = cloudinaryConfig;
            SetCloudinary();
        }

        public async Task CreateProductAsync<TEntityType, TSourceType>(TSourceType sourceType, IFormFile image)
            where TSourceType : class
            where TEntityType : Product
        {
            var product = this.mapper.Map<TEntityType>(sourceType);

            if (product == null)
            {
                return;
            }

            product.ImageUrl = UploadImage(image);

            this.context.Add(product);
            await this.context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductViewModel>> GetTopRatedProducts(int countOfProducts)
        {
            var products = await this.context
                .Products
                .Include(x => x.Reviews)
                .ToListAsync();

            var orderedProducts = products
             .OrderByDescending(x => x.Rating)
             .Take(countOfProducts);

            var viewModel = this.mapper.Map<IEnumerable<ProductViewModel>>(orderedProducts);
            return viewModel;
        }

        public async Task<IEnumerable<ProductViewModel>> GetNewestProductsAsync(int countOfProducts)
        {
            var products = await this.context
                .Products
                .Include(x => x.Reviews)
                .OrderByDescending(x => x.CreatedOn)
                .Take(countOfProducts)
                .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<ProductViewModel>>(products);
            return viewModel;
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllWithReviews()
        {
            var products = await this.context
               .Products
               .Include(x => x.Reviews)
               .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<ProductViewModel>>(products);
            return viewModel;
        }

        public async Task<IEnumerable<IndexProductViewModel>> GetAll()
        {
            var products = await this.context
               .Products
               .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<IndexProductViewModel>>(products);
            return viewModel;
        }

        public async Task<ProductViewModel> GetProductByIdAsync(int id)
        {
            var product = await this.context
                .Products
                .Include(x => x.Reviews)
                .SingleOrDefaultAsync(x => x.Id == id);

            var viewModel = this.mapper.Map<ProductViewModel>(product);
            return viewModel;
        }

        public async Task<int> DeleteProductByIdAsync(int id)
        {
            var product = await this.context.Products.SingleOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                //TODO: throw service error 
            }

            this.context.Remove(product);
            var countOfDeletedItems = await this.context.SaveChangesAsync();

            return countOfDeletedItems;
        }

        public async Task<TDestination> FindProductForEditAsync<TDestination>(int id)
        {
            var product = await this.context
                .Products
                .SingleOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                //TODO: throw service error
            }

            var model = this.mapper.Map<TDestination>(product);

            return model;
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
                //TODO: throw service error
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
                imageUrl = UploadImage(newImage);
            }

            var entity = this.mapper.Map<TDestination>(model);
            entity.ImageUrl = imageUrl; 

            this.context.Update(entity);
            var countOfEditedRows = await this.context.SaveChangesAsync();

            return countOfEditedRows;
        }

        public void ValidateProductType(Type enumType, string wantedType)
        {
            var isValidType = Enum.TryParse(enumType, wantedType, true, out _);

            if (!isValidType)
            {
                //TODO: throw service error: Invalid product type
            }
        }

        private void SetCloudinary()
        {
            Account account = new Account(
           this.cloudinaryConfig.Value.CloudName,
           this.cloudinaryConfig.Value.ApiKey,
           this.cloudinaryConfig.Value.ApiSecret
           );

            this.cloudinary = new Cloudinary(account);
        }

        private string UploadImage(IFormFile image)
        {
            var uploadResult = new ImageUploadResult();
            if (image.Length > 0)
            {
                using (var stream = image.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(image.Name, stream),
                        Transformation = new Transformation()
                                        .Width(CloudinarySettingWidth)
                                        .Height(CloudinarySettingHeight)
                                        .Crop(CloudinarySettingCrop)
                                        .Gravity(CloudinarySettingGravity)
                    };
                    uploadResult = cloudinary.Upload(uploadParams);
                }
            }
            return uploadResult.Uri?.ToString();
        }
    }
}