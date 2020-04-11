using Abp.Authorization;
using TepayLink.Sdisco.Authorization.Roles;
using TepayLink.Sdisco.Authorization.Users;

namespace TepayLink.Sdisco.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
