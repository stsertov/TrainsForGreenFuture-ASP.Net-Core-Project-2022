namespace TrainsForGreenFuture.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static AdminConstants;

    [Area(AreaName)]
    [Authorize(Roles = AdministratorRole)]
    public abstract class AdminController : Controller
    {
    }
}
