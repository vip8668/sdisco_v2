using Abp.AspNetCore.Mvc.Authorization;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Storage;
using Abp.BackgroundJobs;

namespace TepayLink.Sdisco.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}