using Abp.Application.Services.Dto;

namespace Derrick.Dto
{
    /// <summary>
    /// 分页以及排序Input Dto
    /// </summary>
    public class PagedAndSortedInputDto : PagedInputDto, ISortedResultRequest
    {
        /// <summary>
        /// 排序条件
        /// </summary>
        public string Sorting { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PagedAndSortedInputDto()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
    }
}