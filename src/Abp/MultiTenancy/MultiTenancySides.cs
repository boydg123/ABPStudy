using System;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// Represents sides in a multi tenancy application.
    /// 在多租户应用中表示双方
    /// </summary>
    [Flags]
    public enum MultiTenancySides
    {
        /// <summary>
        /// Tenant side.
        /// 租户方
        /// </summary>
        Tenant = 1,

        /// <summary>
        /// Host (tenancy owner) side.
        /// 业主（承租人）方
        /// </summary>
        Host = 2
    }
}