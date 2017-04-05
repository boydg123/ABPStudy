using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;

namespace Abp.Configuration
{
    /// <summary>
    /// This class implements <see cref="ISettingManager"/> to manage setting values in the database.
    /// 此类实现接口<see cref="ISettingManager"/>，管理数据库中的设置值
    /// </summary>
    public class SettingManager : ISettingManager, ISingletonDependency
    {
        /// <summary>
        /// 应用程序设置缓存键
        /// </summary>
        public const string ApplicationSettingsCacheKey = "ApplicationSettings";

        /// <summary>
        /// Reference to the current Session.
        /// 当前会话的引用
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        /// <summary>
        /// Reference to the setting store.
        /// 设置存储的引用
        /// </summary>
        public ISettingStore SettingStore { get; set; }

        /// <summary>
        /// 设置定义管理器
        /// </summary>
        private readonly ISettingDefinitionManager _settingDefinitionManager;
        private readonly ITypedCache<string, Dictionary<string, SettingInfo>> _applicationSettingCache;
        private readonly ITypedCache<int, Dictionary<string, SettingInfo>> _tenantSettingCache;
        private readonly ITypedCache<string, Dictionary<string, SettingInfo>> _userSettingCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="settingDefinitionManager">设置定义管理器</param>
        /// <param name="cacheManager">缓存管理器</param>
        public SettingManager(ISettingDefinitionManager settingDefinitionManager, ICacheManager cacheManager)
        {
            _settingDefinitionManager = settingDefinitionManager;

            AbpSession = NullAbpSession.Instance;
            SettingStore = DefaultConfigSettingStore.Instance;

            _applicationSettingCache = cacheManager.GetApplicationSettingsCache();
            _tenantSettingCache = cacheManager.GetTenantSettingsCache();
            _userSettingCache = cacheManager.GetUserSettingsCache();
        }

        #region Public methods 公共方法

        /// <summary>
        /// 获取设置的当前值，为应用获取，当前租户和当前用户重写的值
        /// </summary>
        /// <param name="name">设置的名称</param>
        /// <returns>设置的当前值</returns>
        public Task<string> GetSettingValueAsync(string name)
        {
            return GetSettingValueInternalAsync(name, AbpSession.TenantId, AbpSession.UserId);
        }

        /// <summary>
        /// 获取应用程序级别的设置值
        /// </summary>
        /// <param name="name">设置名称</param>
        /// <returns>应用程序的当前设置值</returns>
        public Task<string> GetSettingValueForApplicationAsync(string name)
        {
            return GetSettingValueInternalAsync(name);
        }

        /// <summary>
        /// 获取租户级别的设置值
        /// </summary>
        /// <param name="name">设置名称</param>
        /// <param name="tenantId">租户Id</param>
        /// <returns>租户的当前设置值</returns>
        public Task<string> GetSettingValueForTenantAsync(string name, int tenantId)
        {
            return GetSettingValueInternalAsync(name, tenantId);
        }

        /// <summary>
        /// 获取用户级别的设置值
        /// </summary>
        /// <param name="name">设置名称</param>
        /// <param name="tenantId">租户Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>用户的当前设置值</returns>
        public Task<string> GetSettingValueForUserAsync(string name, int? tenantId, long userId)
        {
            return GetSettingValueInternalAsync(name, tenantId, userId);
        }

        /// <summary>
        /// 获取所有设置的当前值,获取为应用程序，当前租户和当前用户重写的值
        /// </summary>
        /// <returns>设置值集合</returns>
        public async Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesAsync()
        {
            return await GetAllSettingValuesAsync(SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User);
        }

