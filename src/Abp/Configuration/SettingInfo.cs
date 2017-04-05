using System;

namespace Abp.Configuration
{
    /// <summary>
    /// Represents a setting information.
    /// 表示一个设置的信息
    /// </summary>
    [Serializable]
    public class SettingInfo
    {
        /// <summary>
        /// TenantId for this setting.TenantId is null if this setting is not Tenant level.
        /// 此设置所属的租户Id,如果设置不是租户级别，TenantId为null
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// UserId for this setting.UserId is null if this setting is not user level.
        /// 此设置所属的用户Id,如果设置不是用户级别，UserId为null
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// Unique name of the setting.
        /// 此设置的唯一性名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value of the setting.
        /// 此设置对应的值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Creates a new <see cref="SettingInfo"/> object.
        /// 构造函数
        /// </summary>
        public SettingInfo()
        {
            
        }

        /// <summary>
        /// Creates a new <see cref="SettingInfo"/> object.
        /// 构造函数
        /// </summary>
        /// <param name="tenantId">TenantId for this setting. TenantId is null if this setting is not Tenant level. / 此设置所属的租户Id. 如果设置不是租户级别，TenantId为null.</param>
        /// <param name="userId">UserId for this setting. UserId is null if this setting is not user level. / 此设置所属的用户Id. 如果设置不是用户级别，UserId为null.</param>
        /// <param name="name">Unique name of the setting / 此设置的唯一性名称</param>
        /// <param name="value">Value of the setting / 此设置对应的值</param>
        public SettingInfo(int? tenantId, long? userId, string name, string value)
        {
            TenantId = tenantId;
            UserId = userId;
            Name = name;
            Value = value;
        }
    }
}