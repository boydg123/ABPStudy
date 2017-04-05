using System.Collections.Generic;

namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// This interface is defined to standardize to return a list of items to clients.
    /// 此接口定义返回客户端的表列项
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="Items"/> list / <see cref="Items"/>列表中项的类型</typeparam>
    public interface IListResult<T>
    {
        /// <summary>
        /// List of items.
        /// 列表
        /// </summary>
        IReadOnlyList<T> Items { get; set; }
    }
}