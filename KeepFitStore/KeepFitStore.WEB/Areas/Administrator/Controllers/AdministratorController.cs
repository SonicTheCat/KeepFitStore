namespace KeepFitStore.WEB.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.WEB.Controllers;
    
    [Area("Administrator")]
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : BaseController
    {
    }
}