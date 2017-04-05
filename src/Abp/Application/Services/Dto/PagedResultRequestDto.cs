using System;
using System.ComponentModel.DataAnnotations;

namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// Simply implements <see cref="IPagedResultRequest"/>.
    /// <see cref="IPagedResultRequest"/>的简单实现
    /// </summary>
    [Serializable]
    public class PagedResultRequestDto : LimitedResultRequestDto, IPagedResultRequest
    {
        /// <summary>
        /// 跳过数量
        /// </summary>
        [Range(0, int.MaxValue)]
        public virtual int SkipCount { get; set; }
    }
}