using Abp.Authorization;
using Derrick.Authorization.Roles;
using Derrick.Authorization.Users;
using Derrick.MultiTenancy;

namespace Derrick.Authorization
{
    /// <summary>
    /// <see cref="PermissionChecker"/>的实现
    /// </summary>
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
