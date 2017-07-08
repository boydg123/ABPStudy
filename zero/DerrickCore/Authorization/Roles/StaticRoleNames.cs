namespace Derrick.Authorization.Roles
{
    /// <summary>
    /// 静态角色名称
    /// </summary>
    public static class StaticRoleNames
    {
        /// <summary>
        /// 宿主
        /// </summary>
        public static class Host
        {
            public const string Admin = "Admin";
        }
        /// <summary>
        /// 商户
        /// </summary>
        public static class Tenants
        {
            public const string Admin = "Admin";

            public const string User = "User";
        }
    }
}