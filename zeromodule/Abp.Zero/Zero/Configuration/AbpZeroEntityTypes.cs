using System;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;

namespace Abp.Zero.Configuration
{
    /// <summary>
    /// ABP Zero实体类型
    /// </summary>
    public class AbpZeroEntityTypes : IAbpZeroEntityTypes
    {
        /// <summary>
        /// 应用程序的用户类型
        /// </summary>
        public Type User
        {
            get { return _user; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (!typeof (AbpUserBase).IsAssignableFrom(value))
                {
                    throw new AbpException(value.AssemblyQualifiedName + " should be derived from " + typeof(AbpUserBase).AssemblyQualifiedName);
                }

                _user = value;
            }
        }
        private Type _user;
        /// <summary>
        /// 应用程序的角色类型
        /// </summary>
        public Type Role
        {
            get { return _role; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (!typeof(AbpRoleBase).IsAssignableFrom(value))
                {
                    throw new AbpException(value.AssemblyQualifiedName + " should be derived from " + typeof(AbpRoleBase).AssemblyQualifiedName);
                }

                _role = value;
            }
        }
        private Type _role;
        /// <summary>
        /// 应用程序的商户类型
        /// </summary>
        public Type Tenant
        {
            get { return _tenant; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (!typeof(AbpTenantBase).IsAssignableFrom(value))
                {
                    throw new AbpException(value.AssemblyQualifiedName + " should be derived from " + typeof(AbpTenantBase).AssemblyQualifiedName);
                }

                _tenant = value;
            }
        }
        private Type _tenant;
    }
}