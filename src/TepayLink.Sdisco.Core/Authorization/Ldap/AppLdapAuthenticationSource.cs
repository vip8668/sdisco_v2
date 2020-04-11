using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.MultiTenancy;

namespace TepayLink.Sdisco.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}