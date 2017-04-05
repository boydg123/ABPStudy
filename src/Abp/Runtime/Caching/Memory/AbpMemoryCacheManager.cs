using System.Runtime.Caching;
using Abp.Dependency;
using Abp.Runtime.Caching.Configuration;

namespace Abp.Runtime.Caching.Memory
{
    /// <summary>
    /// Implements <see cref="ICacheManager"/> to work with <see cref="MemoryCache"/>.
    /// 使用<see cref="MemoryCache"/>工作来实现<see cref="ICacheManager"/>
    /// </summary>
    public class AbpMemoryCacheManager : CacheManagerBase
    {
        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        public AbpMemoryCacheManager(IIocManager iocManager, ICachingConfiguration configuration)
            : base(iocManager, configuration)
        {
            IocManager.RegisterIfNot<AbpMemoryCache>(DependencyLifeStyle.Transient);
        }

        /// <summary>
        /// 用于创建实际缓存实现
        /// </summary>
        /// <param name="name">缓存的名称</param>
        /// <returns>缓存对象引用</returns>
        protected override ICache CreateCacheImplementation(string name)
        {
            return IocManager.Resolve<AbpMemoryCache>(new { name });
        }
    }
}
