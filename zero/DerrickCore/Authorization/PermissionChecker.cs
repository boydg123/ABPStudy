using Abp.Authorization;
using Derrick.Authorization.Roles;
using Derrick.Authorization.Users;
using Derrick.MultiTenancy;

namespace Derrick.Authorization
{
    /// <summary>
    /// Implements <see cref="PermissionChecker"/>.
    /// </summary>
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
