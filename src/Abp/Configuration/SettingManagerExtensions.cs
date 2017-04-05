using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.Threading;

namespace Abp.Configuration
{
    /// <summary>
    /// Extension methods for <see cref="ISettingManager"/>.
    /// <see cref="ISettingManager"/>扩展方法.
    /// </summary>
    public static class SettingManagerExtensions
    {
        /// <summary>
        /// Gets value of a setting in given type (<see cref="T"/>).
        /// 获取一个给定类型(<see cref="T"/>)的设置值
        /// </summary>
        /// <typeparam name="T">Type of the setting to get / 给定设置的类型</typeparam>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        /// <param name="name">Unique name of the setting / 设置的唯一性名称</param>
        /// <returns>Value of the setting / 设置值</returns>
        public static async Task<T> GetSettingValueAsync<T>(this ISettingManager settingManager, string name)
            where T : struct
        {
            return (await settingManager.GetSettingValueAsync(name)).To<T>();
        }

        /// <summary>
        /// Gets current value of a setting for the application level.
        /// 获取应用级别设置的当前值
        /// </summary>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        /// <param name="name">Unique name of the setting / 设置的唯一性名称</param>
        /// <returns>Current value of the setting for the application / 应用级别设置的当前值</returns>
        public static async Task<T> GetSettingValueForApplicationAsync<T>(this ISettingManager settingManager, string name)
            where T : struct
        {
            return (await settingManager.GetSettingValueForApplicationAsync(name)).To<T>();
        }

        /// <summary>
        /// Gets current value of a setting for a tenant level.It gets the setting value, overwritten by given tenant.
        /// 获取租户级别设置的当前值,获取的设置值被租户重写（覆盖）了
        /// </summary>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        /// <param name="name">Unique name of the setting / 设置的唯一性名称</param>
        /// <param name="tenantId">Tenant id / 租户Id</param>
        /// <returns>Current value of the setting / 设置的当前值</returns>
        public static async Task<T> GetSettingValueForTenantAsync<T>(this ISettingManager settingManager, string name, int tenantId)
           where T : struct
        {
            return (await settingManager.GetSettingValueForTenantAsync(name, tenantId)).To<T>();
        }

        /// <summary>
        /// Gets current value of a setting for a user level.It gets the setting value, overwritten by given tenant and user.
        /// 获取用户级别设置的当前值,获取的设置值被给定的用户或租户重写（覆盖）了
        /// </summary>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        /// <param name="name">Unique name of the setting / 设置的唯一性名称</param>
        /// <param name="tenantId">Tenant id / 租户Id</param>
        /// <param name="userId">User id / 用户id</param>
        /// <returns>Current value of the setting for the user / 设置的当前值</returns>
        public static async Task<T> GetSettingValueForUserAsync<T>(this ISettingManager settingManager, string name, int? tenantId, long userId)
           where T : struct
        {
            return (await settingManager.GetSettingValueForUserAsync(name, tenantId, userId)).To<T>();
        }

        /// <summary>
        /// Gets current value of a setting.It gets the setting value, overwritten by application and the current user if exists.
        /// 获取设置的当前值,获取的设置值被应用或用户（如果存在）重写（覆盖）了
        /// </summary>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        /// <param name="name">Unique name of the setting / 设置的唯一性名称</param>
        /// <returns>Current value of the setting / 设置的当前值</returns>
        public static string GetSettingValue(this ISettingManager settingManager, string name)
        {
            return AsyncHelper.RunSync(() => settingManager.GetSettingValueAsync(name));
        }

        /// <summary>
        /// Gets current value of a setting for the application level.
        /// 获取应用级别设置的当前值
        /// </summary>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        /// <param name="name">Unique name of the setting / 设置的唯一性名称</param>
        /// <returns>Current value of the setting for the application / 应用级别设置的当前值</returns>
        public static string GetSettingValueForApplication(this ISettingManager settingManager, string name)
        {
            return AsyncHelper.RunSync(() => settingManager.GetSettingValueForApplicationAsync(name));
        }

        /// <summary>
        /// Gets current value of a setting for a tenant level.It gets the setting value, overwritten by given tenant.
        /// 获取租户级别设置的当前值，获取的设置值被租户重写（覆盖）了
        /// </summary>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        /// <param name="name">Unique name of the setting / 设置的唯一性名称</param>
        /// <param name="tenantId">Tenant id / 租户Id</param>
        /// <returns>Current value of the setting / 设置的当前值</returns>
        public static string GetSettingValueForTenant(this ISettingManager settingManager, string name, int tenantId)
        {
            return AsyncHelper.RunSync(() => settingManager.GetSettingValueForTenantAsync(name, tenantId));
        }

