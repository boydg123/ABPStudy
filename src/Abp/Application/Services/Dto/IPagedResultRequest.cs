namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// This interface is defined to standardize to request a paged result.
    /// 此接口定义请求结果集中的一页
    /// </summary>
    public interface IPagedResultRequest : ILimitedResultRequest
    {
        /// <summary>
        /// Skip count (beginning of the page).
        /// 跳过的数量
        /// </summary>
        int SkipCount { get; set; }
    }
}