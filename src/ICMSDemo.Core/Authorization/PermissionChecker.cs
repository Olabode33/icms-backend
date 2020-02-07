using Abp.Authorization;
using ICMSDemo.Authorization.Roles;
using ICMSDemo.Authorization.Users;

namespace ICMSDemo.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
