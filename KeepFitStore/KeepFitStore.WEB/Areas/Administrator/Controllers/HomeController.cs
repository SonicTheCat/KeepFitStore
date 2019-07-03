namespace KeepFitStore.WEB.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : AdministratorController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}