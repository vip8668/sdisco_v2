using Abp.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TepayLink.Sdisco.Authorization.Roles;

namespace TepayLink.Sdisco.Authorization.Users
{
    public class UserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory<User, Role>
    {
        public UserClaimsPrincipalFactory(
            UserManager userManager,
            RoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(
                  userManager,
                  roleManager,
                  optionsAccessor)
        {
        }
    }
}
