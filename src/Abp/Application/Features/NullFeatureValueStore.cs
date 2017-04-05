using System.Threading.Tasks;

namespace Abp.Application.Features
{
    /// <summary>
    /// Null pattern (default) implementation of <see cref="IFeatureValueStore"/>.It gets null for all feature values.
    /// <see cref="IFeatureValueStore"/>null 模式(默认)的实现，为所有的功能值获取null
    /// <see cref="Instance"/> can be used via property injection of <see cref="IFeatureValueStore"/>.
    /// <see cref="Instance"/>可以用<see cref="IFeatureValueStore"/>属性注入
    /// </summary>
    public class NullFeatureValueStore : IFeatureValueStore
    {
        /// <summary>
        /// Gets the singleton instance.
        /// 获取单例
        /// </summary>
        public static NullFeatureValueStore Instance { get { return SingletonInstance; } }
        private static readonly NullFeatureValueStore SingletonInstance = new NullFeatureValueStore();

        /// <summary>
        /// 获取功能值或null
        /// </summary>
        /// <param name="tenantId">The tenant id. / 租户ID</param>
        /// <param name="feature">The feature. / 功能对象</param>
        /// <returns></returns>
        public Task<string> GetValueOrNullAsync(int tenantId, Feature feature)
        {
            return Task.FromResult((string) null);
        }
    }
}