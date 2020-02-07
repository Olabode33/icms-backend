using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using ICMSDemo.Authorization.Users;
using ICMSDemo.MultiTenancy;

namespace ICMSDemo.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}