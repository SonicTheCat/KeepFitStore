namespace KeepFitStore.WEB.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.WEB.Controllers;
    using KeepFitStore.WEB.Common;

    [Area(WebConstants.AdministratorRoleName)]
    [Authorize(Roles = WebConstants.AdministratorRoleName)]
    public class AdministratorController : BaseController
    {
    }
}