        /// <summary>
        /// 获取所有设置的当前值
        /// 获取所有设置的默认值，以及为给定作用域重写的值
        /// </summary>
        /// <param name="scopes">作用域</param>
        /// <returns>设置值集合</returns>
        public async Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesAsync(SettingScopes scopes)
        {
            var settingDefinitions = new Dictionary<string, SettingDefinition>();
            var settingValues = new Dictionary<string, ISettingValue>();

            //Fill all setting with default values.
            foreach (var setting in _settingDefinitionManager.GetAllSettingDefinitions())
            {
                settingDefinitions[setting.Name] = setting;
                settingValues[setting.Name] = new SettingValueObject(setting.Name, setting.DefaultValue);
            }

            //Overwrite application settings
            if (scopes.HasFlag(SettingScopes.Application))
            {
                foreach (var settingValue in await GetAllSettingValuesForApplicationAsync())
                {
                    var setting = settingDefinitions.GetOrDefault(settingValue.Name);

                    //TODO: Conditions get complicated, try to simplify it
                    if (setting == null || !setting.Scopes.HasFlag(SettingScopes.Application))
                    {
                        continue;
                    }

                    if (!setting.IsInherited &&
                        ((setting.Scopes.HasFlag(SettingScopes.Tenant) && AbpSession.TenantId.HasValue) || (setting.Scopes.HasFlag(SettingScopes.User) && AbpSession.UserId.HasValue)))
                    {
                        continue;
                    }

                    settingValues[settingValue.Name] = new SettingValueObject(settingValue.Name, settingValue.Value);
                }
            }

            //Overwrite tenant settings
            if (scopes.HasFlag(SettingScopes.Tenant) && AbpSession.TenantId.HasValue)
            {
                foreach (var settingValue in await GetAllSettingValuesForTenantAsync(AbpSession.TenantId.Value))
                {
                    var setting = settingDefinitions.GetOrDefault(settingValue.Name);

                    //TODO: Conditions get complicated, try to simplify it
                    if (setting == null || !setting.Scopes.HasFlag(SettingScopes.Tenant))
                    {
                        continue;
                    }

                    if (!setting.IsInherited &&
                        (setting.Scopes.HasFlag(SettingScopes.User) && AbpSession.UserId.HasValue))
                    {
                        continue;
                    }

                    settingValues[settingValue.Name] = new SettingValueObject(settingValue.Name, settingValue.Value);
                }
            }

            //Overwrite user settings
            if (scopes.HasFlag(SettingScopes.User) && AbpSession.UserId.HasValue)
            {
                foreach (var settingValue in await GetAllSettingValuesForUserAsync(AbpSession.ToUserIdentifier()))
                {
                    var setting = settingDefinitions.GetOrDefault(settingValue.Name);
                    if (setting != null && setting.Scopes.HasFlag(SettingScopes.User))
                    {
                        settingValues[settingValue.Name] = new SettingValueObject(settingValue.Name, settingValue.Value);
                    }
                }
            }

            return settingValues.Values.ToImmutableList();
        }

        /// <summary>
        /// 获取应用程序级别的所有设置值
        /// 只返回显式为应用程序设置的设置
        /// 如果一个设置使用默认值，它将不包含在结果集中
        /// 如果你想获取所有设置的当前值，请使用方法<see cref="GetAllSettingValuesAsync()"/>
        /// </summary>
        /// <returns>设置值列表</returns>
        public async Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForApplicationAsync()
        {
            return (await GetApplicationSettingsAsync()).Values
                .Select(setting => new SettingValueObject(setting.Name, setting.Value))
                .ToImmutableList();
        }

        /// <summary>
        /// 获取租户级别的所有设置值
        /// 只返回显式为租户设置的设置
        /// 如果一个设置使用默认值，它将不包含在结果集中
        /// 如果你想获取所有设置的当前值，请使用方法<see cref="GetAllSettingValuesAsync()"/>
        /// </summary>
        /// <param name="tenantId">租户Id</param>
        /// <returns>设置值列表</returns>
        public async Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForTenantAsync(int tenantId)
        {
            return (await GetReadOnlyTenantSettings(tenantId)).Values
                .Select(setting => new SettingValueObject(setting.Name, setting.Value))
                .ToImmutableList();
        }

        /// <summary>
        /// 获取用户级别的所有设置值
        /// 只返回显式为用户设置的设置
        /// 如果一个设置使用默认值，它将不包含在结果集中
        /// 如果你想获取所有设置的当前值，请使用方法<see cref="GetAllSettingValuesAsync()"/>
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>设置值列表</returns>
        public Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForUserAsync(long userId)
        {
            return GetAllSettingValuesForUserAsync(new UserIdentifier(AbpSession.TenantId, userId));
        }

        /// <summary>
        /// 获取用户级别的所有设置值
        /// 只返回显式为用户设置的设置
        /// 如果一个设置使用默认值，它将不包含在结果集中
        /// 如果你想获取所有设置的当前值，请使用方法<see cref="GetAllSettingValuesAsync()"/>
        /// </summary>
        /// <param name="user">设置的用户</param>
        /// <returns>设置值列表</returns>
        public async Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForUserAsync(UserIdentifier user)
        {
            return (await GetReadOnlyUserSettings(user)).Values
                .Select(setting => new SettingValueObject(setting.Name, setting.Value))
                .ToImmutableList();
        }

