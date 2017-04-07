using Abp.Configuration.Startup;
using Abp.Dependency;

namespace Abp.Runtime.Caching.Redis
{
    /// <summary>
    /// ABP Redis缓存选项扩展
    /// </summary>
    public static class AbpRedisCacheOptionsExtensions
    {
        /// <summary>
        /// 使用UseProto序列化
        /// </summary>
        /// <param name="options">ABP Redis缓存选项</param>
        public static void UseProtoBuf(this AbpRedisCacheOptions options)
        {
            options.AbpStartupConfiguration
                .ReplaceService<IRedisCacheSerializer, ProtoBufRedisCacheSerializer>(DependencyLifeStyle.Transient);
        }
    }
}
