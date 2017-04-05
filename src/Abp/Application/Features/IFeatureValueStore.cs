using System.Threading.Tasks;

namespace Abp.Application.Features
{
    /// <summary>
    /// Defines a store to get feature values.
    /// 定义功能值的存储
    /// </summary>
    public interface IFeatureValueStore
    {
        /// <summary>
        /// Gets the feature value or null.
        /// 获取功能值或null
        /// </summary>
        /// <param name="tenantId">The tenant id. / 租户ID</param>
        /// <param name="feature">The feature. / 功能对象</param>
        Task<string> GetValueOrNullAsync(int tenantId, Feature feature);
    }
}