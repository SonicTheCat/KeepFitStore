namespace KeepFitStore.WEB.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.WEB.Controllers;
    using KeepFitStore.Common;

    [Area(GlobalConstants.AdministratorRoleName)]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class AdministratorController : BaseController
    {
    }
}