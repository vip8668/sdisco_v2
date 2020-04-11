using System.Threading.Tasks;
using Abp.Authorization.Users;
using TepayLink.Sdisco.Authorization.Users;

namespace TepayLink.Sdisco.Authorization
{
    public static class UserManagerExtensions
    {
        public static async Task<User> GetAdminAsync(this UserManager userManager)
        {
            return await userManager.FindByNameAsync(AbpUserBase.AdminUserName);
        }
    }
}
