using Abp.Dependency;
using Abp.Runtime.Caching.Configuration;

namespace Abp.Runtime.Caching.Redis
{
    /// <summary>
    /// Used to create <see cref="AbpRedisCache"/> instances.
    /// 用于创建<see cref="AbpRedisCache"/>实例
    /// </summary>
    public class AbpRedisCacheManager : CacheManagerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbpRedisCacheManager"/> class.
        /// 构造函数
        /// </summary>
        public AbpRedisCacheManager(IIocManager iocManager, ICachingConfiguration configuration)
            : base(iocManager, configuration)
        {
            IocManager.RegisterIfNot<AbpRedisCache>(DependencyLifeStyle.Transient);
        }

        /// <summary>
        /// 创建缓存实现
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        protected override ICache CreateCacheImplementation(string name)
        {
            return IocManager.Resolve<AbpRedisCache>(new { name });
        }
    }
}
