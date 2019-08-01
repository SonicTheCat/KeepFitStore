namespace KeepFitStore.Services.PhotoKeeper
{
    using Microsoft.AspNetCore.Http;

    public interface IMyCloudinary
    {
        string UploadImage(IFormFile image);
    }
}