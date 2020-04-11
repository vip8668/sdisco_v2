using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Controllers;

namespace TepayLink.Sdisco.Web.Public.Controllers
{
    public class HomeController : SdiscoControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}