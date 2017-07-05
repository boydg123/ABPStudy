using System;

namespace Abp.Zero.Configuration
{
    /// <summary>
    /// ABP Zero实体类型
    /// </summary>
    public interface IAbpZeroEntityTypes
    {
        /// <summary>
        /// 应用程序的用户类型
        /// </summary>
        Type User { get; set; }

        /// <summary>
        /// 应用程序的角色类型
        /// </summary>
        Type Role { get; set; }

        /// <summary>
        /// 应用程序的商户类型
        /// </summary>
        Type Tenant { get; set; }
    }
}