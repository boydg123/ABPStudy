using System.Threading.Tasks;

namespace Abp.Application.Features
{
    /// <summary>
    /// ABP Zero功能值存储
    /// </summary>
    public interface IAbpZeroFeatureValueStore : IFeatureValueStore
    {
        /// <summary>
        /// 获取值(没有则返回Null)
        /// </summary>
        /// <param name="tenantId">租户ID</param>
        /// <param name="featureName">功能名称</param>
        /// <returns></returns>
        Task<string> GetValueOrNullAsync(int tenantId, string featureName);
        /// <summary>
        /// 获取版本值(没有则返回Null)
        /// </summary>
        /// <param name="editionId">租户ID</param>
        /// <param name="featureName">功能名称</param>
        /// <returns></returns>
        Task<string> GetEditionValueOrNullAsync(int editionId, string featureName);
        /// <summary>
        /// 设置版本功能值
        /// </summary>
        /// <param name="editionId">商户ID</param>
        /// <param name="featureName">功能名称</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        Task SetEditionFeatureValueAsync(int editionId, string featureName, string value);
    }
}