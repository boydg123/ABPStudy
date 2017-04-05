using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Configuration
{
    /// <summary>
    /// This interface is used to get/set settings from/to a data source (database).
    /// 此接口用于从数据源（数据库）中获取设置值或将设置值保存到数据源中
    /// </summary>
    public interface ISettingStore
    {
        /// <summary>
        /// Gets a setting or null.
        /// 获取一个设置值或null
        /// </summary>
        /// <param name="tenantId">TenantId or null / 租户Id或Null</param>
        /// <param name="userId">UserId or null / 用户Id或Null</param>
        /// <param name="name">Name of the setting / 设置的名称</param>
        /// <returns>Setting object / 设置的名称</returns>
        Task<SettingInfo> GetSettingOrNullAsync(int? tenantId, long? userId, string name);

        /// <summary>
        /// Deletes a setting.
        /// 删除一个设置
        /// </summary>
        /// <param name="setting">Setting to be deleted / 将被删除的设置</param>
        Task DeleteAsync(SettingInfo setting);

        /// <summary>
        /// Adds a setting.
        /// 添加一个设置
        /// </summary>
        /// <param name="setting">Setting to add / 将被添加的设置</param>
        Task CreateAsync(SettingInfo setting);

        /// <summary>
        /// Update a setting.
        /// 更新一个设置
        /// </summary>
        /// <param name="setting">Setting to add / 将被更新的设置</param>
        Task UpdateAsync(SettingInfo setting);

        /// <summary>
        /// Gets a list of setting.
        /// 获取设置列表
        /// </summary>
        /// <param name="tenantId">TenantId or null / 租户Id或Null</param>
        /// <param name="userId">UserId or null / 用户Id或Null</param>
        /// <returns>List of settings / 设置列表</returns>
        Task<List<SettingInfo>> GetAllListAsync(int? tenantId, long? userId);
    }
}