using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Controllers;

namespace TepayLink.Sdisco.Web.Public.Controllers
{
    public class AboutController : SdiscoControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}