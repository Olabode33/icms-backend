using System.Threading.Tasks;
using Abp.Authorization.Users;
using ICMSDemo.Authorization.Users;

namespace ICMSDemo.Authorization
{
    public static class UserManagerExtensions
    {
        public static async Task<User> GetAdminAsync(this UserManager userManager)
        {
            return await userManager.FindByNameAsync(AbpUserBase.AdminUserName);
        }
    }
}
