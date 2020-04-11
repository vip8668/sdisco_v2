using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Controllers;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize]
    public class WelcomeController : SdiscoControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}