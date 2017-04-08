using System.Collections.Generic;
using System.Linq;

namespace Abp.Authorization
{
    /// <summary>
    /// Used to store and manipulate dictionary of permissions.
    /// 用于存储和操作权限权限的字典
    /// </summary>
    internal class PermissionDictionary : Dictionary<string, Permission>
    {
        /// <summary>
        /// Adds all child permissions of current permissions recursively.
        /// 递归添加当前权限的所有子权限，这很有必要
        /// </summary>
        public void AddAllPermissions()
        {
            foreach (var permission in Values.ToList())
            {
                AddPermissionRecursively(permission);
            }
        }

        /// <summary>
        /// Adds a permission and it's all child permissions to dictionary.
        /// 添加一个权限和它所有子权限到字黄
        /// </summary>
        /// <param name="permission">Permission to be added / 需要添加的权限</param>
        private void AddPermissionRecursively(Permission permission)
        {
            //Prevent multiple adding of same named permission.
            //阻止添加多个相同名称的权限
            Permission existingPermission;
            if (TryGetValue(permission.Name, out existingPermission))
            {
                if (existingPermission != permission)
                {
                    throw new AbpInitializationException("Duplicate permission name detected for " + permission.Name);                    
                }
            }
            else
            {
                this[permission.Name] = permission;
            }

            //Add child permissions (recursive call)
            //添加子权限 (递归调用)
            foreach (var childPermission in permission.Children)
            {
                AddPermissionRecursively(childPermission);
            }
        }
    }
}