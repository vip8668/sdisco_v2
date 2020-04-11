using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TepayLink.Sdisco.Authentication.TwoFactor.Google;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Authorization.Roles;
using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.Editions;
using TepayLink.Sdisco.MultiTenancy;

namespace TepayLink.Sdisco.Identity
{
    public static class IdentityRegistrar
    {
        public static IdentityBuilder Register(IServiceCollection services)
        {
            services.AddLogging();

            return services.AddAbpIdentity<Tenant, User, Role>(options =>
                {
                    options.Tokens.ProviderMap[GoogleAuthenticatorProvider.Name] = new TokenProviderDescriptor(typeof(GoogleAuthenticatorProvider));
                })
                .AddAbpTenantManager<TenantManager>()
                .AddAbpUserManager<UserManager>()
                .AddAbpRoleManager<RoleManager>()
                .AddAbpEditionManager<EditionManager>()
                .AddAbpUserStore<UserStore>()
                .AddAbpRoleStore<RoleStore>()
                .AddAbpSignInManager<SignInManager>()
                .AddAbpUserClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
                .AddAbpSecurityStampValidator<SecurityStampValidator>()
                .AddPermissionChecker<PermissionChecker>()
                .AddDefaultTokenProviders();
        }
    }
}
