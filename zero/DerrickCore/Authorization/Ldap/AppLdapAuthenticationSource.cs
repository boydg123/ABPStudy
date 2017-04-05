using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using Derrick.Authorization.Users;
using Derrick.MultiTenancy;

namespace Derrick.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}