        /// <summary>
        /// 修改应用程序级别的设置
        /// </summary>
        /// <param name="name">设置名称</param>
        /// <param name="value">设置值</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task ChangeSettingForApplicationAsync(string name, string value)
        {
            await InsertOrUpdateOrDeleteSettingValueAsync(name, value, null, null);
            await _applicationSettingCache.RemoveAsync(ApplicationSettingsCacheKey);
        }

        /// <summary>
        /// 修改租户级别的设置
        /// </summary>
        /// <param name="tenantId">租户Id</param>
        /// <param name="name">设置名称</param>
        /// <param name="value">设置值</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task ChangeSettingForTenantAsync(int tenantId, string name, string value)
        {
            await InsertOrUpdateOrDeleteSettingValueAsync(name, value, tenantId, null);
            await _tenantSettingCache.RemoveAsync(tenantId);
        }

        /// <summary>
        /// 修改用户级别的设置
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="name">设置名称</param>
        /// <param name="value">设置值</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual Task ChangeSettingForUserAsync(long userId, string name, string value)
        {
            return ChangeSettingForUserAsync(new UserIdentifier(AbpSession.TenantId, userId), name, value);
        }

        /// <summary>
        /// 修改用户级别的设置
        /// </summary>
        /// <param name="user">用户标识</param>
        /// <param name="name">设置名称</param>
        /// <param name="value">设置值</param>
        /// <returns></returns>
        public async Task ChangeSettingForUserAsync(UserIdentifier user, string name, string value)
        {
            await InsertOrUpdateOrDeleteSettingValueAsync(name, value, user.TenantId, user.UserId);
            await _userSettingCache.RemoveAsync(user.ToUserIdentifierString());
        }

        #endregion

        #region Private methods 私有方法

        /// <summary>
        /// 获取设置值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="tenantId">租房Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        private async Task<string> GetSettingValueInternalAsync(string name, int? tenantId = null, long? userId = null)
        {
            var settingDefinition = _settingDefinitionManager.GetSettingDefinition(name);

            //Get for user if defined
            if (settingDefinition.Scopes.HasFlag(SettingScopes.User) && userId.HasValue)
            {
                var settingValue = await GetSettingValueForUserOrNullAsync(new UserIdentifier(tenantId, userId.Value), name);
                if (settingValue != null)
                {
                    return settingValue.Value;
                }

                if (!settingDefinition.IsInherited)
                {
                    return settingDefinition.DefaultValue;
                }
            }

            //Get for tenant if defined
            if (settingDefinition.Scopes.HasFlag(SettingScopes.Tenant) && tenantId.HasValue)
            {
                var settingValue = await GetSettingValueForTenantOrNullAsync(tenantId.Value, name);
                if (settingValue != null)
                {
                    return settingValue.Value;
                }

                if (!settingDefinition.IsInherited)
                {
                    return settingDefinition.DefaultValue;
                }
            }

            //Get for application if defined
            if (settingDefinition.Scopes.HasFlag(SettingScopes.Application))
            {
                var settingValue = await GetSettingValueForApplicationOrNullAsync(name);
                if (settingValue != null)
                {
                    return settingValue.Value;
                }
            }

            //Not defined, get default value
            return settingDefinition.DefaultValue;
        }

        /// <summary>
        /// 更新设置（增、删、改）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<SettingInfo> InsertOrUpdateOrDeleteSettingValueAsync(string name, string value, int? tenantId, long? userId)
        {
            var settingDefinition = _settingDefinitionManager.GetSettingDefinition(name);
            var settingValue = await SettingStore.GetSettingOrNullAsync(tenantId, userId, name);

            //Determine defaultValue
            var defaultValue = settingDefinition.DefaultValue;

            if (settingDefinition.IsInherited)
            {
                //For Tenant and User, Application's value overrides Setting Definition's default value.
                if (tenantId.HasValue || userId.HasValue)
                {
                    var applicationValue = await GetSettingValueForApplicationOrNullAsync(name);
                    if (applicationValue != null)
                    {
                        defaultValue = applicationValue.Value;
                    }
                }

                //For User, Tenants's value overrides Application's default value.
                if (userId.HasValue && tenantId.HasValue)
                {
                    var tenantValue = await GetSettingValueForTenantOrNullAsync(tenantId.Value, name);
                    if (tenantValue != null)
                    {
                        defaultValue = tenantValue.Value;
                    }
                }
            }

            //No need to store on database if the value is the default value
            if (value == defaultValue)
            {
                if (settingValue != null)
                {
                    await SettingStore.DeleteAsync(settingValue);
                }

                return null;
            }

            //If it's not default value and not stored on database, then insert it
            if (settingValue == null)
            {
                settingValue = new SettingInfo
                {
                    TenantId = tenantId,
                    UserId = userId,
                    Name = name,
                    Value = value
                };

                await SettingStore.CreateAsync(settingValue);
                return settingValue;
            }

            //It's same value in database, no need to update
            if (settingValue.Value == value)
            {
                return settingValue;
            }

            //Update the setting on database.
            settingValue.Value = value;
            await SettingStore.UpdateAsync(settingValue);

            return settingValue;
        }

