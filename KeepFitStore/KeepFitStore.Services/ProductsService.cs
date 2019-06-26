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

    public class ProductsService : IProductsService
    {
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
                                        .Width(500).Height(500)
                                        .Crop("fill")
                                        .Gravity("face")
                    };
                    uploadResult = cloudinary.Upload(uploadParams);
                }
            }
            return uploadResult.Uri?.ToString();
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
            //TODO: change order!
            var products = await this.context
                .Products
                .OrderBy(x => x.Price)
                .Take(countOfProducts)
                .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<ProductViewModel>>(products);
            return viewModel;
        }

        public async Task<IEnumerable<ProductViewModel>> GetNewestProductsAsync(int countOfProducts)
        {
            var products = await this.context
                .Products
                .OrderByDescending(x => x.CreatedOn)
                .Take(countOfProducts)
                .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<ProductViewModel>>(products);
            return viewModel;
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllAsync()
        {
            var products = await this.context
               .Products
               .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<ProductViewModel>>(products);
            return viewModel;
        }
    }
}