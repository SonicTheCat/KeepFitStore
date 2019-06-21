namespace KeepFitStore.WEB.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Common;

    [Area(GlobalConstants.AdministratorRoleName)]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class HomeController : AdministratorController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Settings()
        {
            return this.View(); 
        }
    }
}