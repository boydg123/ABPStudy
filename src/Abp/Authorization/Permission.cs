using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Abp.Application.Features;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Abp.Authorization
{
    /// <summary>
    /// Represents a permission.A permission is used to restrict functionalities of the application from unauthorized users.
    /// 表示一个权限,一个权限用来限制未授权的用户使用系统的功能
    /// </summary>
    public sealed class Permission
    {
        /// <summary>
        /// Parent of this permission if one exists.If set, this permission can be granted only if parent is granted.
        /// 父权限，如果设置了父权限，就只有当父权限授权后，才能受权此权限
        /// </summary>
        public Permission Parent { get; private set; }

        /// <summary>
        /// Unique name of the permission.This is the key name to grant permissions.
        /// 权限名称，作为授权的key
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Display name of the permission.This can be used to show permission to the user.
        /// 权限的显示名称,用于像用户显示的名称
        /// </summary>
        public ILocalizableString DisplayName { get; set; }

        /// <summary>
        /// A brief description for this permission.
        /// 对于本权限的简短描述
        /// </summary>
        public ILocalizableString Description { get; set; }

        /// <summary>
        /// Which side can use this permission.
        /// 多租户双方(哪一方可以使用此权限)
        /// </summary>
        public MultiTenancySides MultiTenancySides { get; set; }

        /// <summary>
        /// Depended feature(s) of this permission.
        /// 此权限的功能依赖
        /// </summary>
        public IFeatureDependency FeatureDependency { get; set; }

        /// <summary>
        /// List of child permissions. A child permission can be granted only if parent is granted.
        /// 子权限列表，子权限只有在父权限被授予用户时才能被授予用户
        /// </summary>
        public IReadOnlyList<Permission> Children => _children.ToImmutableList();
        private readonly List<Permission> _children;

        /// <summary>
        /// Creates a new Permission.
        /// 创建一个新的权限
        /// </summary>
        /// <param name="name">Unique name of the permission / 权限名称</param>
        /// <param name="displayName">Display name of the permission / 显示名称</param>
        /// <param name="description">A brief description for this permission / 权限的简短描述</param>
        /// <param name="multiTenancySides">Which side can use this permission / 租户的哪一方可以使用此权限</param>
        /// <param name="featureDependency">Depended feature(s) of this permission / 此权限的功能依赖</param>
        public Permission(
            string name,
            ILocalizableString displayName = null,
            ILocalizableString description = null,
            MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant,
            IFeatureDependency featureDependency = null)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            Name = name;
            DisplayName = displayName;
            Description = description;
            MultiTenancySides = multiTenancySides;
            FeatureDependency = featureDependency;

            _children = new List<Permission>();
        }

        /// <summary>
        /// Adds a child permission.A child permission can be granted only if parent is granted.
        /// 添加子权限,子权限只有在父权限被授予用户时才能被授予用户
        /// </summary>
        /// <returns>Returns newly created child permission / 返回最新添加的子权限</returns>
        public Permission CreateChildPermission(
            string name, 
            ILocalizableString displayName = null, 
            ILocalizableString description = null, 
            MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant,
            IFeatureDependency featureDependency = null)
        {
            var permission = new Permission(name, displayName, description, multiTenancySides, featureDependency) { Parent = this };
            _children.Add(permission);
            return permission;
        }

        public override string ToString()
        {
            return string.Format("[Permission: {0}]", Name);
        }
    }
}
