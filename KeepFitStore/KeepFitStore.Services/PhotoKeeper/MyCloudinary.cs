namespace KeepFitStore.Services.PhotoKeeper
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;

    using KeepFitStore.Helpers;

    public class MyCloudinary : IMyCloudinary
    {
        private const string CloudinarySettingCrop = "fill";

        private const string CloudinarySettingGravity = "face";

        private const int CloudinarySettingWidth = 500;

        private const int CloudinarySettingHeight = 500;

        private readonly IOptions<CloudinarySettings> cloudinaryConfig;
        private Cloudinary cloudinary;

        public MyCloudinary(IOptions<CloudinarySettings> cloudinaryConfig)
        {
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

        public string UploadImage(IFormFile image)
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