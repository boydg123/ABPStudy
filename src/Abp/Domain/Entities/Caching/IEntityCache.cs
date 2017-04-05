using System.Threading.Tasks;
using Abp.Runtime.Caching;

namespace Abp.Domain.Entities.Caching
{
    /// <summary>
    /// 主键为 <see cref="int"/> 类型的实体缓存
    /// </summary>
    /// <typeparam name="TCacheItem">缓存项</typeparam>
    public interface IEntityCache<TCacheItem> : IEntityCache<TCacheItem, int>
    {

    }

    /// <summary>
    /// 实体缓存接口
    /// </summary>
    /// <typeparam name="TCacheItem">缓存项</typeparam>
    /// <typeparam name="TPrimaryKey">主键</typeparam>
    public interface IEntityCache<TCacheItem, TPrimaryKey>
    {
        TCacheItem this[TPrimaryKey id] { get; }

        string CacheName { get; }

        ITypedCache<TPrimaryKey, TCacheItem> InternalCache { get; }

        TCacheItem Get(TPrimaryKey id);

        Task<TCacheItem> GetAsync(TPrimaryKey id);
    }
}