namespace Derrick.Dto
{
    /// <summary>
    /// 分页排序已经过滤Input Dto
    /// </summary>
    public class PagedSortedAndFilteredInputDto : PagedAndSortedInputDto
    {
        /// <summary>
        /// 过滤条件
        /// </summary>
        public string Filter { get; set; }
    }
}