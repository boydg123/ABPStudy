using System;

namespace Abp.Application.Features
{
    /// <summary>
    /// Scopes of a <see cref="Feature"/>.
    /// 功能范围
    /// </summary>
    [Flags]
    public enum FeatureScopes
    {
        /// <summary>
        /// This <see cref="Feature"/> can be enabled/disabled per edition.
        /// 此<see cref="Feature"/>可以由版本启用/禁用
        /// </summary>
        Edition = 1,

        /// <summary>
        /// This Feature<see cref="Feature"/> can be enabled/disabled per tenant.
        /// 此<see cref="Feature"/>可以由租户启用/禁用
        /// </summary>
        Tenant = 2,

        /// <summary>
        /// This <see cref="Feature"/> can be enabled/disabled per tenant and edition.
        /// 此<see cref="Feature"/>可以由版本/租户 启用/禁用
        /// </summary>
        All = 3
    }
}