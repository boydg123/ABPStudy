using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using Derrick.Web.Controllers;

namespace Derrick.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize]
    public class WelcomeController : AbpZeroTemplateControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}