        /// <summary>
        /// Gets current value of a setting for a user level.It gets the setting value, overwritten by given tenant and user.
        /// 获取用户级别设置的当前值，获取的设置值被给定的用户或租户重写（覆盖）了
        /// </summary>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        /// <param name="name">Unique name of the setting / 设置的唯一性名称</param>
        /// <param name="tenantId">Tenant id / 租户Id</param>
        /// <param name="userId">User id / 用户id</param>
        /// <returns>Current value of the setting for the user / 设置的当前值</returns>
        public static string GetSettingValueForUser(this ISettingManager settingManager, string name, int? tenantId, long userId)
        {
            return AsyncHelper.RunSync(() => settingManager.GetSettingValueForUserAsync(name, tenantId, userId));
        }

        /// <summary>
        /// Gets value of a setting.
        /// 获取设置的当前值
        /// </summary>
        /// <typeparam name="T">Type of the setting to get / 获取设置的类型</typeparam>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        /// <param name="name">Unique name of the setting / 设置的唯一性名称</param>
        /// <returns>Value of the setting / 设置的当前值</returns>
        public static T GetSettingValue<T>(this ISettingManager settingManager, string name)
            where T : struct
        {
            return AsyncHelper.RunSync(() => settingManager.GetSettingValueAsync<T>(name));
        }

        /// <summary>
        /// Gets current value of a setting for the application level.
        /// 获取应用级别设置的当前值
        /// </summary>
        /// <typeparam name="T">Type of the setting to get / 获取设置的类型</typeparam>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        /// <param name="name">Unique name of the setting / 设置的唯一性名称</param>
        /// <returns>Current value of the setting for the application / 应用级别设置的当前值</returns>
        public static T GetSettingValueForApplication<T>(this ISettingManager settingManager, string name)
            where T : struct
        {
            return AsyncHelper.RunSync(() => settingManager.GetSettingValueForApplicationAsync<T>(name));
        }

        /// <summary>
        /// Gets current value of a setting for a tenant level.It gets the setting value, overwritten by given tenant.
        /// 获取租户级别设置的当前值,获取的设置值被租户重写（覆盖）了
        /// </summary>
        /// <typeparam name="T">Type of the setting to get / 获取设置的类型</typeparam>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        /// <param name="name">Unique name of the setting / 设置的唯一性名称</param>
        /// <param name="tenantId">Tenant id / 租户Id</param>
        /// <returns>Current value of the setting / 设置的当前值</returns>
        public static T GetSettingValueForTenant<T>(this ISettingManager settingManager, string name, int tenantId)
            where T : struct
        {
            return AsyncHelper.RunSync(() => settingManager.GetSettingValueForTenantAsync<T>(name, tenantId));
        }

        /// <summary>
        /// Gets current value of a setting for a user level.It gets the setting value, overwritten by given tenant and user.
        /// 获取用户级别设置的当前值,获取的设置值被给定的用户或租户重写（覆盖）了
        /// </summary>
        /// <typeparam name="T">Type of the setting to get / 获取设置的类型</typeparam>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        /// <param name="name">Unique name of the setting / 设置的唯一性名称</param>
        /// <param name="tenantId">Tenant id / 租户Id</param>
        /// <param name="userId">User id / 用户id</param>
        /// <returns>Current value of the setting for the user / 设置的当前值</returns>
        public static T GetSettingValueForUser<T>(this ISettingManager settingManager, string name, int? tenantId, long userId)
            where T : struct
        {
            return AsyncHelper.RunSync(() => settingManager.GetSettingValueForUserAsync<T>(name, tenantId, userId));
        }

        /// <summary>
        /// Gets current values of all settings.It gets all setting values, overwritten by application and the current user if exists.
        /// 获取所有设置的当前值,获取的所有设置值被应用或用户（如果存在）重写（覆盖）了
        /// </summary>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        /// <returns>List of setting values</returns>
        public static IReadOnlyList<ISettingValue> GetAllSettingValues(this ISettingManager settingManager)
        {
            return AsyncHelper.RunSync(settingManager.GetAllSettingValuesAsync);
        }

