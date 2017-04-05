using System;

namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// Simply implements <see cref="IPagedAndSortedResultRequest"/>.
    /// <see cref="IPagedAndSortedResultRequest"/>¼òµ¥ÊµÏÖ
    /// </summary>
    [Serializable]
    public class PagedAndSortedResultRequestDto : PagedResultRequestDto, IPagedAndSortedResultRequest
    {
        /// <summary>
        /// ÅÅÐò×Ö·û´®
        /// </summary>
        public virtual string Sorting { get; set; }
    }
}