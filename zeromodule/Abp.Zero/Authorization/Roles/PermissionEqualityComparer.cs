using System.Collections.Generic;

namespace Abp.Authorization.Roles
{
    /// <summary>
    /// Equality comparer for <see cref="Permission"/> objects.
    /// <see cref="Permission"/>对象相等比较器
    /// </summary>
    internal class PermissionEqualityComparer : IEqualityComparer<Permission>
    {
        /// <summary>
        /// 获取实例
        /// </summary>
        public static PermissionEqualityComparer Instance { get { return _instance; } }
        private static PermissionEqualityComparer _instance = new PermissionEqualityComparer();

        /// <summary>
        /// 相等比较操作
        /// </summary>
        /// <param name="x">权限X对象</param>
        /// <param name="y">权限Y对象</param>
        /// <returns></returns>
        public bool Equals(Permission x, Permission y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return Equals(x.Name, y.Name);
        }
        /// <summary>
        /// 获取权限名称的HashCode
        /// </summary>
        /// <param name="permission">权限对象</param>
        /// <returns></returns>
        public int GetHashCode(Permission permission)
        {
            return permission.Name.GetHashCode();
        }
    }
}