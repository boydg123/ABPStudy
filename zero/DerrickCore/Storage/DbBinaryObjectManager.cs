using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;

namespace Derrick.Storage
{
    /// <summary>
    /// <see cref="IBinaryObjectManager"/>实现，而机制对象管理器
    /// </summary>
    public class DbBinaryObjectManager : IBinaryObjectManager, ITransientDependency
    {
        /// <summary>
        /// 二进制仓储
        /// </summary>
        private readonly IRepository<BinaryObject, Guid> _binaryObjectRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="binaryObjectRepository">二进制仓储</param>
        public DbBinaryObjectManager(IRepository<BinaryObject, Guid> binaryObjectRepository)
        {
            _binaryObjectRepository = binaryObjectRepository;
        }

        /// <summary>
        /// 获取二进制对象或Null - 异步
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns></returns>
        public Task<BinaryObject> GetOrNullAsync(Guid id)
        {
            return _binaryObjectRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 保存二进制对象
        /// </summary>
        /// <param name="file">二进制对象</param>
        /// <returns></returns>
        public Task SaveAsync(BinaryObject file)
        {
            return _binaryObjectRepository.InsertAsync(file);
        }

        /// <summary>
        /// 通过ID删除二进制对象
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns></returns>
        public Task DeleteAsync(Guid id)
        {
            return _binaryObjectRepository.DeleteAsync(id);
        }
    }
}