using Abp.AspNetCore.Mvc.Authorization;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
        }
    }
}