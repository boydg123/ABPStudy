using System;

namespace Abp.Configuration
{
    /// <summary>
    /// Defines scope of a setting.
    /// 定义设置的作用域
    /// </summary>
    [Flags]
    public enum SettingScopes
    {
        /// <summary>
        /// Represents a setting that can be configured/changed for the application level.
        /// 表示设置能被应用程序配置/修改
        /// </summary>
        Application = 1,

        /// <summary>
        /// Represents a setting that can be configured/changed for each Tenant.This is reserved
        /// 表示设置能被租户配置/修改,这是预留的
        /// </summary>
        Tenant = 2,

        /// <summary>
        /// Represents a setting that can be configured/changed for each User.
        /// 表示设置能被用户配置/修改
        /// </summary>
        User = 4
    }
}