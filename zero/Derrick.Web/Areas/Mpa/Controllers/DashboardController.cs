using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using Derrick.Authorization;
using Derrick.Web.Controllers;

namespace Derrick.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class DashboardController : AbpZeroTemplateControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}