using Microsoft.AspNetCore.Antiforgery;

namespace TepayLink.Sdisco.Web.Controllers
{
    public class AntiForgeryController : SdiscoControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
