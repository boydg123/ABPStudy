namespace Abp.MemoryDb
{
    /// <summary>
    /// Defines interface to obtain a <see cref="MemoryDatabase"/> object.
    /// 定义一个接口用于获得一个<see cref="MemoryDatabase"/>对象
    /// </summary>
    public interface IMemoryDatabaseProvider
    {
        /// <summary>
        /// Gets the <see cref="MemoryDatabase"/>.
        /// 获取<see cref="MemoryDatabase"/>对象
        /// </summary>
        MemoryDatabase Database { get; }
    }
}