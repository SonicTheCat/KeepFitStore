namespace KeepFitStore.WEB.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;

    using ViewModels;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Delivery()
        {
            return this.View(); 
        }

        public IActionResult Info()
        {
            return this.View();
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}