        /// <summary>
        /// Gets a list of all setting values specified for the application.
        /// It returns only settings those are explicitly set for the application.
        /// If a setting's default value is used, it's not included the result list.
        /// If you want to get current values of all settings, use <see cref="GetAllSettingValues"/> method.
        /// 获取给定应用的所有设置值。
        /// 仅返回指定应用明确使用的设置。
        /// 如果设置使用的是默认值，则该设置将不包含在结果集中。
        /// 如果你想获取所有设置的值，请使用方法<see cref="GetAllSettingValues"/>
        /// </summary>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        /// <returns>List of setting values / 设置值列表</returns>
        public static IReadOnlyList<ISettingValue> GetAllSettingValuesForApplication(this ISettingManager settingManager)
        {
            return AsyncHelper.RunSync(settingManager.GetAllSettingValuesForApplicationAsync);
        }

        /// <summary>
        /// Gets a list of all setting values specified for a tenant.
        /// It returns only settings those are explicitly set for the tenant.
        /// If a setting's default value is used, it's not included the result list.
        /// If you want to get current values of all settings, use <see cref="GetAllSettingValues"/> method.
        /// 获取给定租户的所有设置值。
        /// 仅返回指定租户明确使用的设置。
        /// 如果设置使用的是默认值，则该设置将不包含在结果集中。
        /// 如果你想获取所有设置的值，请使用方法<see cref="GetAllSettingValues"/>
        /// </summary>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        /// <param name="tenantId">Tenant to get settings / 租户Id</param>
        /// <returns>List of setting values / 设置值列表</returns>
        public static IReadOnlyList<ISettingValue> GetAllSettingValuesForTenant(this ISettingManager settingManager, int tenantId)
        {
            return AsyncHelper.RunSync(() => settingManager.GetAllSettingValuesForTenantAsync(tenantId));
        }

        /// <summary>
        /// Gets a list of all setting values specified for a user.
        /// It returns only settings those are explicitly set for the user.
        /// If a setting's value is not set for the user (for example if user uses the default value), it's not included the result list.
        /// If you want to get current values of all settings, use <see cref="GetAllSettingValues"/> method.
        /// 获取给定用户的所有设置值。
        /// 仅返回指定用户明确使用的设置。
        /// 如果设置使用的是默认值，则该设置将不包含在结果集中。
        /// 如果你想获取所有设置的值，请使用方法<see cref="GetAllSettingValues"/>
        /// </summary>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        /// <param name="user">User to get settings / 获取设置的用户</param>
        /// <returns>All settings of the user / 设置值列表</returns>
        public static IReadOnlyList<ISettingValue> GetAllSettingValuesForUser(this ISettingManager settingManager, UserIdentifier user)
        {
            return AsyncHelper.RunSync(() => settingManager.GetAllSettingValuesForUserAsync(user));
        }

        /// <summary>
        /// Changes setting for the application level.
        /// 修改应用级别的设置
        /// </summary>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        /// <param name="name">Unique name of the setting / 设置的唯一性名称</param>
        /// <param name="value">Value of the setting / 设置的值</param>
        public static void ChangeSettingForApplication(this ISettingManager settingManager, string name, string value)
        {
            AsyncHelper.RunSync(() => settingManager.ChangeSettingForApplicationAsync(name, value));
        }

        /// <summary>
        /// Changes setting for a Tenant.
        /// 修改租户级别的设置
        /// </summary>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        /// <param name="tenantId">TenantId / 租户Id</param>
        /// <param name="name">Unique name of the setting / 设置的唯一性名称</param>
        /// <param name="value">Value of the setting / 设置的值</param>
        public static void ChangeSettingForTenant(this ISettingManager settingManager, int tenantId, string name, string value)
        {
            AsyncHelper.RunSync(() => settingManager.ChangeSettingForTenantAsync(tenantId, name, value));
        }

        /// <summary>
        /// Changes setting for a user.
        /// 修改用户级别的设置
        /// </summary>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        /// <param name="user">User / 用户Id</param>
        /// <param name="name">Unique name of the setting / 设置的唯一性名称</param>
        /// <param name="value">Value of the setting / 设置的值</param>
        public static void ChangeSettingForUser(this ISettingManager settingManager, UserIdentifier user, string name, string value)
        {
            AsyncHelper.RunSync(() => settingManager.ChangeSettingForUserAsync(user, name, value));
        }
    }
}