using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace TepayLink.Sdisco.Web.Controllers
{
    public class HomeController : SdiscoControllerBase
    {
        [DisableAuditing]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Ui");
        }
    }
}
