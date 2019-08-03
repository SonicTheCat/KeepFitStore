namespace KeepFitStore.WEB.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Models.ViewModels.Favorites;

    [Authorize]
    public class FavoritesController : BaseController
    {
        private readonly IFavoriteService favoriteService;

        public FavoritesController(IFavoriteService favoriteService)
        {
            this.favoriteService = favoriteService;
        }

        public async Task<IActionResult> Index()
        {
            var favariteProducts = await this.favoriteService
                .GetAllByUser<IndexFavoritesViewModel>(this.User.Identity.Name);

            return this.View(favariteProducts);
        }

        public async Task<IActionResult> Add(int productId)
        {
            var username = this.User.Identity.Name;
            await this.favoriteService.AddAsync(productId, username);

            return this.RedirectToAction(nameof(Index)); 
        }

        public async Task<IActionResult> Remove(int productId)
        {
            var username = this.User.Identity.Name;
            await this.favoriteService.RemoveAsync(productId, username);

            return this.RedirectToAction(nameof(Index));
        }
    }
}