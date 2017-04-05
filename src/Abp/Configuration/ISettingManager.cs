using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Configuration
{
    /// <summary>
    /// This is the main interface that must be implemented to be able to load/change values of settings.
    /// 这是要加载/改变设置值，必须要实现的主要接口
    /// </summary>
    public interface ISettingManager
    {
        /// <summary>
        /// Gets current value of a setting.It gets the setting value, overwritten by application, current tenant and current user if exists.
        /// 获取设置的当前值，为应用获取，当前租户和当前用户重写的值
        /// </summary>
        /// <param name="name">Unique name of the setting / 设置的名称</param>
        /// <returns>Current value of the setting / 设置的当前值</returns>
        Task<string> GetSettingValueAsync(string name);

        /// <summary>
        /// Gets current value of a setting for the application level.
        /// 获取应用程序级别的设置值
        /// </summary>
        /// <param name="name">Unique name of the setting / 设置名称</param>
        /// <returns>Current value of the setting for the application / 应用程序的当前设置值</returns>
        Task<string> GetSettingValueForApplicationAsync(string name);

        /// <summary>
        /// Gets current value of a setting for a tenant level.It gets the setting value, overwritten by given tenant.
        /// 获取租户级别的设置值
        /// </summary>
        /// <param name="name">Unique name of the setting / 设置名称</param>
        /// <param name="tenantId">Tenant id / 租户Id</param>
        /// <returns>Current value of the setting / 租户的当前设置值</returns>
        Task<string> GetSettingValueForTenantAsync(string name, int tenantId);

        /// <summary>
        /// Gets current value of a setting for a user level.It gets the setting value, overwritten by given tenant and user.
        /// 获取用户级别的设置值
        /// </summary>
        /// <param name="name">Unique name of the setting / 设置名称</param>
        /// <param name="tenantId">Tenant id / 租户Id</param>
        /// <param name="userId">User id / 用户Id</param>
        /// <returns>Current value of the setting for the user / 用户的当前设置值</returns>
        Task<string> GetSettingValueForUserAsync(string name, int? tenantId, long userId); //TODO: Can be overloaded for UserIdentifier.

        /// <summary>
        /// Gets current values of all settings.It gets all setting values, overwritten by application, current tenant (if exists) and the current user (if exists).
        /// 获取所有设置的当前值,获取为应用程序，当前租户和当前用户重写的值
        /// </summary>
        /// <returns>List of setting values / 设置值集合</returns>
        Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesAsync();

        /// <summary>
        /// Gets current values of all settings.It gets default values of all settings then overwrites by given scopes.
        /// 获取所有设置的当前值,获取所有设置的默认值，以及为给定作用域重写的值。
        /// </summary>
        /// <param name="scopes">One or more scope to overwrite / 作用域</param>
        /// <returns>List of setting values / 设置值集合</returns>
        Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesAsync(SettingScopes scopes);

        /// <summary>
        /// Gets a list of all setting values specified for the application.
        /// It returns only settings those are explicitly set for the application.
        /// If a setting's default value is used, it's not included the result list.
        /// If you want to get current values of all settings, use <see cref="GetAllSettingValuesAsync()"/> method.
        /// 获取应用程序级别的所有设置值
        /// 只返回显式为应用程序设置的设置
        /// 如果一个设置使用默认值，它将不包含在结果集中
        /// 如果你想获取所有设置的当前值，请使用方法<see cref="GetAllSettingValuesAsync()"/>
        /// </summary>
        /// <returns>List of setting values / 设置值列表</returns>
        Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForApplicationAsync();

        /// <summary>
        /// Gets a list of all setting values specified for a tenant.
        /// It returns only settings those are explicitly set for the tenant.
        /// If a setting's default value is used, it's not included the result list.
        /// If you want to get current values of all settings, use <see cref="GetAllSettingValuesAsync()"/> method.
        /// 获取租户级别的所有设置值
        /// 只返回显式为租户设置的设置
        /// 如果一个设置使用默认值，它将不包含在结果集中
        /// 如果你想获取所有设置的当前值，请使用方法<see cref="GetAllSettingValuesAsync()"/>
        /// </summary>
        /// <param name="tenantId">Tenant to get settings / 租户Id</param>
        /// <returns>List of setting values / 设置值列表</returns>
        Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForTenantAsync(int tenantId);

        /// <summary>
        /// Gets a list of all setting values specified for a user.
        /// It returns only settings those are explicitly set for the user.
        /// If a setting's value is not set for the user (for example if user uses the default value), it's not included the result list.
        /// If you want to get current values of all settings, use <see cref="GetAllSettingValuesAsync()"/> method.
        /// 获取用户级别的所有设置值
        /// 只返回显式为用户设置的设置
        /// 如果一个设置使用默认值，它将不包含在结果集中
        /// 如果你想获取所有设置的当前值，请使用方法<see cref="GetAllSettingValuesAsync()"/>
        /// </summary>
        /// <param name="user">User to get settings / 设置的用户</param>
        /// <returns>All settings of the user / 设置值列表</returns>
        Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForUserAsync(UserIdentifier user);

        /// <summary>
        /// Changes setting for the application level.
        /// 修改应用程序级别的设置
        /// </summary>
        /// <param name="name">Unique name of the setting / 设置名称</param>
        /// <param name="value">Value of the setting / 设置值</param>
        Task ChangeSettingForApplicationAsync(string name, string value);

        /// <summary>
        /// Changes setting for a Tenant.
        /// 修改租户级别的设置
        /// </summary>
        /// <param name="tenantId">TenantId / 租户Id</param>
        /// <param name="name">Unique name of the setting / 设置名称</param>
        /// <param name="value">Value of the setting / 设置值</param>
        Task ChangeSettingForTenantAsync(int tenantId, string name, string value);

        /// <summary>
        /// Changes setting for a user.
        /// 修改用户级别的设置
        /// </summary>
        /// <param name="user">UserId / 用户Id</param>
        /// <param name="name">Unique name of the setting / 设置名称</param>
        /// <param name="value">Value of the setting / 设置值</param>
        Task ChangeSettingForUserAsync(UserIdentifier user, string name, string value);
    }
}
