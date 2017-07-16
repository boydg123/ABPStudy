using System;
using System.Threading.Tasks;

namespace Derrick.Storage
{
    /// <summary>
    /// 二进制对象管理器
    /// </summary>
    public interface IBinaryObjectManager
    {
        /// <summary>
        /// 获取二进制对象或Null - 异步
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns></returns>
        Task<BinaryObject> GetOrNullAsync(Guid id);
        
        /// <summary>
        /// 保存二进制对象
        /// </summary>
        /// <param name="file">二进制对象</param>
        /// <returns></returns>
        Task SaveAsync(BinaryObject file);

        /// <summary> 
        /// 通过ID删除二进制对象
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);
    }
}