using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Abp.Logging;

namespace Abp.Configuration
{
    /// <summary>
    /// Implements default behavior for ISettingStore.Only <see cref="GetSettingOrNullAsync"/> method is implemented and it gets setting's value from application's configuration file if exists, or returns null if not.
    /// 接口IsettingStroe的默认实现，只实现了<see cref="GetSettingOrNullAsync"/>方法，如果存在配置文件,它从应用的配置文件中获取设置值,否则返回null;
    /// </summary>
    public class DefaultConfigSettingStore : ISettingStore
    {
        /// <summary>
        /// Gets singleton instance.
        /// 单例对象
        /// </summary>
        public static DefaultConfigSettingStore Instance { get { return SingletonInstance; } }
        private static readonly DefaultConfigSettingStore SingletonInstance = new DefaultConfigSettingStore();

        /// <summary>
        /// 构造函数
        /// </summary>
        private DefaultConfigSettingStore()
        {
        }

        /// <summary>
        /// 异步获取设置
        /// </summary>
        /// <param name="tenantId">租户ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public Task<SettingInfo> GetSettingOrNullAsync(int? tenantId, long? userId, string name)
        {
            var value = ConfigurationManager.AppSettings[name];

            if (value == null)
            {
                return Task.FromResult<SettingInfo>(null);
            }

            return Task.FromResult(new SettingInfo(tenantId, userId, name, value));
        }
#pragma warning disable 1998
        /// <summary>
        /// 异步删除
        /// </summary>
        /// <param name="setting">设置信息</param>
        /// <returns></returns>
        public async Task DeleteAsync(SettingInfo setting)
        {
            //TODO: Call should be async and use await
            LogHelper.Logger.Warn("ISettingStore is not implemented, using DefaultConfigSettingStore which does not support DeleteAsync.");
        }
#pragma warning restore 1998

#pragma warning disable 1998
        /// <summary>
        /// 异步创建
        /// </summary>
        /// <param name="setting">设置信息</param>
        /// <returns></returns>
        public async Task CreateAsync(SettingInfo setting)
        {
            //TODO: Call should be async and use await
            LogHelper.Logger.Warn("ISettingStore is not implemented, using DefaultConfigSettingStore which does not support CreateAsync.");
        }
#pragma warning restore 1998

#pragma warning disable 1998
        /// <summary>
        /// 异步更新
        /// </summary>
        /// <param name="setting">设置信息</param>
        /// <returns></returns>
        public async Task UpdateAsync(SettingInfo setting)
        {
            //TODO: Call should be async and use await
            LogHelper.Logger.Warn("ISettingStore is not implemented, using DefaultConfigSettingStore which does not support UpdateAsync.");
        }
#pragma warning restore 1998

        /// <summary>
        /// 异步获取所有元素
        /// </summary>
        /// <param name="tenantId">租户ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public Task<List<SettingInfo>> GetAllListAsync(int? tenantId, long? userId)
        {
            LogHelper.Logger.Warn("ISettingStore is not implemented, using DefaultConfigSettingStore which does not support GetAllListAsync.");
            return Task.FromResult(new List<SettingInfo>());
        }
    }
}