        /// <summary>
        /// 获取应用设置值（如果定义了）
        /// </summary>
        /// <param name="name">设置名称</param>
        /// <returns></returns>
        private async Task<SettingInfo> GetSettingValueForApplicationOrNullAsync(string name)
        {
            return (await GetApplicationSettingsAsync()).GetOrDefault(name);
        }

        /// <summary>
        /// 获取租户设置值（如果定义了）
        /// </summary>
        /// <param name="tenantId">租户Id</param>
        /// <param name="name">设置名称</param>
        /// <returns></returns>
        private async Task<SettingInfo> GetSettingValueForTenantOrNullAsync(int tenantId, string name)
        {
            return (await GetReadOnlyTenantSettings(tenantId)).GetOrDefault(name);
        }

        /// <summary>
        /// 获取用户设置值（如果定义了）
        /// </summary>
        /// <param name="user">用户标识</param>
        /// <param name="name">设置名称</param>
        /// <returns></returns>
        private async Task<SettingInfo> GetSettingValueForUserOrNullAsync(UserIdentifier user, string name)
        {
            return (await GetReadOnlyUserSettings(user)).GetOrDefault(name);
        }

        /// <summary>
        /// 获取应用程序设置
        /// </summary>
        /// <returns></returns>
        private async Task<Dictionary<string, SettingInfo>> GetApplicationSettingsAsync()
        {
            return await _applicationSettingCache.GetAsync(ApplicationSettingsCacheKey, async () =>
            {
                var dictionary = new Dictionary<string, SettingInfo>();

                var settingValues = await SettingStore.GetAllListAsync(null, null);
                foreach (var settingValue in settingValues)
                {
                    dictionary[settingValue.Name] = settingValue;
                }

                return dictionary;
            });
        }

        /// <summary>
        /// 获取租户设置
        /// </summary>
        /// <param name="tenantId">租户ID</param>
        /// <returns></returns>
        private async Task<ImmutableDictionary<string, SettingInfo>> GetReadOnlyTenantSettings(int tenantId)
        {
            var cachedDictionary = await GetTenantSettingsFromCache(tenantId);
            lock (cachedDictionary)
            {
                return cachedDictionary.ToImmutableDictionary();
            }
        }

        /// <summary>
        /// 获取用户设置
        /// </summary>
        /// <param name="user">用户标识</param>
        /// <returns></returns>
        private async Task<ImmutableDictionary<string, SettingInfo>> GetReadOnlyUserSettings(UserIdentifier user)
        {
            var cachedDictionary = await GetUserSettingsFromCache(user);
            lock (cachedDictionary)
            {
                return cachedDictionary.ToImmutableDictionary();
            }
        }

        /// <summary>
        /// 从缓存获取租户设置
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        private async Task<Dictionary<string, SettingInfo>> GetTenantSettingsFromCache(int tenantId)
        {
            return await _tenantSettingCache.GetAsync(
                tenantId,
                async () =>
                {
                    var dictionary = new Dictionary<string, SettingInfo>();

                    var settingValues = await SettingStore.GetAllListAsync(tenantId, null);
                    foreach (var settingValue in settingValues)
                    {
                        dictionary[settingValue.Name] = settingValue;
                    }

                    return dictionary;
                });
        }

        /// <summary>
        /// 从缓存获取用户设置
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<Dictionary<string, SettingInfo>> GetUserSettingsFromCache(UserIdentifier user)
        {
            return await _userSettingCache.GetAsync(
                user.ToUserIdentifierString(),
                async () =>
                {
                    var dictionary = new Dictionary<string, SettingInfo>();

                    var settingValues = await SettingStore.GetAllListAsync(user.TenantId, user.UserId);
                    foreach (var settingValue in settingValues)
                    {
                        dictionary[settingValue.Name] = settingValue;
                    }

                    return dictionary;
                });
        }

        #endregion

        #region Nested classes 内嵌类

        /// <summary>
        /// 设置值对象
        /// </summary>
        private class SettingValueObject : ISettingValue
        {
            public string Name { get; private set; }

            public string Value { get; private set; }

            public SettingValueObject(string name, string value)
            {
                Value = value;
                Name = name;
            }
        }

        #endregion
    }
}