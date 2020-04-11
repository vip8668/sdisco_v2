using Abp.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TepayLink.Sdisco.Authorization.Roles;
using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.MultiTenancy;

namespace TepayLink.Sdisco.Identity
{
    public class SecurityStampValidator : AbpSecurityStampValidator<Tenant, Role, User>
    {
        public SecurityStampValidator(
            IOptions<SecurityStampValidatorOptions> options,
            SignInManager signInManager,
            ISystemClock systemClock,
            ILoggerFactory loggerFactory)
            : base(options, signInManager, systemClock, loggerFactory)
        {
        }
    }
}