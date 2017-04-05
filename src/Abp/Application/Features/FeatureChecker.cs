using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Runtime.Session;

namespace Abp.Application.Features
{
    /// <summary>
    /// Default implementation for <see cref="IFeatureChecker"/>.
    /// <see cref="IFeatureChecker"/>的实现
    /// </summary>
    public class FeatureChecker : IFeatureChecker, ITransientDependency
    {
        /// <summary>
        /// Reference to current session.
        /// 当前回话的引用
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        /// <summary>
        /// Reference to the store used to get feature values.
        /// 存储用于获取功能值的引用
        /// </summary>
        public IFeatureValueStore FeatureValueStore { get; set; }

        /// <summary>
        /// 功能管理器
        /// </summary>
        private readonly IFeatureManager _featureManager;

        /// <summary>
        /// Creates a new <see cref="FeatureChecker"/> object.
        /// 构造函数
        /// </summary>
        public FeatureChecker(IFeatureManager featureManager)
        {
            _featureManager = featureManager;

            FeatureValueStore = NullFeatureValueStore.Instance;
            AbpSession = NullAbpSession.Instance;
        }

        /// <summary>
        /// 通过功能名称获取值,使用<see cref="IAbpSession.TenantId"/>,所以，只有从Session中获取租户ID才能使用此方法
        /// </summary>
        /// <param name="name">功能的唯一名称</param>
        /// <returns>功能的当前值</returns>
        public Task<string> GetValueAsync(string name)
        {
            if (!AbpSession.TenantId.HasValue)
            {
                throw new AbpException("FeatureChecker can not get a feature value by name. TenantId is not set in the IAbpSession!");
            }

            return GetValueAsync(AbpSession.TenantId.Value, name);
        }

        /// <summary>
        /// 为一个租户的功能名称获取功能值
        /// </summary>
        /// <param name="tenantId">租户的ID</param>
        /// <param name="name">功能的唯一名称</param>
        /// <returns>功能的当前值</returns>
        public async Task<string> GetValueAsync(int tenantId, string name)
        {
            var feature = _featureManager.Get(name);

            var value = await FeatureValueStore.GetValueOrNullAsync(tenantId, feature);
            if (value == null)
            {
                return feature.DefaultValue;
            }

            return value;
        }
    }
}