using System;
using System.Collections.Generic;

namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// Implements <see cref="IPagedResult{T}"/>.
    /// 接口<see cref="IPagedResult{T}"/>的实现
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="ListResultDto{T}.Items"/> list / <see cref="ListResultDto{T}.Items"/>中项的类型</typeparam>
    [Serializable]
    public class PagedResultDto<T> : ListResultDto<T>, IPagedResult<T>
    {
        /// <summary>
        /// Total count of Items.
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Creates a new <see cref="PagedResultDto{T}"/> object.
        /// 构造函数
        /// </summary>
        public PagedResultDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="PagedResultDto{T}"/> object.
        /// 构造函数
        /// </summary>
        /// <param name="totalCount">Total count of Items / 项的总数</param>
        /// <param name="items">List of items in current page / 项的总数</param>
        public PagedResultDto(int totalCount, IReadOnlyList<T> items)
            : base(items)
        {
            TotalCount = totalCount;
        }
    }
}